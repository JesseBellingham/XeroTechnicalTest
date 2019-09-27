using System;
using System.Collections.Generic;
using XeroTechnicalTest.Domain.Models;

namespace XeroTechnicalTest.Domain.Services.Invoice
{
    public interface IInvoiceService
    {
        void RemoveInvoiceLine(Models.Invoice invoice, Guid lineId);
        Models.Invoice CreateInvoice(List<InvoiceLine> lines);
        Models.Invoice Clone(Models.Invoice invoice);
    }
}