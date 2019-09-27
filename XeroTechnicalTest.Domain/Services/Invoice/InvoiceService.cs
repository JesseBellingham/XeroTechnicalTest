using System;
using System.Collections.Generic;
using System.Linq;
using XeroTechnicalTest.Domain.Models;

namespace XeroTechnicalTest.Domain.Services.Invoice
{
    public class InvoiceService : IInvoiceService
    {
        public void RemoveInvoiceLine(Models.Invoice invoice, Guid lineId)
        {
            var item = invoice.LineItems.SingleOrDefault(_ => _.Id == lineId);
            invoice.LineItems.Remove(item);
        }

        public Models.Invoice CreateInvoice(List<InvoiceLine> lines)
        {
            return new Models.Invoice
            {
                LineItems = lines
            };
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