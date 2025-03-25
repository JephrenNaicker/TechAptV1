// Copyright © 2025 Always Active Technologies PTY Ltd

using TechAptV1.Client.Interface;
using TechAptV1.Client.Models;

namespace TechAptV1.Client.Services
{
    public sealed class ThreadingService : IThreadingService
    {
        private readonly ILogger<ThreadingService> _logger;
        private readonly IDataService _dataService;
        private readonly List<Number> _sharedNumbers = new List<Number>();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1); // Thread sync
        private CancellationTokenSource _cts = new();
        private readonly NotificationService _notifications;

        // Counters for odd, even, prime, and total numbers
        private int _oddNumbers = 0;
        private int _evenNumbers = 0;
        private int _primeNumbers = 0;
        private int _totalNumbers = 0;
        private readonly int _maxNumbers = 10000000;
        private readonly int _threadLimit = 2500000;
        public ThreadingService(ILogger<ThreadingService> logger, IDataService dataService, NotificationService notifications)
        {
            _logger = logger;
            _dataService = dataService;
            _notifications = notifications;
        }

        // Methods to get the counters
        public int GetOddNumbers() => _oddNumbers;
        public int GetEvenNumbers() => _evenNumbers;
        public int GetPrimeNumbers() => _primeNumbers;
        public int GetTotalNumbers() => _totalNumbers;
        public bool IsGenerationComplete() => _totalNumbers >= _maxNumbers;
        public async Task Start()
        {
            try
            {
                _logger.LogInformation("Start");
                await _notifications.ShowSuccessAsync("Number generation started!");
                _cts = new CancellationTokenSource();
                // Thread 1: odd numbers
                var oddThread = Task.Run(() => GenerateOddNumbers(_cts.Token));

                // Thread 2: prime numbers
                var primeThread = Task.Run(() => GeneratePrimeNumbers(_cts.Token));

                //Thread 3 when it reaches entries
                var monitorThread = Task.Run(() => MonitorSharedList(_cts.Token));
            }
            catch (Exception ex)
            {
                await _notifications.ShowErrorAsync($"Failed to start: {ex.Message}");
            }
        }

        private async Task GenerateOddNumbers(CancellationToken token)
        {
            var random = new Random();
            while (!token.IsCancellationRequested)
            {
                int oddNumber = GenerateRandomOddNumber(random);
                var number = new Number { Value = oddNumber, IsPrime = 0 };

                await _semaphore.WaitAsync();
                _sharedNumbers.Add(number);
                _oddNumbers++;
                _totalNumbers++;
                _semaphore.Release();

                _logger.LogInformation($"Added odd number: {oddNumber}. Total odd numbers: {_oddNumbers}, Total numbers: {_totalNumbers}");
            }
        }

        private async Task GeneratePrimeNumbers(CancellationToken token)
        {
            var random = new Random();
            while (!token.IsCancellationRequested)
            {
                int primeNumber = GenerateRandomPrimeNumber(random);
                var number = new Number { Value = -primeNumber, IsPrime = 1 };

                await _semaphore.WaitAsync(); // Acquire lock
                _sharedNumbers.Add(number); // Add to shared list
                _primeNumbers++;
                _totalNumbers++;
                _semaphore.Release(); // Release lock

                _logger.LogInformation($"Added prime number: {-primeNumber}. Total prime numbers: {_primeNumbers}, Total numbers: {_totalNumbers}");
            }
        }

        private async Task MonitorSharedList(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await _semaphore.WaitAsync(); // Acquire lock
                if (_sharedNumbers.Count >= _threadLimit)
                {
                    // Start Thread 3: Generate even numbers
                    var evenThread = Task.Run(() => GenerateEvenNumbers(token));

                    _cts.Cancel();  // Stop Thread 1 and Thread 2

                }
                _semaphore.Release(); // Release lock

       
            }
        }

        private async Task GenerateEvenNumbers(CancellationToken token)
        {
            var random = new Random();
            while (_sharedNumbers.Count < _maxNumbers)
            {
                int evenNumber = GenerateRandomEvenNumber(random);
                var number = new Number { Value = evenNumber, IsPrime = 0 };

                await _semaphore.WaitAsync(); // Acquire lock
                _sharedNumbers.Add(number); // Add to shared list
                _evenNumbers++;
                _totalNumbers++;
                _semaphore.Release(); // Release lock

                _logger.LogInformation($"Added even number: {evenNumber}. Total even numbers: {_evenNumbers}, Total numbers: {_totalNumbers}");
            }

            _logger.LogInformation($"Reached {_maxNumbers} entries. Stopping all threads.");
        }

        private int GenerateRandomOddNumber(Random random)
        {
            return random.Next(0, 500) * 2 + 1;
        }

        private int GenerateRandomEvenNumber(Random random)
        {
            return random.Next(0, 500) * 2;
        }

        private int GenerateRandomPrimeNumber(Random random)
        {
            int number;
            do
            {
                number = random.Next(2, 1000);
            } while (!IsPrime(number));

            return number;
        }

        private bool IsPrime(int number)
        {
            if (number < 2)
                return false;

            for (int i = 2; i * i <= number; i++)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }

        public List<Number> SortNumbers(List<Number> numbers)
        {
            return numbers.OrderBy(n => n.Value).ToList();
        }

        public async Task Save()
        {
            try
            {
                _logger.LogInformation("Save");
                var sortedNumbers = SortNumbers(_sharedNumbers);
                await _dataService.Save(sortedNumbers);
                await _notifications.ShowSuccessAsync("Data saved successfully!");
            }
            catch (Exception ex)
            {

                _logger.LogInformation($"Save failed: {ex.Message}");
                await _notifications.ShowErrorAsync($"Save failed: {ex.Message}");
            }
        }

        public async Task<byte[]> GetBinaryData()
        {
            var numbers = await _dataService.GetAll();
            numbers = numbers.OrderBy(n => n.Value).ToList();

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            foreach (var number in numbers)
            {
                writer.Write(number.Value);     // 4-byte int
                writer.Write((byte)number.IsPrime); // 1-byte boolean
            }
            return stream.ToArray();
        }

    }

}
