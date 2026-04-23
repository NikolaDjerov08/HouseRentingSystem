using HouseRentingSystem.Data.Data;
using System.Diagnostics;

namespace HouseRentingSystem.MiddleWare
{
    public class TimeMiddleware
    {
        private RequestDelegate next;
        public TimeMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext, HouseRentingDbContext ctx)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Start");
            await this.next(httpContext);
            stopwatch.Stop();
            Console.WriteLine($"Milliseconds finale time: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
