using System.Net;
using AutoMapper;
using Fair.Company.Api.Models;
using Fair.Company.Data;
using Fair.Company.Data.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fair.Company.Api;

[Route("api/[controller]")]
public class CompanyController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IPersistenceService persistenceService;
    private readonly IMapper mapper;

    public CompanyController(IPersistenceService persistenceService, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(persistenceService);
        this.persistenceService = persistenceService;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var companies = await this.persistenceService.Company.GetAsync();
        return Ok(companies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        [FromRoute, BindRequired] Guid id)
    {
        var company = await this.persistenceService.Company.GetFirstOrDefaultAsync(x => x.CompanyId == id);

        if (company == null)
            return NotFound();

        return Ok(company);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CompanyInsertResponseDTO), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Post(
        [FromBody]CompanyInsertRequestDTO model)
    {
        if (!this.ModelState.IsValid)
            return BadRequest();

        if (await this.persistenceService.Company.ExistsAsync(p => p.Identification == model.Identification))
            return this.Conflict("Company Identification already exists");

        Company.Data.Models.Company modelInsert = this.mapper.Map<Company.Data.Models.Company>(model);      

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
        [FromRoute, BindRequired] Guid id,
        [FromBody] CompanyUpdateRequestDTO model)
    {
        if (!this.ModelState.IsValid)
            return BadRequest();

        Company.Data.Models.Company? modelUpdate = await this.persistenceService.Company.GetFirstOrDefaultAsync(x => x.CompanyId == id);
        if (modelUpdate == null)
            return NotFound();

        mapper.Map(model, modelUpdate);

        this.persistenceService.Company.Update(modelUpdate);
        await persistenceService.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> Patch(
        [FromRoute, BindRequired] Guid id,
        [FromBody] JsonPatchDocument<CompanyUpdateRequestDTO> patchDocument)
    {
        Company.Data.Models.Company? modelUpdate = await this.persistenceService.Company.GetFirstOrDefaultAsync(x => x.CompanyId == id);
        if (modelUpdate == null)
            return NotFound();

        var modelApi = mapper.Map<CompanyUpdateRequestDTO>(modelUpdate);
        patchDocument.ApplyTo(modelApi);
        mapper.Map(modelApi, modelUpdate);

        this.persistenceService.Company.Update(modelUpdate);
        await this.persistenceService.SaveChangesAsync();        

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute, BindRequired] Guid id)
    {
        Company.Data.Models.Company? modelDelete = await this.persistenceService.Company.GetFirstOrDefaultAsync(x => x.CompanyId == id);
        if (modelDelete == null)
            return NotFound();

        this.persistenceService.Company.Remove(modelDelete);
        await persistenceService.SaveChangesAsync();

        return NoContent();
    }
}