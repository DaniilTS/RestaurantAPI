using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITableRepository
    {
        Task<List<Table>> GetCollection();
        Task UpdateObject(Table table);
    }
}
