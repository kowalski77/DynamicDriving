using DynamicDriving.Contracts.Models;
using FluentValidation;

namespace DynamicDriving.DriverManagement.API.UseCases.Drivers.Register;

public class RegisterDriverValidator : AbstractValidator<RegisterDriverRequest>
{
    public RegisterDriverValidator()
    {
        this.RuleFor(x=>x.Id).NotEmpty();
        this.RuleFor(x => x.Name).NotEmpty();
        this.RuleFor(x => x.Car).SetValidator(new CarValidator());
    }
}

public class CarValidator : AbstractValidator<Car>
{
    public CarValidator()
    {
        this.RuleFor(x=>x.Id).NotEmpty();
        this.RuleFor(x => x.Model).NotEmpty();
    }
}
