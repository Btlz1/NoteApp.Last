using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using btlz.Exceptions;

namespace btlz.Services;

public class ExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
		    
        if (exception is UserNotFoundException userNotFoundException)
        {
		        
            httpContext.Response.ContentType = MediaTypeNames.Text.Plain;
            
            httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            
            await httpContext.Response.WriteAsync(userNotFoundException.Message);
            
            return true;
        }
       
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        
        await httpContext.Response.WriteAsync(string.Empty);
        
        return false;
    }
}