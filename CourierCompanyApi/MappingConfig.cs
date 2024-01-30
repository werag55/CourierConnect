using AutoMapper;
using CourierCompanyApi.Models;
using CourierCompanyApi.Models.Dto;

namespace CourierCompanyApi
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Offer, OfferDto>()
                .ForMember(dest => dest.companyOfferId, opt => opt.MapFrom(src => src.Id));
            (CreateMap<Offer, OfferDto>().ReverseMap())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.companyOfferId));
            CreateMap<Inquiry, InquiryDto>();
            CreateMap<Inquiry, InquiryDto>().ReverseMap();
            CreateMap<Address, AddressDto>();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Package, PackageDto>();
            CreateMap<Package, PackageDto>().ReverseMap();
            CreateMap<Request, RequestDto>();
            CreateMap<Request, RequestDto>().ReverseMap();
            CreateMap<Request, RequestSendDto>();
            CreateMap<Request, RequestSendDto>().ReverseMap();
            CreateMap<Request, RequestRejectDto>();
            CreateMap<Request, RequestRejectDto>().ReverseMap();
            CreateMap<Request, RequestAcceptDto>();
            CreateMap<Request, RequestAcceptDto>().ReverseMap();
            CreateMap<PersonalData, PersonalDataDto>();
            CreateMap<PersonalData, PersonalDataDto>().ReverseMap();
            CreateMap<Courier, CourierDto>();
            CreateMap<Courier, CourierDto>().ReverseMap();
            CreateMap<Delivery, DeliveryDto>();
            CreateMap<Delivery, DeliveryDto>().ReverseMap();
            //CreateMap<Offer, OfferCreateDto>().ReverseMap();
        }
    }
}
