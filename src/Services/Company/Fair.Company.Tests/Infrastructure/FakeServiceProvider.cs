using Microsoft.Extensions.DependencyInjection;

namespace Fair.Company.Tests.Infrastruture;

public static class FakeServiceProvider
{
    public static IServiceProvider Get()
    {
        var services = new ServiceCollection();
        
        services.AddAutoMapper(typeof(Fair.Company.Api.CompanyController));

        return services.BuildServiceProvider();
    }
}