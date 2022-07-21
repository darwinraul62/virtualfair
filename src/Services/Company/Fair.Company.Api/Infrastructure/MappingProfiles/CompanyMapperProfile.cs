using AutoMapper;

namespace Fair.Company.Api.Infrastructure.MappingProfiles;

public class CompanyMapperProfile : Profile
{
    public CompanyMapperProfile()
    {
        CreateMap<Company.Data.Models.Company,Models.CompanyUpdateRequestDTO>().ReverseMap();
        CreateMap<Models.CompanyInsertRequestDTO, Company.Data.Models.Company>().BeforeMap((src, dest) =>{
            dest.CompanyId = Guid.NewGuid();
            dest.Active = true;
        });
    }
}