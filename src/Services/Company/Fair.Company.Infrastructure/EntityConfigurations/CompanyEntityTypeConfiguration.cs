using Microsoft.EntityFrameworkCore;

namespace Fair.Company.Data.EntityConfigurations;

public class CompanyEntityTypeConfiguration : IEntityTypeConfiguration<Company.Data.Models.Company>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Company.Data.Models.Company> builder)
    {
        builder.ToTable("Company");
        builder.HasKey(p => p.CompanyId);
    }
}