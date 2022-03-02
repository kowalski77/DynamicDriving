using System.Diagnostics.CodeAnalysis;

namespace DynamicDriving.SharedKernel.Envelopes
{
    public class Envelope
    {
        public Envelope() { }

        private Envelope(ErrorResult? error, string invalidField)
        {
            this.ErrorCode = error?.Code;
            this.ErrorMessage = error?.Message;
            this.InvalidField = invalidField;
            this.TimeGenerated = DateTime.UtcNow;
        }

        public string? ErrorCode { get; set; }

        public string? ErrorMessage { get; set; }

        public string? InvalidField { get; set; }

        public DateTime TimeGenerated { get; set; }

        public static Envelope Ok<T>([NotNull]T result)
            where T : notnull
        {
            return new Envelope<T>(result);
        }

        public static Envelope Ok()
        {
            return new Envelope();
        }

        public static Envelope Error(ErrorResult? error, string invalidField)
        {
            return new Envelope(error, invalidField);
        }
    }
}