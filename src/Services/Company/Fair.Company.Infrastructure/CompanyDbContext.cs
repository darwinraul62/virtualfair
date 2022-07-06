using Fair.Company.Data;
using Fair.Company.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Fair.Company.Infrastructure;

public class CompanyDbContext : DbContext
{
    public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
    {
        ArgumentNullException.ThrowIfNull(options, nameof(options));
    }

    public DbSet<Company.Data.Models.Company>? Companies { get; set; }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));
        modelBuilder.ApplyConfiguration<Company.Data.Models.Company>(new CompanyEntityTypeConfiguration());
    }
 
}