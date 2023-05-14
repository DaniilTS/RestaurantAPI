using MediatR;

namespace Application.Mediator.RestaurantManager
{
    public class AddClientsGroupRequest: IRequest
    {
        public int Size { get; set; }

        public int MaxBoaredIndex { get; set; }

        public int LunchTimeInSeconds { get; set; }
    }
}
