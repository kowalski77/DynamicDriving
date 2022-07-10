using DynamicDriving.Contracts.Identity;
using DynamicDriving.Identity.Service.Entities;
using DynamicDriving.Identity.Service.Exceptions;
using DynamicDriving.SharedKernel;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace DynamicDriving.Identity.Service.Consumers;

public class DeductCreditsConsumer : IConsumer<DeductCredits>
{
    private readonly UserManager<ApplicationUser> userManager;

    public DeductCreditsConsumer(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task Consume(ConsumeContext<DeductCredits> context)
    {
        Guards.ThrowIfNull(context);

        var user = await this.userManager.FindByIdAsync(context.Message.UserId.ToString()).ConfigureAwait(false);
        if (user is null)
        {
            throw new UnknownUserException(context.Message.UserId);
        }

        user.Credits -= context.Message.Credits;
        if (user.Credits < 0)
        {
            throw new NotEnoughCreditsException(context.Message.UserId, user.Credits);
        }

        _ = await this.userManager.UpdateAsync(user).ConfigureAwait(false);

        await context.Publish(new CreditsDeducted(context.Message.CorrelationId)).ConfigureAwait(false);
    }
}
