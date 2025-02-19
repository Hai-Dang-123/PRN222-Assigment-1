using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using GroupBoizBLL.Services.Implement;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;

namespace GroupBoizCommon.Middleware
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;

        public JWTMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    bool isValid = JWTProvide.Validation(token);
                    if (!isValid)
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Invalid or expired token.");
                        return;
                    }
                }
                catch (SecurityTokenExpiredException)
                {
                    context.Response.StatusCode = 401;
                    Console.WriteLine("hết hạn rồi");
                    await context.Response.WriteAsync("Token expired.");
                    context.Response.Redirect("/Home/Error");
                    return;
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync($"Error: {ex.Message}");
                    context.Response.Redirect("/Home/Error");
                    return;
                }
            }

            await _next(context);
        }
    }
}
