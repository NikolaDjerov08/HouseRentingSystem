namespace HouseRentingSystem.MiddleWare
{
    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustom(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddleware>();
        }
    }
}
