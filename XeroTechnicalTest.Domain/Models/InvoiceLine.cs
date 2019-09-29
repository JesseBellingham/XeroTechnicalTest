using System;

namespace XeroTechnicalTest.Domain.Models
{
    public class InvoiceLine : BaseModel
    {
        public Product Product { get; set; }

        public string Description => $"{Product.Name} @ {Product.CostInc}";

        // using long instead of int, just in case
        public long Quantity { get; set; }
    }
}