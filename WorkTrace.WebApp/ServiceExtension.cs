using WorkTrace.WebApp.Services.ApiServices;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp;

public static class ServiceExtension
{
    public static void AddWebAppServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IAuthApiService, AuthApiService>();
        services.AddScoped<IUserApiService, UserApiService>();
    }
}
