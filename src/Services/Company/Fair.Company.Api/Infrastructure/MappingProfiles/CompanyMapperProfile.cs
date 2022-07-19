using AutoMapper;

namespace Fair.Company.Api.Infrastructure.MappingProfiles;

public class CompanyMapperProfile : Profile
{
    public CompanyMapperProfile()
    {
        CreateMap<Company.Data.Models.Company,Models.CompanyUpdateRequestDTO>().ReverseMap();
        CreateMap<Company.Data.Models.Company,Models.CompanyInsertRequestDTO>().ReverseMap();        
    }
}