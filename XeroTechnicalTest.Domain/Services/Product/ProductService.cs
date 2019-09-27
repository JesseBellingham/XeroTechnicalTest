using System.Collections.Generic;
using System.Linq;
using XeroTechnicalTest.Domain.Constants;

namespace XeroTechnicalTest.Domain.Services.Product
{
    public class ProductService : IProductService
    {
        // in a proper application, these would be coming from some data store.
        // mocking for tests
        private readonly Models.Product _apple = new Models.Product
        {
            ProductName = "Apple",
            ProductType = ProductType.Produce,
            Cost = 1.43
        };

        private readonly Models.Product _orange = new Models.Product
        {
            ProductName = "Orange",
            ProductType = ProductType.Produce,
            Cost = 2.12
        };

        private readonly Models.Product _banana = new Models.Product
        {
            ProductName = "Banana",
            ProductType = ProductType.Produce,
            Cost = 1.89
        };

        private readonly Models.Product _milk = new Models.Product
        {
            ProductName = "Milk",
            ProductType = ProductType.Dairy,
            Cost = 2.78
        };
        
        
        public IEnumerable<Models.Product> GetAllProducts()
        {
            return new List<Models.Product>
            {
                _apple,
                _orange,
                _banana,
                _milk
            };
        }

        public IEnumerable<Models.Product> GetProductsOfType(ProductType productType)
        {
            // use polymorphism to allow use of .OfType<T>()
            return GetAllProducts().Where(_ => _.ProductType == productType);
        }
    }
}