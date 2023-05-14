using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Helpers
{
    public static class RestManagerHelper
    {
        public static SynchronizedCollection<Table> InitTables(this SynchronizedCollection<Table> tables, IServiceScopeFactory scopeFactory)
        {
            using var scope = scopeFactory.CreateScope();
            var tableRepository = scope.ServiceProvider.GetRequiredService<ITableRepository>();

            return new SynchronizedCollection<Table>(tables.SyncRoot, tableRepository.GetCollection().Result);
        }

        public static SynchronizedCollection<ClientsGroup> InitQueue(this SynchronizedCollection<ClientsGroup> clientsGroupQueue, IServiceScopeFactory scopeFactory)
        {
            using var scope = scopeFactory.CreateScope();
            var clientsGroupRepository = scope.ServiceProvider.GetRequiredService<IClientsGroupRepository>();

            var queue = clientsGroupRepository.GetCollection(ClientsGroupStatus.InQueue).Result.OrderBy(x => x.ArrivalTime);

            return new SynchronizedCollection<ClientsGroup>(clientsGroupQueue.SyncRoot, queue);
        }

        public static Table GetFreeTable(this SynchronizedCollection<Table> tables, int amountOfGuests)
        {
            var fullyFreeTable = tables.Where(table => table.FreeSpace == table.Size && table.Size >= amountOfGuests).MinBy(x => x.FreeSpace);
            if (fullyFreeTable != null)
                return fullyFreeTable;

            var partiallyFilledTable = tables.Where(table => table.FreeSpace >= amountOfGuests).MinBy(x => x.FreeSpace);
            if (partiallyFilledTable != null)
                return partiallyFilledTable;

            return null;
        }

        public static async Task FreeTable(IServiceScopeFactory scopeFactory, Table table, ClientsGroup clientsGroup)
        {
            table.FreeSpace += clientsGroup.Size;
            table.ClientsGroups.FirstOrDefault(x => x.Id == clientsGroup.Id).Status = ClientsGroupStatus.Served.ToString();

            await UpdateTable(scopeFactory, table);
        }

        public static async Task IncreaseBoredIndexInQueue(this SynchronizedCollection<ClientsGroup> queue,
            IServiceScopeFactory scopeFactory, int servedClientsGroupIndex, Func<ClientsGroup, Semaphore, Task> onLeaveTask, Semaphore semaphore = null)
        {
            var leavingClientsGroups = new List<ClientsGroup>();

            for (var i = 0; i < servedClientsGroupIndex; i++)
            {
                var clientGroup = queue[i];

                clientGroup.IncreaseBoredIndex();
                await UpdateClientsGroup(scopeFactory, clientGroup);

                if (clientGroup.BoredIndex == clientGroup.MaxBoaredIndex)
                {
                    leavingClientsGroups.Add(clientGroup);
                }
            }

            foreach (var cg in leavingClientsGroups)
            {
                semaphore ??= new Semaphore(1, 1);//prevent dead lock
                await onLeaveTask(cg, semaphore);
            }
        }

        public static async Task UpdateTable(IServiceScopeFactory scopeFactory, Table table)
        {
            using var scope = scopeFactory.CreateScope();
            var tableRepository = scope.ServiceProvider.GetRequiredService<ITableRepository>();

            await tableRepository.UpdateObject(table);
        }

        public static async Task<ClientsGroup> GetClientsGroup(IServiceScopeFactory scopeFactory, Guid clientsGroupId)
        {
            using var scope = scopeFactory.CreateScope();
            var clientsGroupRepository = scope.ServiceProvider.GetRequiredService<IClientsGroupRepository>();

            return await clientsGroupRepository.GetObject(clientsGroupId);
        }

        public static async Task AddClientsGroup(IServiceScopeFactory scopeFactory, ClientsGroup clientsGroup, ClientsGroupStatus clientsGroupStatus)
        {
            clientsGroup.Status = clientsGroupStatus.ToString();

            await AddClientsGroup(scopeFactory, clientsGroup);
        }

        public static async Task AddClientsGroup(IServiceScopeFactory scopeFactory, ClientsGroup clientsGroup)
        {
            using var scope = scopeFactory.CreateScope();
            var clientsGroupRepository = scope.ServiceProvider.GetRequiredService<IClientsGroupRepository>();

            await clientsGroupRepository.Create(clientsGroup);
        }

        public static async Task UpdateClientsGroupStatus(IServiceScopeFactory scopeFactory, ClientsGroup clientsGroup, ClientsGroupStatus clientsGroupStatus)
        {
            clientsGroup.Status = clientsGroupStatus.ToString();

            await UpdateClientsGroup(scopeFactory, clientsGroup);
        }

        public static async Task UpdateClientsGroup(IServiceScopeFactory scopeFactory, ClientsGroup clientsGroup)
        {
            using var scope = scopeFactory.CreateScope();
            var clientsGroupRepository = scope.ServiceProvider.GetRequiredService<IClientsGroupRepository>();

            await clientsGroupRepository.UpdateObject(clientsGroup);
        }
    }
}
