using DynamicDriving.SharedKernel.Results;
using FluentValidation;

namespace DynamicDriving.SharedKernel.Validators;

public static class RegisterRequestValidators
{
    public static IRuleBuilderOptions<T, decimal> MustBeValueObject<T, TValueObject>(
        this IRuleBuilder<T, decimal> ruleBuilder,
        Func<decimal, Result<TValueObject>> factoryMethod)
    {
        return (IRuleBuilderOptions<T, decimal>)ruleBuilder.Custom((value, context) =>
        {
            var result = factoryMethod(value);
            if (result.Failure)
            {
                context.AddFailure(result.Error.Serialize()); // TODO: check nullable
            }
        });
    }
}
