using System;

namespace XeroTechnicalTest.Domain.Models
{
    public class InvoiceLine : BaseModel
    {
        public Product Product { get; set; }

        public string Description => $"{Product.ProductName} @ {Product.Cost}";

        public int Quantity { get; set; }
    }
}