using Fair.Company.Data;
using Fair.Company.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class PersistenseServiceExtensions
{
    public static void AddPersistenceService(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<CompanyDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
                b => b.MigrationsAssembly("Fair.Company.Infrastructure.SqlServer"));                        
        });            
        
        services.AddTransient<IPersistenceService, PersistenceService>();
    }

    public static void UseDataBaseUpdate(this WebApplication app)
    {
        using (var serviceScope = app.Services.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<CompanyDbContext>();
            context.Database.Migrate();
        }
    }    
}