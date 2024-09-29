using AutoMapper;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Mappers
{
    public class PromoCodeMappingProfile : Profile
    {
        public PromoCodeMappingProfile()
        {
            CreateMap<GivePromoCodeRequest, PromoCode>();
            CreateMap<PromoCode, PromoCodeShortResponse>();
        }
    }
}
