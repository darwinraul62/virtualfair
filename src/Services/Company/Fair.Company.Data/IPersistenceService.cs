using Fair.Company.Data.Repositories;

namespace Fair.Company.Data;

public interface IPersistenceService : Ecubytes.Repository.Abstractions.IUnitOfWork
{
    public ICompanyRepository Company { get; }
}