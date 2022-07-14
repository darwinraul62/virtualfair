using System.Linq.Expressions;
using System.Net;
using Fair.Company.Api;
using Fair.Company.Api.Models;
using Fair.Company.Data;
using Fair.Company.Tests.Infrastruture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;

namespace Fair.Company.Test;

public class CompanyControllerTest
{
    private readonly CompanyController companyController;
    private readonly Mock<IPersistenceService> persistenceService;

    public CompanyControllerTest()
    {
        persistenceService = new Mock<IPersistenceService>();     
        this.companyController = new CompanyController(persistenceService.Object);     
    }

    [Fact]
    public async Task Get_All_ReturnsOkResult()
    {
        // Arrange
        persistenceService.Setup(x => x.Company.GetAsync(null, null, null))
            .ReturnsAsync(FakeCompanies.GetFakeCompanies());

        // Act
        var result = await this.companyController.Get();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_NotExists_ReturnsNotFound()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        persistenceService.Setup(x => x.Company.GetFirstOrDefaultAsync(p => p.CompanyId == id, null))
            .ReturnsAsync(FakeCompanies.GetFakeCompanies().FirstOrDefault(p => p.CompanyId == id));

        // Act
        var result = await this.companyController.GetById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_Exists_ReturnsCompany()
    {
        //Arrange
        Guid id = FakeCompanies.DefaultCompanyId;
        persistenceService.Setup(x => x.Company.GetFirstOrDefaultAsync(p => p.CompanyId == id, null))
            .ReturnsAsync(FakeCompanies.GetFakeCompanies().FirstOrDefault(p => p.CompanyId == id));

        // Act
        var result = await this.companyController.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        var company = Assert.IsType<Company.Data.Models.Company>(okResult.Value);

        Assert.Equal(FakeCompanies.DefaultCompanyId, company.CompanyId);
    }

    [Fact]
    public async Task Post_Valid_ReturnsCreated()
    {
        // Arrange
        this.persistenceService.Setup(x => x.Company.ExistsAsync(p => p.Identification == FakeCompanies.DefaultCompanyIdentification))
            .ReturnsAsync(false);
        
        persistenceService.Setup(x => x.SaveChangesAsync(CancellationToken.None))
            .ReturnsAsync(1);

        CompanyInsertRequestDTO model = new CompanyInsertRequestDTO()
        {
            Identification = FakeCompanies.DefaultCompanyIdentification,
            Name = "Test Company"
        };

        // Act
        var result = await this.companyController.Post(model);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);

        var company = Assert.IsType<CompanyInsertResponseDTO>(createdResult.Value);

        Assert.NotNull(company);
        Assert.NotEqual(Guid.Empty, company.CompanyId);
    }

    [Fact]
    public async Task Post_EmptyModel_ReturnsBadRequest()
    {
        // Arrange
        CompanyInsertRequestDTO model = new CompanyInsertRequestDTO();
        this.companyController.ModelState.AddModelError("Identification", "Identification is required");
        this.companyController.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await this.companyController.Post(model);

        // Assert
        this.persistenceService.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        var objectResult = (IStatusCodeActionResult)result;
        Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
    }

    [Fact]
    public async Task Post_IdentificationAlreadyExists_ReturnsConflict()
    {
        // Arrange
        CompanyInsertRequestDTO model = new CompanyInsertRequestDTO()
        {
            Identification = FakeCompanies.CompanyIdentificationDuplicated,
            Name = "Test Company ABC"
        };

        this.persistenceService.Setup(x => x.Company.ExistsAsync(p => p.Identification == model.Identification))
            .ReturnsAsync(true);
     
        // Act
        var result = await this.companyController.Post(model);

        // Assert
        this.persistenceService.Verify(x => x.Company.ExistsAsync(It.IsAny<Expression<Func<Company.Data.Models.Company, bool>>>()), Times.Once);
        this.persistenceService.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        var objectResult = (IStatusCodeActionResult)result;
        Assert.Equal((int)HttpStatusCode.Conflict, objectResult.StatusCode);
    }
}