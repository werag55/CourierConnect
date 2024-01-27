using AutoMapper;
using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Models.Dto.Currier;
using CourierConnectWeb.Services.Currier;
using CourierConnect.Models.Dto.CourierHub;
using CourierConnectWeb.Services.CourierHub;

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


            #region Currier
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

            CreateMap<CurrierRequestSendDto, RequestSendDto>()
                .ForMember(dest => dest.companyOfferId, opt => opt.MapFrom(src => src.inquiryId))
                .ForPath(dest => dest.personalData.name, opt => opt.MapFrom(src => src.name))
                .ForPath(dest => dest.personalData.address, opt => opt.MapFrom(src => src.address))
                .ForPath(dest => dest.personalData.email, opt => opt.MapFrom(src => src.email));

            CreateMap<RequestSendDto, CurrierRequestSendDto>()
                .ForMember(dest => dest.inquiryId, opt => opt.MapFrom(src => src.companyOfferId))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.personalData.name))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.personalData.email))
                .ForMember(dest => dest.address, opt => opt.MapFrom(src => src.personalData.address));

            CreateMap<CurrierRequestResponseDto, RequestResponseDto>()
            .ForMember(dest => dest.companyRequestId, opt => opt.MapFrom(src => src.offerRequestId))
            .ForMember(dest => dest.decisionDeadline, opt => opt.MapFrom(src => src.validTo));

            CreateMap<RequestResponseDto, CurrierRequestResponseDto>()
                .ForMember(dest => dest.offerRequestId, opt => opt.MapFrom(src => src.companyRequestId))
                .ForMember(dest => dest.validTo, opt => opt.MapFrom(src => src.decisionDeadline));

            CreateMap<CurrierRequestStatusDto, RequestStatusDto>();
            CreateMap<CurrierRequestStatusDto, RequestStatusDto>().ReverseMap();

            CreateMap<CurrierRequestStatusDto, RequestAcceptDto>()
                .ForMember(dest => dest.requestStatus, opt => opt.MapFrom(src => RequestStatus.Accepted))
                .ForMember(dest => dest.companyDeliveryId, opt => opt.MapFrom(src => src.offerId));

            CreateMap<RequestAcceptDto, CurrierRequestStatusDto>()
                .ForMember(dest => dest.offerId, opt => opt.MapFrom(src => src.companyDeliveryId));

            CreateMap<CurrierDeliveryDto, DeliveryDto>()
                .ForMember(dest => dest.companyDeliveryId, opt => opt.MapFrom(src => src.offerId))
                .ForMember(dest => dest.courier, opt => opt.MapFrom(src => new CourierDto
                {
                    name = string.Empty,
                    surname = string.Empty
                }))
                .ForMember(dest => dest.request, opt => opt.MapFrom(src => new RequestDto
                {
                    offer = new OfferDto
                    {
                        companyOfferId = src.offerId,
                        expirationDate = src.validTo,
                        creationDate = src.inquireDate,
                        price = (decimal)src.totalPrice,
                        taxes = 0,
                        fees = 0,
                        currency = MapCurrency(src.currency),
                        inquiry = new InquiryDto
                        {
                            pickupDate = src.pickupDate,
                            deliveryDate = src.deliveryDate,
                            isPriority = src.priority == "High",
                            weekendDelivery = src.deliveryInWeekend,
                            isCompany = false,
                            sourceAddress = new AddressDto
                            {
                                streetName = src.source.street,
                                houseNumber = stringIntConverter(src.source.houseNumber),
                                flatNumber = stringIntConverter(src.source.apartmentNumber),
                                postcode = src.source.zipCode,
                                city = src.source.city
                            },
                            destinationAddress = new AddressDto
                            {
                                streetName = src.destination.street,
                                houseNumber = stringIntConverter(src.destination.houseNumber),
                                flatNumber = stringIntConverter(src.destination.apartmentNumber),
                                postcode = src.destination.zipCode,
                                city = src.destination.city
                            },
                            package = new PackageDto
                            {
                                width = src.dimensions.width,
                                height = src.dimensions.height,
                                length = src.dimensions.length,
                                dimensionsUnit = src.dimensions.dimensionUnit == "Inches" ? DimensionUnit.Inches : DimensionUnit.Meters,
                                weight = src.weight,
                                weightUnit = src.weightUnit == "Pounds" ? WeightUnit.Pounds : WeightUnit.Kilograms
                            }
                        }
                    },
                    requestStatus = RequestStatus.Accepted,
                    personalData = new PersonalDataDto
                    {
                        name = src.buyerName ?? string.Empty,
                        surname = string.Empty,
                        companyName = string.Empty,
                        address = new AddressDto
                        {
                            streetName = src.buyerAddress.street,
                            houseNumber = stringIntConverter(src.buyerAddress.houseNumber),
                            flatNumber = stringIntConverter(src.buyerAddress.apartmentNumber),
                            postcode = src.buyerAddress.zipCode,
                            city = src.buyerAddress.city
                        },
                        email = string.Empty
                    },
                    rejectionReason = null
                }))
                .ForMember(dest => dest.cancelationDeadline, opt => opt.MapFrom(src => src.validTo))
                .ForMember(dest => dest.pickUpDate, opt => opt.MapFrom(src => src.pickupDate))
                .ForMember(dest => dest.deliveryDate, opt => opt.MapFrom(src => src.deliveryDate))
                .ForMember(dest => dest.deliveryStatus, opt => opt.MapFrom(src => MapDeliveryStatus(src.offerStatus)))
                .ForMember(dest => dest.reason, opt => opt.Ignore());

            #endregion

            #region CourierHub

            CreateMap<AddressDto, CourierHubAddressDto>()
                .ForMember(dest => dest.city, opt => opt.MapFrom(src => src.city))
                .ForMember(dest => dest.postalCode, opt => opt.MapFrom(src => src.postcode))
                .ForMember(dest => dest.street, opt => opt.MapFrom(src => src.streetName))
                .ForMember(dest => dest.number, opt => opt.MapFrom(src => src.houseNumber.ToString()))
                .ForMember(dest => dest.flat, opt => opt.MapFrom(src => src.flatNumber.HasValue ? src.flatNumber.ToString() : null));

            CreateMap<CourierHubAddressDto, AddressDto>()
                .ForMember(dest => dest.city, opt => opt.MapFrom(src => src.city))
                .ForMember(dest => dest.postcode, opt => opt.MapFrom(src => src.postalCode))
                .ForMember(dest => dest.streetName, opt => opt.MapFrom(src => src.street))
                .ForMember(dest => dest.houseNumber, opt => opt.MapFrom(src => stringIntConverter(src.number)))
                .ForMember(dest => dest.flatNumber, opt => opt.MapFrom(src => stringIntConverter(src.flat)));

            CreateMap<InquiryDto, CourierHubInquiryDto>()
                .ForMember(dest => dest.depth, opt => opt.MapFrom(src => src.package.height))
                .ForMember(dest => dest.width, opt => opt.MapFrom(src => src.package.width))
                .ForMember(dest => dest.length, opt => opt.MapFrom(src => src.package.length))
                .ForMember(dest => dest.mass, opt => opt.MapFrom(src => src.package.weight))
                .ForMember(dest => dest.sourceAddress, opt => opt.MapFrom(src => src.sourceAddress))
                .ForMember(dest => dest.destinationAddress, opt => opt.MapFrom(src => src.destinationAddress))
                .ForMember(dest => dest.sourceDate, opt => opt.MapFrom(src => src.pickupDate))
                .ForMember(dest => dest.destinationDate, opt => opt.MapFrom(src => src.deliveryDate))
                .ForMember(dest => dest.datetime, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.isCompany, opt => opt.MapFrom(src => src.isCompany))
                .ForMember(dest => dest.isWeekend, opt => opt.MapFrom(src => src.weekendDelivery))
                .ForMember(dest => dest.priority, opt => opt.MapFrom(src => src.isPriority ? PriorityType.High : PriorityType.Low));

            CreateMap<CourierHubInquiryDto, InquiryDto>()
                .ForMember(dest => dest.pickupDate, opt => opt.MapFrom(src => src.sourceDate))
                .ForMember(dest => dest.deliveryDate, opt => opt.MapFrom(src => src.destinationDate)) 
                .ForMember(dest => dest.isPriority, opt => opt.MapFrom(src => src.priority != PriorityType.Low))
                .ForMember(dest => dest.weekendDelivery, opt => opt.MapFrom(src => src.isWeekend))
                .ForMember(dest => dest.isCompany, opt => opt.MapFrom(src => src.isCompany))
                .ForMember(dest => dest.sourceAddress, opt => opt.MapFrom(src => src.sourceAddress))
                .ForMember(dest => dest.destinationAddress, opt => opt.MapFrom(src => src.destinationAddress))
                .ForMember(dest => dest.package, opt => opt.MapFrom(src => new PackageDto
                {
                    height = src.depth,
                    width = src.width,
                    length = src.length,
                    weight = src.mass
                }));

            CreateMap<OfferDto, CourierHubOfferDto>()
            .ForMember(dest => dest.price, opt => opt.MapFrom(src => (double)src.price))
            .ForMember(dest => dest.code, opt => opt.MapFrom(src => src.companyOfferId))
            .ForMember(dest => dest.expirationDate, opt => opt.MapFrom(src => src.expirationDate));

            CreateMap<CourierHubOfferDto, OfferDto>() // Uzupełnić InquiryDto i companyId
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => (decimal)src.price))
                .ForMember(dest => dest.companyOfferId, opt => opt.MapFrom(src => src.code))
                .ForMember(dest => dest.creationDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.expirationDate, opt => opt.MapFrom(src => src.expirationDate))
                .ForMember(dest => dest.taxes, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.fees, opt => opt.MapFrom(src => 0));

            CreateMap<RequestSendDto, CourierHubRequestSendDto>()
                .ForMember(dest => dest.inquireCode, opt => opt.MapFrom(src => src.companyOfferId))
                .ForMember(dest => dest.clientName, opt => opt.MapFrom(src => src.personalData.name))
                .ForMember(dest => dest.clientSurname, opt => opt.MapFrom(src => src.personalData.surname))
                .ForMember(dest => dest.clientEmail, opt => opt.MapFrom(src => src.personalData.email))
                .ForMember(dest => dest.clientPhoneNumber, opt => opt.MapFrom(src => "234457234"))
                .ForMember(dest => dest.clientCompany, opt => opt.MapFrom(src => src.personalData.companyName))
                .ForMember(dest => dest.clientAddress, opt => opt.MapFrom(src => src.personalData.address));

            CreateMap<CourierHubRequestSendDto, RequestResponseDto>()
                .ForMember(dest => dest.companyRequestId, opt => opt.MapFrom(src => src.inquireCode))
                .ForMember(dest => dest.decisionDeadline, opt => opt.MapFrom(src => DateTime.MaxValue));


            #endregion
        }

        private static int stringIntConverter(string s)
        {
            return int.TryParse(s, out var result) ? result : 0;
        }

        private static Currency MapCurrency(string status)
        {
            return status switch
            {
                "Pln" => Currency.PLN,
                "PLN" => Currency.PLN,
                "USD" => Currency.PLN,
                "Usd" => Currency.PLN,
                "EUR" => Currency.PLN,
                "Eur" => Currency.PLN,
                "GBP" => Currency.PLN,
                "Gbp" => Currency.PLN,
                _ => Currency.PLN 
            };
        }

        private static DeliveryStatus MapDeliveryStatus(string status)
        {
            return status switch
            {
                "Pending" => DeliveryStatus.Proccessing,
                "Accepted" => DeliveryStatus.Proccessing,
                "Delivered" => DeliveryStatus.Delivered,
                "CannotDeliver" => DeliveryStatus.CannotDeliver,
                "Canceled" => DeliveryStatus.Canceled,
                _ => DeliveryStatus.Proccessing // Domyślna wartość
            };
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
