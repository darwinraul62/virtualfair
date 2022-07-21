using System.Linq.Expressions;
using Fair.Company.Api.Services;
using Fair.Company.Data;
using Moq;

namespace Fair.Company.Tests;

public class InitializationServiceTest
{
    private readonly InitializationService initializationService;
    private readonly Mock<IPersistenceService> persistenceService;

    public InitializationServiceTest()
    {
        persistenceService = new Mock<IPersistenceService>();        
        
        this.initializationService = new InitializationService(persistenceService.Object);
    }

    [Fact]
    public async Task CreateHostCompanyIfNotExistsAsync_CompanyNotExists_CompanyCreated()
    {
        persistenceService.Setup(x => x.Company.ExistsAsync(It.IsAny<Expression<Func<Company.Data.Models.Company, bool>>>()))
            .ReturnsAsync(false);

        persistenceService.Setup(x => x.Company.Add(It.IsAny<Company.Data.Models.Company>()));
        persistenceService.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        
        await initializationService.CreateHostCompanyIfNotExistsAsync();
        
        persistenceService.Verify(x => x.Company.ExistsAsync(It.IsAny<Expression<Func<Company.Data.Models.Company, bool>>>()), Times.Once);
        persistenceService.Verify(x => x.Company.Add(It.IsAny<Company.Data.Models.Company>()), Times.Once);
        persistenceService.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    } 

    [Fact]
    public async Task CreateHostCompanyIfNotExistsAsync_CompanyExists_CompanyNotCreated()
    {
        persistenceService.Setup(x => x.Company.ExistsAsync(It.IsAny<Expression<Func<Company.Data.Models.Company, bool>>>()))
            .ReturnsAsync(true);
        
        await initializationService.CreateHostCompanyIfNotExistsAsync();
        
        persistenceService.Verify(x => x.Company.ExistsAsync(It.IsAny<Expression<Func<Company.Data.Models.Company, bool>>>()), Times.Once);
        persistenceService.Verify(x => x.Company.Add(It.IsAny<Company.Data.Models.Company>()), Times.Never);
        persistenceService.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}