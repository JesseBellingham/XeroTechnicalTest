using System;
using System.Collections.Generic;
using System.Linq;

namespace XeroTechnicalTest.Domain.Models
{
    public class Invoice : BaseModel
    {
        public string Code { get; set; } // this would use a db sequence to auto-increment
        public DateTime InvoiceDate { get; set; }

        public List<InvoiceLine> LineItems { get; set; } = new List<InvoiceLine>();

        public long TotalInc
        {
            get
            {
                return LineItems.Sum(_ => _.Product.CostInc * _.Quantity);
            }
        }
        
        public long TotalEx
        {
            get
            {
                return LineItems.Sum(_ => _.Product.CostEx * _.Quantity);
            }
        }

        public string TotalString => $"${Convert.ToDecimal(TotalInc) / 100}";

        /// <summary>
        /// Outputs string containing the following (replace [] with actual values):
        /// Invoice Number: [InvoiceNumber], InvoiceDate: [DD/MM/YYYY], LineItemCount: [Number of items in LineItems] 
        /// </summary>
        public override string ToString()
        {
            return $"Invoice Number: {Code}, InvoiceDate: {InvoiceDate:dd/MM/yyyy}, LineItemCount: {LineItems.Count}";
        }
    }

    public static class InvoiceExtensions
    {
        
        /// <summary>
        /// MergeInvoices appends the items from the sourceInvoice to the current invoice
        /// </summary>
        /// <param name="targetInvoice">Invoice to merge into</param>
        /// <param name="sourceInvoice">Invoice to merge from</param>
        public static void MergeInvoices(this Invoice targetInvoice, Invoice sourceInvoice)
        {
            targetInvoice.LineItems.AddRange(sourceInvoice.LineItems);
        }
    }
}