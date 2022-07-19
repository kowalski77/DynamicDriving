using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.SharedKernel.Envelopes
{
    public sealed class EnvelopeResult : IActionResult
    {
        public EnvelopeResult(HttpStatusCode statusCode, params ErrorEnvelope[] errorEnvelopes)
        {
            this.StatusCode = (int)statusCode;
            this.ErrorEnvelopeCollection = errorEnvelopes;
        }

        public EnvelopeResult(HttpStatusCode statusCode, SuccessEnvelope successEnvelope)
        {
            this.StatusCode = (int)statusCode;
            this.SuccessEnvelope = successEnvelope;
        }

        public IEnumerable<ErrorEnvelope>? ErrorEnvelopeCollection { get; }

        public SuccessEnvelope? SuccessEnvelope { get; }

        public int StatusCode { get; }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = this.ErrorEnvelopeCollection is null ?
                new ObjectResult(this.SuccessEnvelope) { StatusCode = this.StatusCode } :
                new ObjectResult(this.ErrorEnvelopeCollection) { StatusCode = this.StatusCode };

            return objectResult.ExecuteResultAsync(context);
        }
    }
}
