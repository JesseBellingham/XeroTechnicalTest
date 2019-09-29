using XeroTechnicalTest.Domain.Constants;

namespace XeroTechnicalTest.Domain.Models
{
    public class ProductProduce : Product
    {
        public override ProductType ProductType { get; set; } = ProductType.Produce;
    }
}