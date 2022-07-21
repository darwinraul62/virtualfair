namespace Fair.Company.Data.Models;
public class Company
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
    public string? InstagramAddress { get; set; }
    public bool Active { get; set; }    
}