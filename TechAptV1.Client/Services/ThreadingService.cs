using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TechAptV1.Client.Interface;
using TechAptV1.Client.Models;

namespace TechAptV1.Client.Services
{
    public sealed class ThreadingService : IThreadingService
    {
        private readonly ILogger<ThreadingService> _logger;
        private readonly IDataService _dataService;
        private readonly List<Number> _sharedNumbers = new List<Number>(); // Shared global variable
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1); // Thread synchronization
        private bool _shouldStop = false; // Flag to stop threads

        // Counters for odd, even, prime, and total numbers
        private int _oddNumbers = 0;
        private int _evenNumbers = 0;
        private int _primeNumbers = 0;
        private int _totalNumbers = 0;

        public ThreadingService(ILogger<ThreadingService> logger, IDataService dataService) 
        {
            _logger = logger;
            _dataService = dataService;
        }

        // Methods to get the counters
        public int GetOddNumbers() => _oddNumbers;
        public int GetEvenNumbers() => _evenNumbers;
        public int GetPrimeNumbers() => _primeNumbers;
        public int GetTotalNumbers() => _totalNumbers;
        public bool IsGenerationComplete() => _totalNumbers >= 40;
        public async Task Start()
        {
            _logger.LogInformation("Start");

            // Start Thread 1: Generate odd numbers
            var oddThread = new Thread(GenerateOddNumbers);
            oddThread.Start();

            // Start Thread 2: Generate prime numbers
            var primeThread = new Thread(GeneratePrimeNumbers);
            primeThread.Start();

            // Monitor the shared list and start Thread 3 when it reaches 10 entries
            var monitorThread = new Thread(MonitorSharedList);
            monitorThread.Start();
        }

        private async void GenerateOddNumbers()
        {
            var random = new Random();
            while (!_shouldStop)
            {
                int oddNumber = GenerateRandomOddNumber(random);
                var number = new Number { Value = oddNumber, IsPrime = 0 };

                await _semaphore.WaitAsync(); // Acquire lock
                _sharedNumbers.Add(number); // Add to shared list
                _oddNumbers++; // Increment odd number counter
                _totalNumbers++; // Increment total number counter
                _semaphore.Release(); // Release lock

                _logger.LogInformation($"Added odd number: {oddNumber}. Total odd numbers: {_oddNumbers}, Total numbers: {_totalNumbers}");

                await Task.Delay(500); // Simulate some delay
            }
        }

        private async void GeneratePrimeNumbers()
        {
            var random = new Random();
            while (!_shouldStop)
            {
                int primeNumber = GenerateRandomPrimeNumber(random);
                var number = new Number { Value = -primeNumber, IsPrime = 1 };

                await _semaphore.WaitAsync(); // Acquire lock
                _sharedNumbers.Add(number); // Add to shared list
                _primeNumbers++; // Increment prime number counter
                _totalNumbers++; // Increment total number counter
                _semaphore.Release(); // Release lock

                _logger.LogInformation($"Added prime number: {-primeNumber}. Total prime numbers: {_primeNumbers}, Total numbers: {_totalNumbers}");

                await Task.Delay(500); // Simulate some delay
            }
        }

        private async void MonitorSharedList()
        {
            while (!_shouldStop)
            {
                await _semaphore.WaitAsync(); // Acquire lock
                if (_sharedNumbers.Count >= 10 && !_shouldStop)
                {
                    // Start Thread 3: Generate even numbers
                    var evenThread = new Thread(GenerateEvenNumbers);
                    evenThread.Start();
                    _shouldStop = true; // Stop Thread 1 and Thread 2
                }
                _semaphore.Release(); // Release lock

                await Task.Delay(100); // Check the list every 100ms
            }
        }

        private async void GenerateEvenNumbers()
        {
            var random = new Random();
            while (_sharedNumbers.Count < 40)
            {
                int evenNumber = GenerateRandomEvenNumber(random);
                var number = new Number { Value = evenNumber, IsPrime = 0 };

                await _semaphore.WaitAsync(); // Acquire lock
                _sharedNumbers.Add(number); // Add to shared list
                _evenNumbers++; // Increment even number counter
                _totalNumbers++; // Increment total number counter
                _semaphore.Release(); // Release lock

                _logger.LogInformation($"Added even number: {evenNumber}. Total even numbers: {_evenNumbers}, Total numbers: {_totalNumbers}");

                await Task.Delay(500); // Simulate some delay
            }

            _logger.LogInformation("Reached 40 entries. Stopping all threads.");
        }

        private int GenerateRandomOddNumber(Random random)
        {
            int number;
            do
            {
                number = random.Next(1, 100);
            } while (number % 2 == 0); // Ensure the number is odd

            return number;
        }

        private int GenerateRandomEvenNumber(Random random)
        {
            int number;
            do
            {
                number = random.Next(1, 100);
            } while (number % 2 != 0); // Ensure the number is even

            return number;
        }

        private int GenerateRandomPrimeNumber(Random random)
        {
            int number;
            do
            {
                number = random.Next(1, 100);
            } while (!IsPrime(number)); // Ensure the number is prime

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

        public async Task Save()
        {
            _logger.LogInformation("Save");
            await _dataService.Save(_sharedNumbers);
        }

        public List<int> GetNumbers() => throw new NotImplementedException();
        public Task Stop() => throw new NotImplementedException();
    }
}
