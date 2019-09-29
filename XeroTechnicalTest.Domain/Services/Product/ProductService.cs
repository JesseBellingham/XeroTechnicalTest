using System.Collections.Generic;
using System.Linq;
using XeroTechnicalTest.Domain.Constants;

namespace XeroTechnicalTest.Domain.Services.Product
{
    public class ProductService : IProductService
    {
        // in a proper application, these would be coming from some seeded data store.
        private readonly List<Models.Product> _products = new List<Models.Product>
        {
            new Models.ProductProduce
            {
                Name = "Apple",
                ProductType = ProductType.Produce,
                CostInc = 143
            },
            new Models.ProductProduce
            {
                Name = "Orange",
                ProductType = ProductType.Produce,
                CostInc = 212
            },
            new Models.ProductProduce
            {
                Name = "Banana",
                ProductType = ProductType.Produce,
                CostInc = 189
            },
            new Models.ProductDairy
            {
                Name = "Milk",
                ProductType = ProductType.Dairy,
                CostInc = 278
            }
        };

        public IEnumerable<Models.Product> Products()
        {
            return _products;
        }
    }
}