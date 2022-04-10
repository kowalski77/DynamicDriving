namespace DynamicDriving.SharedKernel.Envelopes;

public class ErrorEnvelope
{
    private ErrorEnvelope(ErrorResult? error, string invalidField)
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

    public static ErrorEnvelope Error(ErrorResult? error, string invalidField)
    {
        return new ErrorEnvelope(error, invalidField);
    }
}
