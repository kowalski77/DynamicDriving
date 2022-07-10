using DynamicDriving.Contracts.Commands;
using DynamicDriving.Contracts.Events;
using DynamicDriving.Identity.Service.Entities;
using DynamicDriving.Identity.Service.Exceptions;
using DynamicDriving.SharedKernel;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace DynamicDriving.Identity.Service.Consumers;

public class AddCreditsConsumer : IConsumer<AddCredits>
{
    private readonly UserManager<ApplicationUser> userManager;

    public AddCreditsConsumer(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task Consume(ConsumeContext<AddCredits> context)
    {
        Guards.ThrowIfNull(context);

        var user = await this.userManager.FindByIdAsync(context.Message.UserId.ToString()).ConfigureAwait(false);
        if (user is null)
        {
            throw new AddCreditsException($"User with Id: {context.Message.UserId} does not exists");
        }

        user.Credits += context.Message.Credits;

        var result = await this.userManager.UpdateAsync(user).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            throw new AddCreditsException(string.Join(",", result.Errors));
        }

        await context.Publish(new CreditsAdded(context.Message.CorrelationId)).ConfigureAwait(false);
    }
}
