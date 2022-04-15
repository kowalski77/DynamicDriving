using System.Net;
using System.Net.Mime;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.TripManagement.API.Support;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class ApplicationController : ControllerBase
{
    public ApplicationController(IMediator mediator)
    {
        this.Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected IMediator Mediator { get; }

    protected static IActionResult Error(ErrorResult? error, string invalidField)
    {
        return new EnvelopeResult(ErrorEnvelope.Error(error, invalidField), HttpStatusCode.BadRequest);
    }

    protected static IActionResult FromResultModel<T>(Result<T> result)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(result);

        IActionResult actionResult = (result.Success, result.Error?.Code) switch
        {
            (true, _) => new EnvelopeResult(SuccessEnvelope.Create(result.Value), HttpStatusCode.Created),
            (false, ErrorConstants.RecordNotFound) => NotFound(result.Error, string.Empty),
            _ => Error(result.Error, string.Empty)
        };

        return actionResult;
    }

    protected IActionResult CreatedResultModel<T, TR>(Result<T> result, Func<T, TR> converter, string routeName, Func<object> routeValues)
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

    protected static IActionResult FromResultModel<T, TR>(Result<T> result, Func<T, TR> converter)
        where T : class
        where TR : class
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(converter);

        IActionResult actionResult = (result.Success, result.Error?.Code) switch
        {
            (true, _) => new EnvelopeResult(SuccessEnvelope.Create(converter(result.Value)), HttpStatusCode.OK),
            (false, ErrorConstants.RecordNotFound) => NotFound(result.Error, string.Empty),
            _ => Error(result.Error, string.Empty)
        };

        return actionResult;
    }

    protected IActionResult FromResultModel(Result result)
    {
        ArgumentNullException.ThrowIfNull(result);

        IActionResult actionResult = (result.Success, result.Error?.Code) switch
        {
            (true, _) => new EnvelopeResult(SuccessEnvelope.Ok(), HttpStatusCode.OK),
            (false, ErrorConstants.RecordNotFound) => NotFound(result.Error, string.Empty),
            _ => Error(result.Error, string.Empty)
        };

        return actionResult;
    }

    private static IActionResult NotFound(ErrorResult error, string invalidField)
    {
        return new EnvelopeResult(ErrorEnvelope.Error(error, invalidField), HttpStatusCode.NotFound);
    }
}
