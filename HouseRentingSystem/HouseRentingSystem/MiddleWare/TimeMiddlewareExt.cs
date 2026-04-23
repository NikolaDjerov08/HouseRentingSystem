namespace HouseRentingSystem.MiddleWare
{
    public static class TimeMiddlewareExt
    {
        public static IApplicationBuilder UseTimer
            (this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TimeMiddleware>();
        }
    }
}
