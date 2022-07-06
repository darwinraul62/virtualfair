using Ecubytes.Repository.EntityFramework;
using Fair.Company.Data.Repositories;

namespace Fair.Company.Infrastructure.Repositories;

public class CompanyRepository : Repository<Company.Data.Models.Company>, ICompanyRepository
{
    public CompanyRepository(CompanyDbContext context) : base(context)
    {
    }
}