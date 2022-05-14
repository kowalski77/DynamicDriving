using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace DynamicDriving.SharedKernel.Envelopes;

public sealed class ExceptionHandler
{
    private readonly IWebHostEnvironment environment;
    private readonly RequestDelegate next;

    public ExceptionHandler(RequestDelegate next, IWebHostEnvironment environment)
    {
        this.next = next;
        this.environment = environment;
    }

    public async Task Invoke(HttpContext context)
    {
        Guards.ThrowIfNull(context);

        try
        {
            await this.next(context).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            await this.HandleException(context, e).ConfigureAwait(false);
        }
    }

    private Task HandleException(HttpContext context, Exception exception)
    {
        var errorMessage = this.environment.IsProduction() ? "Internal server error" : "Exception: " + exception.Message;
        var error = GeneralErrors.InternalServerError(errorMessage);
        var envelope = ErrorEnvelope.Error(error, string.Empty);
        var result = JsonSerializer.Serialize(envelope);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsync(result);
    }
}
