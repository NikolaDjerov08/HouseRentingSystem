using HouseRentingSystem.Data.Data;
using Microsoft.EntityFrameworkCore;
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
        public async Task InvokeAsync(HttpContext httpContext, HouseRentingDbContext ctx, IConfiguration config) 
        {
            var housesCount = await ctx.Houses.CountAsync();
            Console.WriteLine();
            await this.next(httpContext);
        }
    }
}
