using DynamicDriving.Models;
using FluentValidation;

namespace DynamicDriving.TripManagement.API.UseCases.Trips.CreateDraft;

public class CreateDraftTripValidator : AbstractValidator<CreateDraftTripRequest>
{
    public CreateDraftTripValidator()
    {
        this.RuleFor(x => x.UserId).NotEmpty();
    }
}
