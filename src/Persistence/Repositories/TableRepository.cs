using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class TableRepository: ITableRepository
    {
        private readonly DbContext _dbContext;

        public TableRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Table>> GetCollection()
        {
            return await _dbContext.Tables.Include(x => x.ClientsGroups).ToListAsync();
        }

        public async Task UpdateObject(Table table)
        {
            _dbContext.Tables.Update(table);
            await _dbContext.SaveChangesAsync();
        }
    }
}
