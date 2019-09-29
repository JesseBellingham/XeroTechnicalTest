using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using XeroTechnicalTest.Domain.Constants;
using XeroTechnicalTest.Domain.Models;
using XeroTechnicalTest.Domain.Services.Invoice;
using XeroTechnicalTest.Tests.Fixtures;
using Xunit;

namespace XeroTechnicalTest.Tests.Services
{
    /// <summary>
    /// There may be merit in having more randomised data for quantities, I'm not sure.
    /// The functionality we're testing in these particular tests don't seem to need it.
    /// </summary>
    public class InvoiceServiceTests : IClassFixture<ProductsFixture>
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ProductsFixture _fixture;

        private const int AppleQuantity = 4;
        private const int MilkQuantity = 1;
        private const int OrangeQuantity = 5;
        private const int BananaQuantity = 3;

        public InvoiceServiceTests(ProductsFixture fixture)
        {
            var logger = Mock.Of<ILogger<InvoiceService>>();
            _invoiceService = new InvoiceService(logger);
            _fixture = fixture;
        }

        [Fact]
        public void InvoiceWithOneLineItemCalculatesCorrectTotal()
        {
            var invoice = _invoiceService.CreateInvoice(
                new List<InvoiceLine>
                {
                    new InvoiceLine
                    {
                        Product = _fixture.Apple,
                        Quantity = AppleQuantity
                    }
                });

            var expectedTotal = _fixture.Apple.CostInc * AppleQuantity;
            Assert.Equal(expectedTotal, invoice.TotalInc);
        }

        [Fact]
        public void CreateInvoiceWithMultipleItemsAndQuantities()
        {
            var lines = new List<InvoiceLine>
            {
                new InvoiceLine
                {
                    Product = _fixture.Apple,
                    Quantity = AppleQuantity,
                },
                new InvoiceLine
                {
                    Product = _fixture.Milk,
                    Quantity = MilkQuantity
                },
                new InvoiceLine
                {
                    Product = _fixture.Orange,
                    Quantity = OrangeQuantity
                }
            };

            var invoice = _invoiceService.CreateInvoice(lines);
            var expectedTotal = _fixture.Apple?.CostInc * AppleQuantity
                                + _fixture.Milk?.CostInc * MilkQuantity
                                + _fixture.Orange?.CostInc * OrangeQuantity;
            Assert.Equal(expectedTotal,invoice.TotalInc);
        }

        [Theory]
        [InlineData("57d6b1ad-e1f1-4e6d-a8de-86cec478ec06")]
        [InlineData("02ae29d8-74eb-4970-b773-2b6860155a07")]
        public void RemoveItemAtIndexRemovesCorrectItem(Guid id)
        {
            // this is failing, I suspect because the inline data type is a string, rather than a guid.
            // I am unable to supply a guid, because it requires a compile time constant
            var lines = new List<InvoiceLine>
            {
                new InvoiceLine
                {
                    Id = Guid.Parse("57d6b1ad-e1f1-4e6d-a8de-86cec478ec06"),
                    Quantity = OrangeQuantity,
                    Product = _fixture.Orange
                },
                new InvoiceLine
                {
                    Id = Guid.Parse("02ae29d8-74eb-4970-b773-2b6860155a07"),
                    Quantity = BananaQuantity,
                    Product = _fixture.Banana
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
                        Quantity = BananaQuantity,
                        Product = _fixture.Banana
                    }
                });

            var invoice2Lines = new List<InvoiceLine>
            {
                new InvoiceLine
                {
                    Quantity = OrangeQuantity,
                    Product = _fixture.Orange
                },
                new InvoiceLine
                {
                    Quantity = MilkQuantity,
                    Product = _fixture.Milk
                }
            };
            var invoice2 = _invoiceService.CreateInvoice(invoice2Lines);
            var expectedTotal = invoice1.LineItems.Sum(_ => _.Product.CostInc * _.Quantity) +
                                invoice2.LineItems.Sum(_ => _.Product.CostInc * _.Quantity);

            invoice1.MergeInvoices(invoice2);

            Assert.Equal(expectedTotal, invoice1.TotalInc);
        }

        [Fact]
        public void ClonedInvoiceCorrectlyDeepClonesTargetInvoice()
        {
            var invoice = new Invoice
            {
                InvoiceDate = new DateTime(2019, 09, 27),
                Code = "I10001",
                LineItems = new List<InvoiceLine>
                {
                    new InvoiceLine
                    {
                        Quantity = AppleQuantity,
                        Product = _fixture.Apple
                    },
                    new InvoiceLine
                    {
                        Quantity = OrangeQuantity,
                        Product = _fixture.Orange
                    }
                }
            };

            var clonedInvoice = _invoiceService.Clone(invoice);
            Assert.Equal(invoice.Id, clonedInvoice.Id);
            Assert.Equal(invoice.LineItems.Count, clonedInvoice.LineItems.Count);
            Assert.Equal(invoice.InvoiceDate, clonedInvoice.InvoiceDate);
            Assert.Equal(invoice.Code, clonedInvoice.Code);
            Assert.Contains(clonedInvoice.LineItems, _ =>
                _.Product.Code.Equals(Products.OrangeProductCode) &&
                _.Quantity == OrangeQuantity);
            Assert.Contains(clonedInvoice.LineItems, _ =>
                _.Product.Code.Equals(Products.AppleProductCode) &&
                _.Quantity == AppleQuantity);
        }

        [Fact]
        public void InvoiceToStringOutputsExpectedFormattedString()
        {
            var invoice = new Invoice
            {
                InvoiceDate = new DateTime(2019, 09, 27),
                Code = "I10000",
                LineItems = new List<InvoiceLine>
                {
                    new InvoiceLine
                    {
                        Quantity = AppleQuantity,
                        Product = _fixture.Apple
                    }
                }
            };

            Assert.Equal($"Invoice Number: {invoice.Code}, InvoiceDate: {invoice.InvoiceDate:dd/MM/yyyy}, LineItemCount: {invoice.LineItems.Count}",
                invoice.ToString());
        }
    }
}