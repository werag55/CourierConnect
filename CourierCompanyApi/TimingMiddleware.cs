using System.Diagnostics;

public class TimingMiddleware
{
    private readonly RequestDelegate _next;

    public TimingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        // Przed przetworzeniem żądania

        // Logika śledzenia czasu przed wywołaniem _next
        //var executionTime = stopwatch.ElapsedMilliseconds;
        //context.Response.Headers.Add("X-Execution-Time", executionTime.ToString());

        context.Response.OnStarting(state => 
        {
            var executionTime = stopwatch.ElapsedMilliseconds;
            var httpContext = (HttpContext)state;
            httpContext.Response.Headers.Add("X-Execution-Time", executionTime.ToString());

            return Task.CompletedTask;
        }, context);

        await _next(context);

        // Po przetworzeniu żądania
        stopwatch.Stop();
    }
}


public static class TimingMiddlewareExtensions
{
    public static IApplicationBuilder UseTimingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TimingMiddleware>();
    }
}