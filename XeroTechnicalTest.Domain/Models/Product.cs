using XeroTechnicalTest.Domain.Constants;

namespace XeroTechnicalTest.Domain.Models
{
    public class Product : BaseModel
    {
        public string ProductCode => $"{ProductType.ToString()} - {ProductName}";

        public double Cost { get; set; }
        public string ProductName { get; set; }
        public ProductType ProductType { get; set; }
    }
}