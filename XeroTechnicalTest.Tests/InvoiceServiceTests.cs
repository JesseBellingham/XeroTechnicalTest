using System;
using System.Collections.Generic;
using System.Linq;
using XeroTechnicalTest.Domain.Models;
using XeroTechnicalTest.Domain.Services.Invoice;
using Xunit;
using Xunit.Abstractions;

namespace XeroTechnicalTest.Tests
{
    public class InvoiceServiceTests
    {
        private readonly ITestOutputHelper _output;
        private readonly IInvoiceService _invoiceService;

        public InvoiceServiceTests(
            ITestOutputHelper output)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
            _invoiceService = new InvoiceService();
        }


        [Fact]
        public void CreateInvoiceWithOneItem()
        {
            var invoice = new Invoice();

            _invoiceService.AddInvoiceLine(
                invoice,
                new InvoiceLine
                {
//                InvoiceLineId = "1",
                    Cost = 6.99,
                    Quantity = 1,
                    Description = "Apple"
                });

            var total = _invoiceService.GetTotal(invoice);
            Assert.True(total == 6.99m);
            Console.WriteLine(total);
        }

        [Fact]
        public void CreateInvoiceWithMultipleItemsAndQuantities()
        {
            var invoice = new Invoice();
            var lines = new List<InvoiceLine>
            {
                new InvoiceLine
                {
//                InvoiceLineId = 1,
                    Cost = 10.21,
                    Quantity = 4,
                    Description = "Banana"
                },
                new InvoiceLine
                {
//                InvoiceLineId = 2,
                    Cost = 5.21,
                    Quantity = 1,
                    Description = "Orange"
                },
                new InvoiceLine
                {
//                InvoiceLineId = 3,
                    Cost = 5.21,
                    Quantity = 5,
                    Description = "Pineapple"
                }
            };

            _invoiceService.AddInvoiceLines(invoice, lines);

            var total = _invoiceService.GetTotal(invoice);
            Assert.True(total == 72.1m);
            _output.WriteLine($"{total}");
        }

        [Fact]
        public void RemoveItem()
        {
            var invoice = new Invoice();
            var lines = new List<InvoiceLine>
            {
                new InvoiceLine
                {
//                InvoiceLineId = 1,
                    Cost = 5.21,
                    Quantity = 1,
                    Description = "Orange"
                },
                new InvoiceLine
                {
//                InvoiceLineId = 2,
                    Cost = 10.99,
                    Quantity = 4,
                    Description = "Banana"
                }
            };
            
            _invoiceService.AddInvoiceLines(invoice, lines);
            Assert.True(invoice.LineItems.Count == 2);

            // because I changed ids to be automatically set in the constructor, I can no longer just say
            // remove line with the id 1 or something
            // todo: come back to this
            var invoiceLineId = invoice.LineItems.FirstOrDefault()?.Id;
            if (invoiceLineId == null)
            {
                Console.WriteLine("No invoice line item found to remove.");
            }
            else
            {
                _invoiceService.RemoveInvoiceLine(invoice, (Guid)invoiceLineId);
            }
            Assert.True(invoice.LineItems.Count == 1);
        }

        [Fact]
        public void MergeInvoices()
        {
            var invoice1 = new Invoice();
            _invoiceService.AddInvoiceLine(
                invoice1,
                new InvoiceLine
                {
//                InvoiceLineId = 1,
                    Cost = 10.33,
                    Quantity = 4,
                    Description = "Banana"
                });


            var invoice2 = new Invoice();

            _invoiceService.AddInvoiceLine(
                invoice2,
                new InvoiceLine
                {
//                InvoiceLineId = 2,
                    Cost = 5.22,
                    Quantity = 1,
                    Description = "Orange"
                });

            _invoiceService.AddInvoiceLine(
                invoice2,
                new InvoiceLine
                {
//                InvoiceLineId = 3,
                    Cost = 6.27,
                    Quantity = 3,
                    Description = "Blueberries"
                });

            invoice1.MergeInvoices(invoice2);

            var total = _invoiceService.GetTotal(invoice1);
            Assert.True(total == 65.35m);
        }

        [Fact]
        public void CloneInvoice()
        {
            var invoice = new Invoice();

            _invoiceService.AddInvoiceLine(
                invoice,
                new InvoiceLine
                {
//                InvoiceLineId = 1,
                    Cost = 6.99,
                    Quantity = 1,
                    Description = "Apple"
                });

            _invoiceService.AddInvoiceLine(
                invoice,
                new InvoiceLine
                {
//                InvoiceLineId = 2,
                    Cost = 6.27,
                    Quantity = 3,
                    Description = "Blueberries"
                });

            var clonedInvoice = _invoiceService.Clone(invoice);
            Assert.True(clonedInvoice.LineItems.Count == 2);
            Assert.Contains(clonedInvoice.LineItems, _ => _.Description == "Blueberries" && _.Quantity == 3);
        }

        [Fact]
        public void InvoiceToString()
        {
            var invoice = new Invoice
            {
                InvoiceDate = new DateTime(2019, 09, 27),
                InvoiceNumber = 1000,
                LineItems = new List<InvoiceLine>()
                {
                    new InvoiceLine
                    {
//                        InvoiceLineId = 1,
                        Cost = 6.99,
                        Quantity = 1,
                        Description = "Apple"
                    }
                }
            };

            Assert.Equal("Invoice Number: 1000, InvoiceDate: 27/09/2019, LineItemCount: 1", invoice.ToString());
        }
    }
}