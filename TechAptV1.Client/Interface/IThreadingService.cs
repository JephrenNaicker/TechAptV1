// Copyright © 2025 Always Active Technologies PTY Ltd

namespace TechAptV1.Client.Interface
{
    public interface IThreadingService
    {
        int GetOddNumbers();
        int GetEvenNumbers();
        int GetPrimeNumbers();
        int GetTotalNumbers();
        bool IsGenerationComplete();
        List<int> GetNumbers();

        Task Start();
        Task Stop();
        Task Save();
    }
}
