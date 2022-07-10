using DynamicDriving.Contracts.Commands;
using DynamicDriving.Contracts.Events;
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
        if(user is null)
        {
            throw new DeductCreditsException($"User with Id: {context.Message.UserId} does not exists");
        }

        user.Credits -= context.Message.Credits;
        
        var result = await this.userManager.UpdateAsync(user).ConfigureAwait(false);
        if (result.Succeeded)
        {
            throw new DeductCreditsException(string.Join(",", result.Errors));
        }

        await context.Publish(new CreditsDeducted(context.Message.CorrelationId)).ConfigureAwait(false);
    }
}
