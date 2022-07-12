using Microsoft.Extensions.DiagnosticAdapter;

namespace DynamicDriving.Identity.Service.Support;

// NOTE: In this example, we are doing some trivial logging but the possibility is there to do far more interesting things as needs be: inspecting headers; reading query parameters; writing to other data sinks etc.
public class AnalysisDiagnosticAdapter
{
    private readonly ILogger<AnalysisDiagnosticAdapter> logger;
    
    public AnalysisDiagnosticAdapter(ILogger<AnalysisDiagnosticAdapter> logger)
    {
        this.logger = logger;
    }

    [DiagnosticName("Microsoft.AspNetCore.MiddlewareAnalysis.MiddlewareStarting")]
    public void OnMiddlewareStarting(HttpContext httpContext, string name, Guid instance, long timestamp)
    {
        this.logger.LogInformation($"MiddlewareStarting: '{name}'; Request Path: '{httpContext.Request.Path}'");
    }

    [DiagnosticName("Microsoft.AspNetCore.MiddlewareAnalysis.MiddlewareException")]
    public void OnMiddlewareException(Exception exception, HttpContext httpContext, string name, Guid instance, long timestamp, long duration)
    {
        this.logger.LogInformation($"MiddlewareException: '{name}'; '{exception.Message}'");
    }

    [DiagnosticName("Microsoft.AspNetCore.MiddlewareAnalysis.MiddlewareFinished")]
    public void OnMiddlewareFinished(HttpContext httpContext, string name, Guid instance, long timestamp, long duration)
    {
        this.logger.LogInformation($"MiddlewareFinished: '{name}'; Status: '{httpContext.Response.StatusCode}'");
    }
}
