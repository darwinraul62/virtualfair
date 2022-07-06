using Fair.Company.Data;
using Fair.Company.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class PersistenseServiceExtensions
{
    public static void AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CompanyDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));            
        });        

        services.AddTransient<IPersistenceService, PersistenceService>();
    }
}