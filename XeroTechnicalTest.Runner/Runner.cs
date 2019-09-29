using System;
using System.Collections.Generic;
using System.Linq;
using XeroTechnicalTest.Domain.Constants;
using XeroTechnicalTest.Domain.Models;
using XeroTechnicalTest.Domain.Services.Invoice;
using XeroTechnicalTest.Domain.Services.Product;

namespace XeroTechnicalTest.Runner
{
    public class Runner
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IProductService _productService;

        /// <summary>
        /// I kept all the methods from the original Program.cs, although half of them seem more like things you'd have
        /// as unit tests or something (which I've also done).
        /// </summary>
        public Runner(
            IInvoiceService invoiceService,
            IProductService productService)
        {
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }
        
        public void CreateInvoiceWithOneItem()
        {
            var apple = _productService.Products()
                .OfType<ProductProduce>()
                .SingleOrDefault(_ => _.Code == Products.AppleProductCode);
            
            var invoice = _invoiceService.CreateInvoice(new List<InvoiceLine>
            {
                new InvoiceLine
                {
                    Quantity = 1,
                    Product = apple
                }
            });
            Console.WriteLine(invoice.TotalString);
        }

        public void CreateInvoiceWithMultipleItemsAndQuantities()
        {
            // todo: logging/handling null products
            var banana = _productService.Products()
                .OfType<ProductProduce>()
                .SingleOrDefault(_ => _.Code == Products.BananaProductCode);
            
            var orange = _productService.Products()
                .OfType<ProductProduce>()
                .SingleOrDefault(_ => _.Code == Products.OrangeProductCode);
            
            var milk = _productService.Products()
                .OfType<ProductDairy>()
                .SingleOrDefault(_ => _.Code == Products.MilkProductCode);
            
            var invoice = _invoiceService.CreateInvoice(new List<InvoiceLine>
            {
                new InvoiceLine
                {
                    Quantity = 4,
                    Product = banana
                },
                new InvoiceLine
                {
                    Quantity = 1,
                    Product = orange
                },
                new InvoiceLine
                {
                    Quantity = 5,
                    Product = milk
                }
            });
            Console.WriteLine(invoice.TotalString);
        }

        public void RemoveItem()
        {
            var banana = _productService.Products()
                .OfType<ProductProduce>()
                .SingleOrDefault(_ => _.Code == Products.BananaProductCode);
            
            var orange = _productService.Products()
                .OfType<ProductProduce>()
                .SingleOrDefault(_ => _.Code == Products.OrangeProductCode);
            
            var lines = new List<InvoiceLine>
            {
                new InvoiceLine
                {
                    Quantity = 1,
                    Product = orange
                },
                new InvoiceLine
                {
                    Quantity = 4,
                    Product = banana
                }
            };
            var invoice = _invoiceService.CreateInvoice(lines);

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
            Console.WriteLine(invoice.TotalString);
        }

        public void MergeInvoices()
        {
            var banana = _productService.Products()
                .OfType<ProductProduce>()
                .SingleOrDefault(_ => _.Code == Products.BananaProductCode);
            
            var orange = _productService.Products()
                .OfType<ProductProduce>()
                .SingleOrDefault(_ => _.Code == Products.OrangeProductCode);
            
            var milk = _productService.Products()
                .OfType<ProductDairy>()
                .SingleOrDefault(_ => _.Code == Products.MilkProductCode);
            
            var invoice1 = _invoiceService.CreateInvoice(new List<InvoiceLine>
            {
                new InvoiceLine
                {
                    Quantity = 4,
                    Product = banana
                }
            });

            var invoice2 = _invoiceService.CreateInvoice(new List<InvoiceLine>
            {
                new InvoiceLine
                {
                    Quantity = 1,
                    Product = orange
                },
                new InvoiceLine
                {
                    Quantity = 3,
                    Product = milk
                }
            });

            invoice1.MergeInvoices(invoice2);
            Console.WriteLine(invoice1.TotalString);
        }

        public void CloneInvoice()
        {
            var banana = _productService.Products()
                .OfType<ProductProduce>()
                .SingleOrDefault(_ => _.Code == Products.BananaProductCode);
            
            var orange = _productService.Products()
                .OfType<ProductProduce>()
                .SingleOrDefault(_ => _.Code == Products.OrangeProductCode);
            
            var invoice = _invoiceService.CreateInvoice(new List<InvoiceLine>
            {
                new InvoiceLine
                {
                    Quantity = 1,
                    Product = banana
                },
                new InvoiceLine
                {
                    Quantity = 3,
                    Product = orange
                }
            });

            var clonedInvoice = _invoiceService.Clone(invoice);
            Console.WriteLine(clonedInvoice.TotalString);
        }

        public void InvoiceAsFormattedString()
        {
            var orange = _productService.Products()
                .OfType<ProductProduce>()
                .SingleOrDefault(_ => _.Code == Products.OrangeProductCode);

            var invoice = new Invoice
            {
                InvoiceDate = DateTime.UtcNow,
                Code = "I10001",
                LineItems = new List<InvoiceLine>
                {
                    new InvoiceLine
                    {
                        Quantity = 1,
                        Product = orange
                    }
                }
            };

            Console.WriteLine(invoice.ToString());
        }
    }
}