using Fair.Company.Api.Services;
using Fair.Company.Data;

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
        var initializationService = serviceProvider.GetService<InitializationService>();

        if(initializationService == null)
            throw new InvalidOperationException("InitializationService service not found");        

        await initializationService.CreateHostCompanyIfNotExistsAsync();
    }
    

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

}