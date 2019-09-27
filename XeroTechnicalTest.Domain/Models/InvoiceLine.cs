using System;

namespace XeroTechnicalTest.Domain.Models
{
    public class InvoiceLine : BaseModel
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Cost { get; set; }
    }
}