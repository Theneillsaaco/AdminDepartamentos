namespace AdminDepartamentos.API.Middleware;

public class RegisterAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public RegisterAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/register", StringComparison.OrdinalIgnoreCase))
        {
            if (!context.User.Identity?.IsAuthenticated ?? false)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
        }

        await _next(context);
    }
}