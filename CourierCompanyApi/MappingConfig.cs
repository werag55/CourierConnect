using AutoMapper;
using CourierCompanyApi.Models;
using CourierCompanyApi.Models.Dto;

namespace CourierCompanyApi
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Offer, OfferDto>();
            CreateMap<Offer, OfferDto>().ReverseMap();
            CreateMap<Inquiry, InquiryDto>();
            CreateMap<Inquiry, InquiryDto>().ReverseMap();
            //CreateMap<Offer, OfferCreateDto>().ReverseMap();
        }
    }
}
