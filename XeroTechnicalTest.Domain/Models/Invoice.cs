using System;
using System.Collections.Generic;
using System.Linq;

namespace XeroTechnicalTest.Domain.Models
{
    public class Invoice : BaseModel
    {
        public int InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }

        public List<InvoiceLine> LineItems { get; set; } = new List<InvoiceLine>();

        public decimal Total
        {
            get
            {
                return (decimal) LineItems.Sum(_ => _.Product.Cost * _.Quantity);
            }
        }

        /// <summary>
        /// Outputs string containing the following (replace [] with actual values):
        /// Invoice Number: [InvoiceNumber], InvoiceDate: [DD/MM/YYYY], LineItemCount: [Number of items in LineItems] 
        /// </summary>
        public override string ToString()
        {
            return $"Invoice Number: {InvoiceNumber}, InvoiceDate: {InvoiceDate:dd/MM/yyyy}, LineItemCount: {LineItems.Count}";
        }
    }

    public static class InvoiceExtensions
    {
        
        /// <summary>
        /// MergeInvoices appends the items from the sourceInvoice to the current invoice
        /// </summary>
        /// <param name="sourceInvoice">Invoice to merge from</param>
        public static void MergeInvoices(this Invoice targetInvoice, Invoice sourceInvoice)
        {
            targetInvoice.LineItems.AddRange(sourceInvoice.LineItems);
        }
    }
}