using Application.Services;
using MediatR;

namespace Application.Mediator.RestaurantManager.AddClientGroup
{
    public class Handler : IRequestHandler<AddClientsGroupRequest>
    {
        private readonly RestManager _restManager;

        public Handler(RestManager restManager)
        {
            _restManager = restManager;
        }

        public async Task Handle(AddClientsGroupRequest request, CancellationToken cancellationToken)
        {
            await _restManager.OnArrive(new Domain.Entities.ClientsGroup()
            { 
                Size = request.Size,
                LunchTimeInSeconds = request.LunchTimeInSeconds,
                MaxBoaredIndex = request.MaxBoaredIndex,
                ArrivalTime = DateTime.UtcNow
            });
        }
    }
}
