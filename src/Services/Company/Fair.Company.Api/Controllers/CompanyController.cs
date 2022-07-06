using System.Net;
using Fair.Company.Api.Models;
using Fair.Company.Data;
using Fair.Company.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fair.Company.Api;

[Route("api/[controller]")]
public class CompanyController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IPersistenceService persistenceService;

    public CompanyController(IPersistenceService persistenceService)
    {
        ArgumentNullException.ThrowIfNull(persistenceService);
        this.persistenceService = persistenceService;        
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var companies = await this.persistenceService.Company.GetAsync();
        return Ok(companies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var companies = await this.persistenceService.Company.GetAsync(x => x.CompanyId == id);
        return Ok(companies);
    }    

    [HttpPost]
    [ProducesResponseType(typeof(CompanyInsertResponseDTO), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Post(CompanyInsertRequestDTO model)
    {        
        if(this.ModelState.IsValid)
            return BadRequest();

        Company.Data.Models.Company modelInsert = new Company.Data.Models.Company()
        {
            Identification = model.Identification,
            Name = model.Name,
            Active = true
        };

        this.persistenceService.Company.Add(modelInsert);
        await persistenceService.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = modelInsert.CompanyId }, new CompanyInsertResponseDTO()
        {
            CompanyId = modelInsert.CompanyId           
        });
    }

    [HttpPut("{id}")]    
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Put(
        [BindRequired]Guid id, 
        CompanyUpdateRequestDTO model)
    {        
        if(this.ModelState.IsValid)
            return BadRequest();

        Company.Data.Models.Company? modelUpdate = await this.persistenceService.Company.GetFirstOrDefaultAsync(x => x.CompanyId == id);
        if(modelUpdate == null)
            return NotFound();

        this.persistenceService.Company.Update(modelUpdate);
        await persistenceService.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        Company.Data.Models.Company? modelDelete = await this.persistenceService.Company.GetFirstOrDefaultAsync(x => x.CompanyId == id);
        if(modelDelete == null)
            return NotFound();

        this.persistenceService.Company.Remove(modelDelete);
        await persistenceService.SaveChangesAsync();

        return NoContent();
    }
}