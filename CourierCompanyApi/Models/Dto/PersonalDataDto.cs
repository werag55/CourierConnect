﻿namespace CourierCompanyApi.Models.Dto
{
    public class PersonalDataDto
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string companyName { get; set; }
        public Address address { get; set; }
        public string email { get; set; }
    }
}
