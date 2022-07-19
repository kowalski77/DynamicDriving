using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.SharedKernel.Envelopes;

public sealed class ModelStateValidator
{
    public static IActionResult ValidateModelState(ActionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        (var fieldName, var entry) = context.ModelState.First(x => x.Value?.Errors.Count > 0);
        var errorSerialized = entry?.Errors.First().ErrorMessage;

        var error = ErrorResult.Deserialize(errorSerialized);
        var envelope = ErrorEnvelope.Error(error, fieldName);
        var envelopeResult = new EnvelopeResult(HttpStatusCode.BadRequest, envelope);

        return envelopeResult;
    }
}
