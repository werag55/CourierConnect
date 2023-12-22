namespace CourierCompanyApi.Models.Dto
{
    public class PackageDto
    {
        public double width { get; set; }
        public double height { get; set; }
        public double length { get; set; }
        public string dimensionsUnit { get; set; }
        public double weight { get; set; }
        public string weightUnit { get; set; }
    }
}
