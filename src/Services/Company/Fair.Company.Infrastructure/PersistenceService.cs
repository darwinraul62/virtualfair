using Fair.Company.Data;
using Fair.Company.Data.Repositories;
using Fair.Company.Infrastructure.Repositories;

namespace Fair.Company.Infrastructure;

public class PersistenceService : IPersistenceService
{
    private readonly CompanyDbContext context;

    public PersistenceService(CompanyDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.context = context;
    }

    public ICompanyRepository Company => new CompanyRepository(context);

    public void Dispose()
    {
        if(context!=null)        
            context.Dispose();        
    }

    public Task ResilientTransactionAsync(Func<Task> action)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync();
    }
}