using AutoMapper;
using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Models.Dto.Currier;

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



            CreateMap<CurrierInquiryDto, InquiryDto>()
               .ForMember(dest => dest.pickupDate, opt => opt.MapFrom(src => src.pickupDate))
               .ForMember(dest => dest.deliveryDate, opt => opt.MapFrom(src => src.deliveryDay))
               .ForMember(dest => dest.isPriority, opt => opt.MapFrom(src => src.priority == "High"))
               .ForMember(dest => dest.weekendDelivery, opt => opt.MapFrom(src => src.deliveryInWeekend))
               .ForMember(dest => dest.isCompany, opt => opt.MapFrom(src => src.isCompany))
               .ForMember(dest => dest.sourceAddress, opt => opt.MapFrom(src => src.source))
               .ForMember(dest => dest.destinationAddress, opt => opt.MapFrom(src => src.destination))
               .ForMember(dest => dest.package, opt => opt.MapFrom(src => new PackageDto
               {
                   width = src.dimensions.width,
                   height = src.dimensions.height,
                   length = src.dimensions.length,
                   dimensionsUnit = src.dimensions.dimensionUnit == "Inches" ? DimensionUnit.Inches : DimensionUnit.Meters,
                   weight = src.weight,
                   weightUnit = src.weightUnit == "Pounds" ? WeightUnit.Pounds : WeightUnit.Kilograms
               }));
            CreateMap<InquiryDto, CurrierInquiryDto>()
               .ForMember(dest => dest.pickupDate, opt => opt.MapFrom(src => src.pickupDate))
               .ForMember(dest => dest.deliveryDay, opt => opt.MapFrom(src => src.deliveryDate))
               .ForMember(dest => dest.priority, opt => opt.MapFrom(src => src.isPriority ? "High" : "Low"))
               .ForMember(dest => dest.deliveryInWeekend, opt => opt.MapFrom(src => src.weekendDelivery))
               .ForMember(dest => dest.isCompany, opt => opt.MapFrom(src => src.isCompany))
               .ForMember(dest => dest.source, opt => opt.MapFrom(src => src.sourceAddress))
               .ForMember(dest => dest.destination, opt => opt.MapFrom(src => src.destinationAddress))
               .ForMember(dest => dest.dimensions, opt => opt.MapFrom(src => new DimensionsDto
               {
                   width = (float)src.package.width,
                   height = (float)src.package.height,
                   length = (float)src.package.length,
                   dimensionUnit = src.package.dimensionsUnit == DimensionUnit.Inches ? "Inches" : "Meters"
               }))
               .ForMember(dest => dest.weight, opt => opt.MapFrom(src => src.package.weight))
               .ForMember(dest => dest.weightUnit, opt => opt.MapFrom(src => src.package.weightUnit == WeightUnit.Pounds ? "Pounds" : "Kilograms"))
               .ForMember(dest => dest.currency, opt => opt.MapFrom(src => "Pln"));

            CreateMap<CurrierAddressDto, AddressDto>()
                .ForMember(dest => dest.streetName, opt => opt.MapFrom(src => src.street))
                .ForMember(dest => dest.houseNumber, opt => opt.ConvertUsing(new StringIntConverter(), src => src.houseNumber))
                .ForMember(dest => dest.flatNumber, opt =>  opt.ConvertUsing(new StringNullableIntConverter(), src => src.apartmentNumber))
                .ForMember(dest => dest.postcode, opt => opt.MapFrom(src => src.zipCode))
                .ForMember(dest => dest.city, opt => opt.MapFrom(src => src.city));

            CreateMap<AddressDto, CurrierAddressDto>()
                .ForMember(dest => dest.street, opt => opt.MapFrom(src => src.streetName))
                .ForMember(dest => dest.houseNumber, opt => opt.MapFrom(src => src.houseNumber.ToString()))
                .ForMember(dest => dest.apartmentNumber, opt => opt.MapFrom(src => src.flatNumber.ToString()))
                .ForMember(dest => dest.zipCode, opt => opt.MapFrom(src => src.postcode))
                .ForMember(dest => dest.city, opt => opt.MapFrom(src => src.city))
                .ForMember(dest => dest.country, opt => opt.MapFrom(src => "Poland"));

            CreateMap<OfferDto, CurrierOfferDto>()
                .ForMember(dest => dest.inquiryId, opt => opt.MapFrom(src => src.companyOfferId))
                .ForMember(dest => dest.totalPrice, opt => opt.MapFrom(src => src.price + src.taxes + src.fees))
                .ForMember(dest => dest.currency, opt => opt.MapFrom(src => src.currency.ToString()))
                .ForMember(dest => dest.expiringAt, opt => opt.MapFrom(src => src.expirationDate));

            CreateMap<CurrierOfferDto, OfferDto>()
                .ForMember(dest => dest.companyOfferId, opt => opt.MapFrom(src => src.inquiryId))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.totalPrice))
                .ForMember(dest => dest.taxes, opt => opt.MapFrom(src => 0)) 
                .ForMember(dest => dest.fees, opt => opt.MapFrom(src => 0))  
                .ForMember(dest => dest.currency, opt => opt.MapFrom(src => Enum.Parse<Currency>(src.currency)))
                .ForMember(dest => dest.expirationDate, opt => opt.MapFrom(src => src.expiringAt))
                .ForMember(dest => dest.creationDate, opt => opt.MapFrom(src => DateTime.Now));
        }

        public class StringNullableIntConverter : IValueConverter<string, int?>
        {
            public int? Convert(string source, ResolutionContext context)
            {
                if (int.TryParse(source, out var result))
                {
                    return result;
                }

                return 0; // or throw an exception, depending on your requirements
            }
        }

        public class StringIntConverter : IValueConverter<string, int>
        {
            public int Convert(string source, ResolutionContext context)
            {
                if (int.TryParse(source, out var result))
                {
                    return result;
                }

                return 0; // or throw an exception, depending on your requirements
            }
        }
    }
}
