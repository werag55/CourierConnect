using AutoMapper;
using CourierConnect.Models;
using CourierConnect.Models.Dto;

namespace CourierConnect
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Offer, OfferDto>();
            CreateMap<Offer, OfferDto>().ReverseMap();
            CreateMap<Inquiry, InquiryDto>();
            CreateMap<Inquiry, InquiryDto>().ReverseMap();
            CreateMap<Address, AddressDto>();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Package, PackageDto>();
            CreateMap<Package, PackageDto>().ReverseMap();
        }
    }
}
