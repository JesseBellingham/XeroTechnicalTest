using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using XeroTechnicalTest.Domain.Models;

namespace XeroTechnicalTest.Domain.Services.Invoice
{
    /// <summary>
    /// As it stands currently, I have mixed feelings about this class. The methods are all just wrappers for not
    /// particularly complex, fairly self-explanatory logic that you get for free in .NET.
    ///
    /// The logging keeps it from being totally redundant.
    /// </summary>
    public class InvoiceService : IInvoiceService
    {
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(ILogger<InvoiceService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public void RemoveInvoiceLine(Models.Invoice invoice, Guid lineId)
        {
            var item = invoice.LineItems.SingleOrDefault(_ => _.Id == lineId);
            invoice.LineItems.Remove(item);
            _logger.LogInformation($"Line: {lineId} removed from invoice: {invoice.Id}");
        }

        public Models.Invoice CreateInvoice(List<InvoiceLine> lines)
        {
            _logger.LogInformation($"-- Creating invoice --" +
                                   $"Lines: {lines.Select(_ => _.Description)}");
            
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
            _logger.LogInformation($"Deep cloning invoice: {sourceInvoice.Id}");
            //todo: use reflection?
            return new Models.Invoice
            {
                Id = sourceInvoice.Id, // this seems a little smelly to me, but the method summary says ALL fields and properties
                InvoiceDate = sourceInvoice.InvoiceDate,
                Code = sourceInvoice.Code,
                LineItems = sourceInvoice.LineItems
            };
        }
    }
}