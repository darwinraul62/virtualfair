using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fair.Company.Api.Models;

public class CompanyDTO
{
    public Guid CompanyId { get; set; }
    public string? Identification { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }
}

public class CompanyInsertRequestDTO
{
    [BindRequired]
    public string? Identification { get; set; }
    [BindRequired]
    public string? Name { get; set; }    
}

public class CompanyInsertResponseDTO
{
    public Guid CompanyId { get; set; }    
}

public class CompanyUpdateRequestDTO
{
    [BindRequired]
    public string? Identification { get; set; }
    [BindRequired]
    public string? Name { get; set; }
    [BindRequired]
    public bool Active { get; set; }
}