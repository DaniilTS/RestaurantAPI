using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class ClientsGroupRepository : IClientsGroupRepository
    {
        private readonly DbContext _dbContext;

        public ClientsGroupRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(ClientsGroup clientsGroup)
        {
            await _dbContext.ClientsGroups.AddAsync(clientsGroup);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ClientsGroup>> GetCollection()
        {
            return await _dbContext.ClientsGroups.ToListAsync();
        }

        public async Task<List<ClientsGroup>> GetCollection(ClientsGroupStatus clientsGroupStatus)
        {
            return await _dbContext.ClientsGroups.Where(x => x.Status == clientsGroupStatus.ToString()).Include(x => x.Table).ToListAsync();
        }

        public async Task<ClientsGroup> GetObject(Guid id)
        {
            return await _dbContext.ClientsGroups.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateObject(ClientsGroup clientsGroup)
        {
            _dbContext.ClientsGroups.Update(clientsGroup);
            await _dbContext.SaveChangesAsync();
        }
    }
}
