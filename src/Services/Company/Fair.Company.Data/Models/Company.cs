namespace Fair.Company.Data.Models;
public class Company
{
    public Guid CompanyId { get; set; }
    public string? Identification { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }    
}