namespace CourierCompanyApi.Models.Dto
{
    public class AddressDto
    {
        public string streetName { get; set; }

        public int houseNumber { get; set; }

        public int? flatNumber { get; set; }

        public string postcode { get; set; }

        public string city { get; set; }
    }
}
