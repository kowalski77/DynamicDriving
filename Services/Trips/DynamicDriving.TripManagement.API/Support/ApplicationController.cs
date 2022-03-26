using System.Net;
using System.Net.Mime;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.ResultModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.TripManagement.API.Support;

[ApiController]
[Authorize]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class ApplicationController : ControllerBase
{
    public ApplicationController(IMediator mediator)
    {
        this.Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected IMediator Mediator { get; }

    protected static IActionResult EnvelopeOk(object result)
    {
        return new EnvelopeResult(Envelope.Ok(result), HttpStatusCode.OK);
    }

    protected static IActionResult EnvelopeOk()
    {
        return new EnvelopeResult(Envelope.Ok(), HttpStatusCode.OK);
    }

    protected static IActionResult Error(ErrorResult? error, string invalidField)
    {
        return new EnvelopeResult(Envelope.Error(error, invalidField), HttpStatusCode.BadRequest);
    }

    protected static IActionResult FromResultModel<T>(IResultModel<T> result)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(result);

        IActionResult actionResult = (result.Success, result.ErrorResult?.Code) switch
        {
            (true, _) => EnvelopeOk(result.Value),
            (false, ErrorConstants.RecordNotFound) => NotFound(result.ErrorResult, string.Empty),
            _ => Error(result.ErrorResult, string.Empty)
        };

        return actionResult;
    }

    protected IActionResult CreatedResultModel<T>(IResultModel<T> result, string routeName, object routeValues)
        where T : class // TDO: review this null and errorResult class
    {
        ArgumentNullException.ThrowIfNull(result);

        IActionResult actionResult = (result.Success, result.ErrorResult?.Code) switch
        {
            (true, _) => this.CreatedAtRoute(routeName, routeValues, EnvelopeOk(result.Value)),
            (false, ErrorConstants.RecordNotFound) => NotFound(result.ErrorResult, string.Empty),
            _ => Error(result.ErrorResult, string.Empty)
        };

        return actionResult;
    }

    protected static IActionResult FromResultModel<T, TR>(IResultModel<T> result, Func<T, TR> converter)
        where TR : class
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(converter);

        IActionResult actionResult = (result.Success, result.ErrorResult?.Code) switch
        {
            (true, _) => EnvelopeOk(converter(result.Value)),
            (false, ErrorConstants.RecordNotFound) => NotFound(result.ErrorResult, string.Empty),
            _ => Error(result.ErrorResult, string.Empty)
        };

        return actionResult;
    }

    protected IActionResult FromResultModel(IResultModel result)
    {
        ArgumentNullException.ThrowIfNull(result);

        IActionResult actionResult = (result.Success, result.ErrorResult?.Code) switch
        {
            (true, _) => EnvelopeOk(),
            (false, ErrorConstants.RecordNotFound) => NotFound(result.ErrorResult, string.Empty),
            _ => Error(result.ErrorResult, string.Empty)
        };

        return actionResult;
    }

    private static IActionResult NotFound(ErrorResult error, string invalidField)
    {
        return new EnvelopeResult(Envelope.Error(error, invalidField), HttpStatusCode.NotFound);
    }
}
