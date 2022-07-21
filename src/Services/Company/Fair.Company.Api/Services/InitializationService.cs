using Fair.Company.Data;

namespace Fair.Company.Api.Services;

public class InitializationService
{
    private readonly IPersistenceService persistenceService;
    public InitializationService(IPersistenceService persistenceService)
    {
        ArgumentNullException.ThrowIfNull(persistenceService);
        this.persistenceService = persistenceService;
    }

    public async Task CreateHostCompanyIfNotExistsAsync()
    {
        bool exists = await persistenceService.Company.ExistsAsync(p => p.CompanyId == Constants.HostCompanyId);
        if (!exists)
        {
            var hostCompany = new Company.Data.Models.Company()
            {
                CompanyId = Constants.HostCompanyId,
                Active = true,
                Name = "Default Company"
            };

            persistenceService.Company.Add(hostCompany);
            await persistenceService.SaveChangesAsync();
        }
    }

}