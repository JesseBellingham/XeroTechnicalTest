using System;
using System.Collections.Generic;
using System.Linq;
using XeroTechnicalTest.Domain.Constants;
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
        
        // in a proper application, these would be coming from some data store.
        // mocking for tests
        private readonly Product _apple = new Product
        {
            ProductName = "Apple",
            ProductType = ProductType.Produce,
            Cost = 1.43
        };

        private readonly Product _orange = new Product
        {
            ProductName = "Orange",
            ProductType = ProductType.Produce,
            Cost = 2.12
        };

        private readonly Product _banana =
        new Product
        {
            ProductName = "Banana",
            ProductType = ProductType.Produce,
            Cost = 1.89
        };

        private readonly Product _milk =
        new Product
        {
            ProductName = "Milk",
            ProductType = ProductType.Dairy,
            Cost = 2.78
        };

        public InvoiceServiceTests(ITestOutputHelper output)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
            _invoiceService = new InvoiceService();
        }

        [Fact]
        public void InvoiceWithOneLineItemCalculatesCorrectTotal()
        {
            var quantity = 1;
            
            var invoice = _invoiceService.CreateInvoice(
                new List<InvoiceLine>
                {
                    new InvoiceLine
                    {
                        Product = _apple,
                        Quantity = quantity
                    }
                });

            var expectedTotal = (decimal)_apple.Cost * quantity;
            Assert.Equal(expectedTotal, invoice.Total);
        }

        [Fact]
        public void CreateInvoiceWithMultipleItemsAndQuantities()
        {
            var lines = new List<InvoiceLine>
            {
                new InvoiceLine
                {
                    Product = _apple,
                    Quantity = 4,
                },
                new InvoiceLine
                {
                    Product = _milk,
                    Quantity = 1
                },
                new InvoiceLine
                {
                    Product = _orange,
                    Quantity = 5,
                }
            };

            var invoice = _invoiceService.CreateInvoice(lines);
            var expectedTotal = (decimal) (_apple.Cost * 4 + _milk.Cost + _orange.Cost * 5);
            Assert.Equal(expectedTotal,invoice.Total);
        }

        [Theory]
        [InlineData("57d6b1ad-e1f1-4e6d-a8de-86cec478ec06")]
        [InlineData("02ae29d8-74eb-4970-b773-2b6860155a07")]
        public void RemoveItemAtIndexRemovesCorrectItem(Guid id)
        {
            // this is failing, I suspect because the inline data type is a string, rather than a guid
            // I am unable to supply a guid, because it requires a compile time constant
            var lines = new List<InvoiceLine>
            {
                new InvoiceLine
                {
                    Id = Guid.Parse("57d6b1ad-e1f1-4e6d-a8de-86cec478ec06"),
                    Quantity = 1,
                    Product = _orange
                },
                new InvoiceLine
                {
                    Id = Guid.Parse("02ae29d8-74eb-4970-b773-2b6860155a07"),
                    Quantity = 4,
                    Product = _banana
                }
            };
            
            var invoice = _invoiceService.CreateInvoice(lines);
            var lineToKeep = invoice.LineItems.SingleOrDefault(_ => _.Id != id);
            Assert.Equal(2, invoice.LineItems.Count);
            
            _invoiceService.RemoveInvoiceLine(invoice, id);
            Assert.Single(invoice.LineItems);
            Assert.Contains(lineToKeep, invoice.LineItems);
        }

        [Fact]
        public void MergeInvoices()
        {
            var invoice1 = _invoiceService.CreateInvoice(
                new List<InvoiceLine>
                {
                    new InvoiceLine
                    {
                        Quantity = 4,
                        Product = _banana
                    }
                });

            var invoice2Lines = new List<InvoiceLine>
            {
                new InvoiceLine
                {
                    Quantity = 1,
                    Product = _orange
                },
                new InvoiceLine
                {
                    Quantity = 3,
                    Product = _milk
                }
            };
            var invoice2 = _invoiceService.CreateInvoice(invoice2Lines);
            var expectedTotal = (decimal)invoice1.LineItems.Sum(_ => _.Product.Cost * _.Quantity) +
                                (decimal)invoice2.LineItems.Sum(_ => _.Product.Cost * _.Quantity);

            invoice1.MergeInvoices(invoice2);

            
            Assert.Equal(expectedTotal, invoice1.Total);
        }

        [Fact]
        public void ClonedInvoiceCorrectlyDeepClonesTargetInvoice()
        {
            var invoice = new Invoice
            {
                InvoiceDate = new DateTime(2019, 09, 27),
                InvoiceNumber = 123,
                LineItems = new List<InvoiceLine>
                {
                    new InvoiceLine
                    {
                        Quantity = 1,
                        Product = _apple
                    },
                    new InvoiceLine
                    {
                        Quantity = 3,
                        Product = _orange
                    }
                }
            };

            var clonedInvoice = _invoiceService.Clone(invoice);
            Assert.Equal(invoice.Id, clonedInvoice.Id);
            Assert.Equal(invoice.LineItems.Count, clonedInvoice.LineItems.Count);
            Assert.Equal(invoice.InvoiceDate, clonedInvoice.InvoiceDate);
            Assert.Equal(invoice.InvoiceNumber, clonedInvoice.InvoiceNumber);
            Assert.Contains(clonedInvoice.LineItems, _ =>
                _.Product.ProductCode.Contains("orange", StringComparison.InvariantCultureIgnoreCase) &&
                _.Quantity == 3);
        }

        [Fact]
        public void InvoiceToString()
        {
            var invoice = new Invoice
            {
                InvoiceDate = new DateTime(2019, 09, 27),
                InvoiceNumber = 1000,
                LineItems = new List<InvoiceLine>
                {
                    new InvoiceLine
                    {
                        Quantity = 1,
                        Product = _apple
                    }
                }
            };

            Assert.Equal("Invoice Number: 1000, InvoiceDate: 27/09/2019, LineItemCount: 1", invoice.ToString());
        }
    }
}