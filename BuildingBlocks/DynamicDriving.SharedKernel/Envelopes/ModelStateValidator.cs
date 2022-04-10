using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.SharedKernel.Envelopes
{
    public sealed class ModelStateValidator
    {
        public static IActionResult ValidateModelState(ActionContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            //TODO: review nullable
            (string fieldName, var entry) = context.ModelState.First(x => x.Value.Errors.Count > 0);
            string errorSerialized = entry.Errors.First().ErrorMessage;

            var error = ErrorResult.Deserialize(errorSerialized);
            var envelope = ErrorEnvelope.Error(error, fieldName);
            var envelopeResult = new EnvelopeResult(envelope, HttpStatusCode.BadRequest);

            return envelopeResult;
        }
    }
}
