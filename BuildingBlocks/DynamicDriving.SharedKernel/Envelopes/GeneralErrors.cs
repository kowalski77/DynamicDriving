namespace DynamicDriving.SharedKernel.Envelopes;

public static class GeneralErrors
{
    public static ErrorResult NotFound(Guid id, string argument)
    {
        var forId = id == Guid.Empty ? "" : $" for Id '{id}'";
        return new ErrorResult(ErrorConstants.RecordNotFound, $"Record {argument} not found {forId}");
    }

    public static ErrorResult ValueIsRequired()
    {
        return new ErrorResult(ErrorConstants.ValueIsRequired, "Value is required");
    }

    public static ErrorResult NotValidEmailAddress()
    {
        return new ErrorResult(ErrorConstants.NotValidEmail, "Value is not a valid email.");
    }

    public static ErrorResult InternalServerError(string message)
    {
        return new ErrorResult(ErrorConstants.InternalServerError, message);
    }

    public static ErrorResult IdNotValid(string message)
    {
        return new ErrorResult(ErrorConstants.IdIsNullOrEmpty, message);
    }
}
