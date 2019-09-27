using System;
using System.Collections.Generic;
using System.Linq;
using XeroTechnicalTest.Domain.Models;

namespace XeroTechnicalTest.Domain.Services.Invoice
{
    public class InvoiceService : IInvoiceService
    {

        public void AddInvoiceLine(Models.Invoice invoice, InvoiceLine line)
        {
            invoice.LineItems.Add(line);
        }

        public void AddInvoiceLines(Models.Invoice invoice, ICollection<InvoiceLine> lines)
        {
            invoice.LineItems.AddRange(lines);
        }

        public void RemoveInvoiceLine(Models.Invoice invoice, Guid lineId)
        {
            var item = invoice.LineItems.SingleOrDefault(_ => _.Id == lineId);
            invoice.LineItems.Remove(item);
        }

        /// <summary>
        /// GetTotal should return the sum of (Cost * Quantity) for each line item
        /// </summary>
        public decimal GetTotal(Models.Invoice invoice)
        {
            try
            {
                return (decimal) invoice.LineItems.Sum(_ => _.Cost * _.Quantity);
            }
            catch (InvalidCastException ex)
            {
                // todo: add logging
                throw;
            }
        }


        /// <summary>
        /// Creates a deep clone of the current invoice (all fields and properties)
        /// </summary>
        public Models.Invoice Clone(Models.Invoice sourceInvoice)
        {
            //todo: use reflection?
            return new Models.Invoice
            {
                Id = sourceInvoice.Id, // this seems a little smelly to me, but the method summary says ALL fields and properties
                InvoiceDate = sourceInvoice.InvoiceDate,
                InvoiceNumber = sourceInvoice.InvoiceNumber,
                LineItems = sourceInvoice.LineItems
            };
        }
    }
}