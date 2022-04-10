using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.SharedKernel.Envelopes
{
    public sealed class EnvelopeResult : IActionResult
    {
        public EnvelopeResult(ErrorEnvelope errorEnvelope, HttpStatusCode statusCode)
        {
            this.ErrorEnvelope = errorEnvelope;
            this.StatusCode = (int)statusCode;
        }

        public EnvelopeResult(SuccessEnvelope successEnvelope, HttpStatusCode statusCode)
        {
            this.SuccessEnvelope = successEnvelope;
            this.StatusCode = (int)statusCode;
        }

        public ErrorEnvelope? ErrorEnvelope { get; }

        public SuccessEnvelope? SuccessEnvelope { get; }

        public int StatusCode { get; }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = this.ErrorEnvelope is null ?
                new ObjectResult(this.SuccessEnvelope) { StatusCode = this.StatusCode } :
                new ObjectResult(this.ErrorEnvelope) { StatusCode = this.StatusCode };

            return objectResult.ExecuteResultAsync(context);
        }
    }
}
