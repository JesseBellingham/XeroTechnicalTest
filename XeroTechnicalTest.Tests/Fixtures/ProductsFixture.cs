using System.Linq;
using XeroTechnicalTest.Domain.Constants;
using XeroTechnicalTest.Domain.Models;
using XeroTechnicalTest.Domain.Services.Product;

namespace XeroTechnicalTest.Tests.Fixtures
{
    public class ProductsFixture
    {
        public readonly Product Apple;
        public readonly Product Orange;
        public readonly Product Milk;
        public readonly Product Banana;
        
        public ProductsFixture()
        {
            // because the product service defines all of these in code,
            // mocking them out for easy access in the test class is pretty straight forward.
            
            var productService = new ProductService();
            Apple = productService.Products()
                .OfType<ProductProduce>()
                .SingleOrDefault(_ => _.Code == Products.AppleProductCode);
            
            Orange = productService.Products()
                .OfType<ProductProduce>()
                .SingleOrDefault(_ => _.Code == Products.OrangeProductCode);
            
            Milk = productService.Products()
                .OfType<ProductDairy>()
                .SingleOrDefault(_ => _.Code == Products.MilkProductCode);
            Banana = productService.Products()
                .OfType<ProductProduce>()
                .SingleOrDefault(_ => _.Code == Products.BananaProductCode);
        }
    }
}