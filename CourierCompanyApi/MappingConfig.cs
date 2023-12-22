﻿using AutoMapper;
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
            CreateMap<Address, AddressDto>();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Package, PackageDto>();
            CreateMap<Package, PackageDto>().ReverseMap();
            //CreateMap<Offer, OfferCreateDto>().ReverseMap();
        }
    }
}
