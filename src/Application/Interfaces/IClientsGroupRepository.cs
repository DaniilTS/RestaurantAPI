using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IClientsGroupRepository
    {
        Task<ClientsGroup> GetObject(Guid id);
        Task<List<ClientsGroup>> GetCollection();
        Task<List<ClientsGroup>> GetCollection(ClientsGroupStatus clientsGroupStatus);
        Task Create(ClientsGroup clientsGroup);
        Task UpdateObject(ClientsGroup clientsGroup);
    }
}
