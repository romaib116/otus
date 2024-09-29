using AutoMapper;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Mappers
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<CreateOrEditCustomerRequest, Customer>();
            CreateMap<Customer, CustomerShortResponse>();
            CreateMap<Customer, CustomerResponse>()
                .ForMember(x => x.Preferences, map => map.MapFrom(y => y.Preferences));
        }
    }
}
