// Copyright © 2025 Always Active Technologies PTY Ltd

using TechAptV1.Client.Models;

namespace TechAptV1.Client.Interface
{
    public interface IDataService
    {
        Task Save(List<Number> dataList);
        IEnumerable<Number> Get(int count);
        IEnumerable<Number> GetAll();
    }
}
