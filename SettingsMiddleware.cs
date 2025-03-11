   public class SettingsMiddleware
   {
       private readonly RequestDelegate _next;

       public SettingsMiddleware(RequestDelegate next)
       {
           _next = next;
       }

       public async Task InvokeAsync(HttpContext context)
       {
           // Check for settings change request
           if (context.Request.Path.StartsWithSegments("/change-setting"))
           {
               var name = context.Request.Query["name"];
               var value = context.Request.Query["value"];

               if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
               {
                   context.Response.Cookies.Append(name, value, new CookieOptions
                   {
                       Expires = DateTime.UtcNow.AddYears(1)
                   });
               }
           }

           await _next(context);
       }
   }
   