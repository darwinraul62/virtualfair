using Fair.Company.Api.Services;
using Fair.Company.Data;
using Fair.Company.Infrastructure;

namespace Fair.Company.Api.HostedServices;

public class InitializeDataWorker : IHostedService
{    
    private readonly IServiceProvider serviceProvider;    
    public InitializeDataWorker(IServiceProvider serviceProvider)
    {        
        ArgumentNullException.ThrowIfNull(serviceProvider);
        this.serviceProvider = serviceProvider;      
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var initializationService = scope.ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<InitializationService>();
        
        await initializationService.CreateHostCompanyIfNotExistsAsync();
    }
    

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

}