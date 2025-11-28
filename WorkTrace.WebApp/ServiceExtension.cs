using WorkTrace.WebApp.Services.ApiServices;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp;

public static class ServiceExtension
{
    public static void AddWebAppServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<AuthTokenHandler>();

        var apiBaseUrl = services.BuildServiceProvider().GetRequiredService<IConfiguration>()["APIConfigurations:ApiUrl"];
        if (string.IsNullOrEmpty(apiBaseUrl))
        {
            throw new InvalidOperationException("API base URL is not configured in appsettings.json. Please add 'ApiUrl' to the 'APIConfigurations' section.");
        }

        services.AddHttpClient<IAuthApiService, AuthApiService>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        });

        services.AddHttpClient<IUserApiService, UserApiService>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        }).AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<IClientApiService, ClientApiService>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        }).AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<IStatusApiService, StatusApiService>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        }).AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<IServiceApiService, ServiceApiService>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        }).AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<IAssignmentApiService, AssignmentApiService>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        }).AddHttpMessageHandler<AuthTokenHandler>();
    }
}
