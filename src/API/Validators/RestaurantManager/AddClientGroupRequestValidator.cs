using Application.Mediator.RestaurantManager;
using FluentValidation;

namespace API.Validators.RestaurantManager
{
    public class AddClientGroupRequestValidator : AbstractValidator<AddClientsGroupRequest>
    {
        public AddClientGroupRequestValidator()
        {
            RuleFor(x => x.Size).GreaterThanOrEqualTo(1);
            RuleFor(x => x.MaxBoaredIndex).GreaterThanOrEqualTo(1);
            RuleFor(x => x.LunchTimeInSeconds).GreaterThanOrEqualTo(1);
        }
    }
}
