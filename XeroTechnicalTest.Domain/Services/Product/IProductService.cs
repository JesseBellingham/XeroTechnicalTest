using System.Collections.Generic;
using XeroTechnicalTest.Domain.Constants;

namespace XeroTechnicalTest.Domain.Services.Product
{
    public interface IProductService
    {
        IEnumerable<Models.Product> Products();
    }
}