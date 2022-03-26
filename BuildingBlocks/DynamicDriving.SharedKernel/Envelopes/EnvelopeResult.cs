using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.SharedKernel.Envelopes
{
    public sealed class EnvelopeResult : IActionResult
    {
        public EnvelopeResult(Envelope envelope, HttpStatusCode statusCode)
        {
            this.StatusCode = (int)statusCode;
            this.Envelope = envelope;
        }

        public Envelope Envelope { get; }

        public int StatusCode { get; }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(this.Envelope) { StatusCode = this.StatusCode };

            return objectResult.ExecuteResultAsync(context);
        }
    }
}
