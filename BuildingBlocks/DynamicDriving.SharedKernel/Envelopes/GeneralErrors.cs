namespace DynamicDriving.SharedKernel.Envelopes
{
    public static class GeneralErrors
    {
        public static ErrorResult NotFound(Guid id)
        {
            var forId = id == Guid.Empty ? "" : $" for Id '{id}'";
            return new ErrorResult(ErrorConstants.RecordNotFound, $"Record not found{forId}");
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
            return new ErrorResult("internal.server.error", message);
        }
    }
}