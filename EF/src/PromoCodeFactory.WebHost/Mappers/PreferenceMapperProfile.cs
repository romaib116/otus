using AutoMapper;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Mappers
{
    public class PreferenceMapperProfile : Profile
    {
        public PreferenceMapperProfile()
        {
            CreateMap<Preference, PreferenceResponse>();
            CreateMap<CustomerPreference, PreferenceResponse>()
                .ForMember(d => d.Name, map => map.MapFrom(m => m.Preference.Name));
        }
    }
}
