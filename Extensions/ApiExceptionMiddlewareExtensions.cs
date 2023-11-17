using System.Net;
using backend.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace backend.Extensions;

public static class ApiExceptionMiddlewareExtensions 
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError => {

            appError.Run(async context => {

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature?.Error is AppException) 
                {
                    var exception = contextFeature.Error as AppException;
                    context.Response.StatusCode = exception.StatusCode;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(exception.ToString());
                }


            });

        });
    }
}