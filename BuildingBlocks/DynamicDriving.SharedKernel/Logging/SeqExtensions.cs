using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DynamicDriving.SharedKernel.Logging;

public static class SeqExtensions
{
    public static IServiceCollection AddSeqLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(loggingBuilder =>
        {
            var seqSettings = configuration.GetSection(nameof(SeqSettings)).Get<SeqSettings>();
            loggingBuilder.AddSeq(seqSettings.ServerUrl);
        });

        return services;
    }
}
