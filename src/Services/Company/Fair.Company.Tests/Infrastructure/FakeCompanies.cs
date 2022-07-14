namespace Fair.Company.Tests.Infrastruture;

public sealed class FakeCompanies
{    
    public static Guid DefaultCompanyId => Guid.Parse("78fbb862-4c0b-4e9e-a2cd-dc08eb0c4131");

    public const string DefaultCompanyIdentification = "0011234567890";
    public const string CompanyIdentificationDuplicated = "0000000000001";


    public static List<Company.Data.Models.Company> GetFakeCompanies() => 
        new List<Company.Data.Models.Company>
        {
            new Company.Data.Models.Company
            {
                CompanyId = DefaultCompanyId,
                Identification = "0000000000001",
                Name = "Company 1",                
                Active = true            
            },
            new Company.Data.Models.Company
            {
                CompanyId = Guid.NewGuid(),
                Identification = "0000000000002",
                Name = "Company 2",                
                Active = true
            },
            new Company.Data.Models.Company
            {
                CompanyId = Guid.NewGuid(),
                Identification = "0000000000003",
                Name = "Company 3",     
                Active = true
            },
            new Company.Data.Models.Company
            {
                CompanyId = Guid.NewGuid(),
                Identification = "0000000000004",
                Name = "Company 4", 
                Active = true
            },
            new Company.Data.Models.Company
            {
                CompanyId = Guid.NewGuid(),
                Identification = "0000000000005",
                Name = "Company 5", 
                Active = true
            }
        };
}