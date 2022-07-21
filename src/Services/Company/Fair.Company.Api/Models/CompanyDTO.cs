using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fair.Company.Api.Models;

public class CompanyDTO
{
    public Guid CompanyId { get; set; }
    public string? Identification { get; set; }
    public string? Name { get; set; }
    public string? LegalName { get; set; }
    public string? PhoneNumber1 { get; set; }
    public string? PhoneNumber2 { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? FacebookAddress { get; set; }
    public string? TwitterAddress { get; set; }
    public string? YoutubeAddress { get; set; }
    public bool Active { get; set; }
}

public class CompanyInsertRequestDTO
{
    [BindRequired]
    public string? Identification { get; set; }
    [BindRequired]
    public string? Name { get; set; }
    public string? LegalName { get; set; }
    public string? PhoneNumber1 { get; set; }
    public string? PhoneNumber2 { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? FacebookAddress { get; set; }
    public string? TwitterAddress { get; set; }
    public string? YoutubeAddress { get; set; }  
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
    public string? LegalName { get; set; }
    public string? PhoneNumber1 { get; set; }
    public string? PhoneNumber2 { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? FacebookAddress { get; set; }
    public string? TwitterAddress { get; set; }
    public string? YoutubeAddress { get; set; }  
    [BindRequired]
    public bool Active { get; set; }
}