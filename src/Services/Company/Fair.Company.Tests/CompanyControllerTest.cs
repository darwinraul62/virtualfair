using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using Fair.Company.Api;
using Fair.Company.Api.Models;
using Fair.Company.Data;
using Fair.Company.Tests.Infrastruture;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Fair.Company.Test;

public class CompanyControllerTest
{
    private readonly CompanyController companyController;
    private readonly Mock<IPersistenceService> persistenceService;

    public CompanyControllerTest()
    {
        persistenceService = new Mock<IPersistenceService>();
        var serviceProvider = FakeServiceProvider.Get();
        var mapper = (IMapper)serviceProvider.GetRequiredService(typeof(IMapper)); 
        
        this.companyController = new CompanyController(persistenceService.Object, mapper);
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

    [Fact]
    public async Task Put_Valid_ReturnsNoContent()
    {
        // Arrange
        Guid id = FakeCompanies.DefaultCompanyId;
        CompanyUpdateRequestDTO model = new CompanyUpdateRequestDTO()
        {
            Identification = FakeCompanies.DefaultCompanyIdentification,
            Name = "Test Company"
        };

        this.persistenceService.Setup(x => x.Company.GetFirstOrDefaultAsync(p => p.CompanyId == id, null))
            .ReturnsAsync(FakeCompanies.GetFakeCompanies().FirstOrDefault(p => p.CompanyId == id));

        this.persistenceService.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await this.companyController.Put(id, model);

        // Assert
        Assert.IsType<NoContentResult>(result);
        this.persistenceService.Verify(x => x.Company.GetFirstOrDefaultAsync(p => p.CompanyId == id, null), Times.Once);
        this.persistenceService.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);        
    }

    [Fact]
    public async Task Put_EmptyModel_ReturnsBadRequest()
    {
        // Arrange
        Guid id = FakeCompanies.DefaultCompanyId;
        CompanyUpdateRequestDTO model = new CompanyUpdateRequestDTO();
        this.companyController.ModelState.AddModelError("Identification", "Identification is required");
        this.companyController.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await this.companyController.Put(id,model);

        // Assert
        this.persistenceService.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);        
        var objectResult = (IStatusCodeActionResult)result;
        Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
    }

    [Fact]
    public async Task Patch_ChangeName_ReturnsNotContent()
    {
        // Arrange
        Guid id = FakeCompanies.DefaultCompanyId;
        var company = FakeCompanies.GetFakeCompanies().FirstOrDefault(p => p.CompanyId == id);

        this.persistenceService.Setup(x => x.Company.GetFirstOrDefaultAsync(p => p.CompanyId == id, null))
            .ReturnsAsync(company);

        this.persistenceService.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        JsonPatchDocument<CompanyUpdateRequestDTO> patchDocument = new JsonPatchDocument<CompanyUpdateRequestDTO>();
        CompanyUpdateRequestDTO model = new CompanyUpdateRequestDTO()
        {
            Name = "ABC"            
        };

        patchDocument.Replace( e => e.Name, model.Name);

        var result = await this.companyController.Patch(id, patchDocument);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.NotNull(company);
        Assert.Equal(model.Name, company?.Name);
        this.persistenceService.Verify(x => x.Company.GetFirstOrDefaultAsync(p => p.CompanyId == id, null), Times.Once);
        this.persistenceService.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Patch_NotExists_ReturnsNotFound()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        this.persistenceService.Setup(x => x.Company.GetFirstOrDefaultAsync(p => p.CompanyId == id, null))
            .ReturnsAsync(FakeCompanies.GetFakeCompanies().FirstOrDefault(p => p.CompanyId == id));

        // Act
        JsonPatchDocument<CompanyUpdateRequestDTO> patchDocument = new JsonPatchDocument<CompanyUpdateRequestDTO>();
        CompanyUpdateRequestDTO model = new CompanyUpdateRequestDTO()
        {
            Name = "ABC"
        };

        patchDocument.Replace( e => e.Name, model.Name);

        var result = await this.companyController.Patch(id, patchDocument);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_Valid_ReturnsNotContent()
    {
        // Arrange
        Guid id = FakeCompanies.DefaultCompanyId;
        this.persistenceService.Setup(x => x.Company.GetFirstOrDefaultAsync(p => p.CompanyId == id, null))
            .ReturnsAsync(FakeCompanies.GetFakeCompanies().FirstOrDefault(p => p.CompanyId == id));

        this.persistenceService.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await this.companyController.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        this.persistenceService.Verify(x => x.Company.GetFirstOrDefaultAsync(p => p.CompanyId == id, null), Times.Once);
        this.persistenceService.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);  
    }
    

    [Fact]
    public async Task Delete_NotExists_ReturnsNotFound()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        this.persistenceService.Setup(x => x.Company.GetFirstOrDefaultAsync(p => p.CompanyId == id, null))
            .ReturnsAsync(FakeCompanies.GetFakeCompanies().FirstOrDefault(p => p.CompanyId == id));

        // Act
        var result = await this.companyController.Delete(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}