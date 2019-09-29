using XeroTechnicalTest.Domain.Constants;

namespace XeroTechnicalTest.Domain.Models
{
    public class ProductDairy : Product
    {
        public override ProductType ProductType { get; set; } = ProductType.Dairy;
    }
}