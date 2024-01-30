namespace CourierConnect.Models.Dto
{
    public class PackageDto
    {
        public double width { get; set; }
        public double height { get; set; }
        public double length { get; set; }
        public DimensionUnit dimensionsUnit { get; set; }
        public double weight { get; set; }
        public WeightUnit weightUnit { get; set; }
    }
}
