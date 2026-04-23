using HouseRentingSystem.Data.Data;
using System.Diagnostics;

namespace HouseRentingSystem.MiddleWare
{
    public class CustomMiddleware
    {
        private RequestDelegate next;
        public CustomMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext, HouseRentingDbContext ctx, IConfiguration config) //IConfiguration
        {
            var housesCount = await ctx.Houses.CountAsync();
            Console.WriteLine();
            await this.next(httpContext);
        }
    }
}
