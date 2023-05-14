using Application.Helpers;
using Domain.Entities;
using Domain.Enums;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    public class RestManager
    {
        private readonly IServiceScopeFactory _scopeFactory;

        static readonly Semaphore semOnArrive = new(1, 1);
        static readonly Semaphore semOnLeave = new(1, 1);

        private readonly SynchronizedCollection<Table> _tables = new();
        private readonly SynchronizedCollection<ClientsGroup> _clientsGroupQueue = new();

        public RestManager(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

            _clientsGroupQueue = _clientsGroupQueue.InitQueue(scopeFactory);
            _tables = _tables.InitTables(scopeFactory);
        }

        public async Task OnArrive(ClientsGroup clientsGroup)
        {
            semOnArrive.WaitOne();

            var freeTable = _tables.GetFreeTable(clientsGroup.Size);
            if (freeTable != null)
            {
                await SitClientsGroup(freeTable, clientsGroup);
                await _clientsGroupQueue.IncreaseBoredIndexInQueue(_scopeFactory, _clientsGroupQueue.Count, OnLeaveConcurrent, semOnLeave);
            }
            else
            {
                _clientsGroupQueue.Add(clientsGroup);
                await RestManagerHelper.AddClientsGroup(_scopeFactory, clientsGroup, ClientsGroupStatus.InQueue);
            }

            semOnArrive.Release();
        }

        public Table LookUp(ClientsGroup clientsGroup)
        {
            return _tables.FirstOrDefault(table => table.ClientsGroups.Any(cg => cg.Id == clientsGroup.Id));
        }

        public async Task OnLeave(ClientsGroup clientsGroup)
        {
            var clientsGroupTable = LookUp(clientsGroup);
            if (clientsGroupTable != null)
            {
                await RestManagerHelper.FreeTable(_scopeFactory, clientsGroupTable, clientsGroup);
                await SitSomeoneOnFreeSpace(clientsGroupTable);
            }
            else
            {
                _clientsGroupQueue.Remove(clientsGroup);
                await RestManagerHelper.UpdateClientsGroupStatus(_scopeFactory, clientsGroup, ClientsGroupStatus.Boared);
            }
        }

        public async Task OnLeaveConcurrent(Guid clientsGroupId)
        {
            var clientsGroup = await RestManagerHelper.GetClientsGroup(_scopeFactory, clientsGroupId);

            await OnLeaveConcurrent(clientsGroup, semOnLeave);
        }
        private async Task OnLeaveConcurrent(ClientsGroup clientsGroup, Semaphore semaphore)
        {
            semaphore.WaitOne();

            await OnLeave(clientsGroup);

            semaphore.Release();
        }

        private async Task SitSomeoneOnFreeSpace(Table table)
        {
            while (_clientsGroupQueue.Any(cg => cg.Size <= table.FreeSpace))
            {
                var clientsGroup = _clientsGroupQueue.First(x => x.Size <= table.FreeSpace);
                var clientsGroupIndexInQueue = _clientsGroupQueue.IndexOf(clientsGroup);

                await _clientsGroupQueue.IncreaseBoredIndexInQueue(_scopeFactory, clientsGroupIndexInQueue, OnLeaveConcurrent);

                _clientsGroupQueue.Remove(clientsGroup);
                await SitClientsGroup(table, clientsGroup);
            }
        }

        private async Task SitClientsGroup(Table table, ClientsGroup clientsGroup)
        {
            var notCreatedClientsGroup = clientsGroup.Status is null;
            if (notCreatedClientsGroup)
                await SitClientsGroupWithCreate(table, clientsGroup);
            else
                await SitClientsGroupWithUpdate(table, clientsGroup);
        }

        private async Task SitClientsGroupWithCreate(Table table, ClientsGroup clientsGroup)
        {
            clientsGroup.TableId = table.Id;
            await RestManagerHelper.AddClientsGroup(_scopeFactory, clientsGroup, ClientsGroupStatus.AtTable);

            table.FreeSpace -= clientsGroup.Size;
            table.ClientsGroups.Add(clientsGroup);

            await RestManagerHelper.UpdateTable(_scopeFactory, table);

            BackgroundJob.Schedule(() => OnLeaveConcurrent(clientsGroup.Id), TimeSpan.FromSeconds(clientsGroup.LunchTimeInSeconds));
        }

        private async Task SitClientsGroupWithUpdate(Table table, ClientsGroup clientsGroup)
        {
            await RestManagerHelper.UpdateClientsGroupStatus(_scopeFactory, clientsGroup, ClientsGroupStatus.AtTable);

            table.FreeSpace -= clientsGroup.Size;
            table.ClientsGroups.Add(clientsGroup);

            await RestManagerHelper.UpdateTable(_scopeFactory, table);

            BackgroundJob.Schedule(() => OnLeaveConcurrent(clientsGroup.Id), TimeSpan.FromSeconds(clientsGroup.LunchTimeInSeconds));
        }
    }    
}