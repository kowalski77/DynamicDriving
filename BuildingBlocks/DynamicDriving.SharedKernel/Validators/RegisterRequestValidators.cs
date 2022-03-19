using DynamicDriving.SharedKernel.Results;
using FluentValidation;

namespace DynamicDriving.SharedKernel.Validators;

public static class RegisterRequestValidators
{
    public static IRuleBuilderOptions<T, string> MustBeValueObject<T, TValueObject>(
        this IRuleBuilder<T, string> ruleBuilder,
        Func<string, Result<TValueObject>> factoryMethod)
    {
        return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) =>
        {
            var result = factoryMethod(value);
            if (result.Failure)
            {
                context.AddFailure($"'{result.Value}' {result.Error}");
            }
        });
    }
}
