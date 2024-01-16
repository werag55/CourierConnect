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
            CreateMap<Request, RequestDto>();
            CreateMap<Request, RequestDto>().ReverseMap();
			CreateMap<Request, RequestStatusDto>();
			CreateMap<Request, RequestStatusDto>().ReverseMap();
			CreateMap<Request, RequestResponseDto>();
			CreateMap<Request, RequestResponseDto>().ReverseMap();
			CreateMap<Request, RequestSendDto>();
            CreateMap<Request, RequestSendDto>().ReverseMap();
            CreateMap<Request, RequestRejectDto>();
            CreateMap<Request, RequestRejectDto>().ReverseMap();
            CreateMap<Request, RequestAcceptDto>();
            CreateMap<Request, RequestAcceptDto>().ReverseMap();
            CreateMap<PersonalData, PersonalDataDto>();
            CreateMap<PersonalData, PersonalDataDto>().ReverseMap();
        }
    }
}
