// Copyright © 2025 Always Active Technologies PTY Ltd

using TechAptV1.Client.Models;

namespace TechAptV1.Client.Interface
{
    public interface IThreadingService
    {
        int GetOddNumbers();
        int GetEvenNumbers();
        int GetPrimeNumbers();
        int GetTotalNumbers();
        bool IsGenerationComplete();
        Task<byte[]> GetBinaryData();
        List<Number> SortNumbers(List<Number> numbers);
        Task Start();
        Task Save();
    }
}
