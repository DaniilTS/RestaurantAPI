using Application.DTO;
using MediatR;

namespace Application.Mediator.RestaurantManager
{
    public class GetClientsGroupsRequest: IRequest<List<ClientsGroupDTO>>
    {
    }
}
