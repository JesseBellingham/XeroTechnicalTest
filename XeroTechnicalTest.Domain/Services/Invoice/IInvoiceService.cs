using System;
using System.Collections.Generic;
using XeroTechnicalTest.Domain.Models;

namespace XeroTechnicalTest.Domain.Services.Invoice
{
    public interface IInvoiceService
    {
        void AddInvoiceLine(Models.Invoice invoice, InvoiceLine line);
        void AddInvoiceLines(Models.Invoice invoice, ICollection<InvoiceLine> lines);
        void RemoveInvoiceLine(Models.Invoice invoice, Guid lineId);
        decimal GetTotal(Models.Invoice invoice);
        Models.Invoice Clone(Models.Invoice invoice);
    }
}