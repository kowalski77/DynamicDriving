using System.Net;
using System.Net.Mime;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.SharedKernel.Apis;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class ApplicationController : ControllerBase
{
    protected static IActionResult Error(ErrorResult? error, string invalidField)
    {
        return new EnvelopeResult(HttpStatusCode.BadRequest, ErrorEnvelope.Error(error, invalidField));
    }

    protected static IActionResult FromResult<T>(Result<T> result)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(result);

        IActionResult actionResult = (result.Success, result.Error?.Code) switch
        {
            (true, _) => new EnvelopeResult(HttpStatusCode.Created, SuccessEnvelope.Create(result.Value)),
            (false, ErrorConstants.RecordNotFound) => NotFound(result.Error, string.Empty),
            _ => Error(result.Error, string.Empty)
        };

        return actionResult;
    }

    protected static IActionResult Ok<T>(T model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return new EnvelopeResult(HttpStatusCode.OK, SuccessEnvelope.Create(model));
    }

    protected IActionResult CreatedResult<T, TR>(Result<T> result, Func<T, TR> converter, string routeName, Func<object> routeValues)
        where T : class
        where TR : class
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(converter);
        ArgumentNullException.ThrowIfNull(routeValues);

        IActionResult actionResult = (result.Success, result.Error?.Code) switch
        {
            (true, _) => this.CreatedAtRoute(routeName, routeValues.Invoke(), SuccessEnvelope.Create(converter(result.Value))),
            (false, ErrorConstants.RecordNotFound) => NotFound(result.Error, string.Empty),
            _ => Error(result.Error, string.Empty)
        };

        return actionResult;
    }

    protected static IActionResult FromResult<T, TR>(Result<T> result, Func<T, TR> converter)
        where TR : class
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(converter);

        IActionResult actionResult = (result.Success, result.Error?.Code) switch
        {
            (true, _) => new EnvelopeResult(HttpStatusCode.OK, SuccessEnvelope.Create(converter(result.Value))),
            (false, ErrorConstants.RecordNotFound) => NotFound(result.Error, string.Empty),
            _ => Error(result.Error, string.Empty)
        };

        return actionResult;
    }

    protected static IActionResult FromResult(Result result)
    {
        ArgumentNullException.ThrowIfNull(result);

        IActionResult actionResult = (result.Success, result.Error?.Code) switch
        {
            (true, _) => new EnvelopeResult(HttpStatusCode.OK, SuccessEnvelope.Ok()),
            (false, ErrorConstants.RecordNotFound) => NotFound(result.Error, string.Empty),
            _ => Error(result.Error, string.Empty)
        };

        return actionResult;
    }

    private static IActionResult NotFound(ErrorResult error, string invalidField)
    {
        return new EnvelopeResult(HttpStatusCode.NotFound, ErrorEnvelope.Error(error, invalidField));
    }
}
