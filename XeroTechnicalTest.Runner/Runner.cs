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

        public Runner(
            IInvoiceService invoiceService,
            IProductService productService)
        {
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }
        
        public void CreateInvoiceWithOneItem()
        {
            var apple = _productService.GetProductsOfType(ProductType.Produce)
                .SingleOrDefault(
                    _ => _.ProductCode.Contains("apple", StringComparison.InvariantCultureIgnoreCase));
            
            var lines = new List<InvoiceLine>
            {
                new InvoiceLine
                {
                    Quantity = 1,
                    Product = apple
                }
            };
            var invoice = _invoiceService.CreateInvoice(lines);

            Console.WriteLine(invoice.Total);
        }

        public void CreateInvoiceWithMultipleItemsAndQuantities()
        {
            var banana = _productService.GetProductsOfType(ProductType.Produce)
                .SingleOrDefault(
                    _ => _.ProductCode.Contains("banana", StringComparison.InvariantCultureIgnoreCase));
            
            var orange = _productService.GetProductsOfType(ProductType.Produce)
                .SingleOrDefault(
                    _ => _.ProductCode.Contains("orange", StringComparison.InvariantCultureIgnoreCase));
            
            var milk = _productService.GetProductsOfType(ProductType.Dairy)
                .SingleOrDefault(
                    _ => _.ProductCode.Contains("milk", StringComparison.InvariantCultureIgnoreCase));
            
            var lines = new List<InvoiceLine>
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
            };
            var invoice = _invoiceService.CreateInvoice(lines);

            Console.WriteLine(invoice.Total);
        }

        public void RemoveItem()
        {
            var banana = _productService.GetProductsOfType(ProductType.Produce)
                .SingleOrDefault(
                    _ => _.ProductCode.Contains("banana", StringComparison.InvariantCultureIgnoreCase));
            
            var orange = _productService.GetProductsOfType(ProductType.Produce)
                .SingleOrDefault(
                    _ => _.ProductCode.Contains("orange", StringComparison.InvariantCultureIgnoreCase));
            
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
            Console.WriteLine(invoice.Total);
        }

        public void MergeInvoices()
        {
            var banana = _productService.GetProductsOfType(ProductType.Produce)
                .SingleOrDefault(
                    _ => _.ProductCode.Contains("banana", StringComparison.InvariantCultureIgnoreCase));
            
            var orange = _productService.GetProductsOfType(ProductType.Produce)
                .SingleOrDefault(
                    _ => _.ProductCode.Contains("orange", StringComparison.InvariantCultureIgnoreCase));
            
            var milk = _productService.GetProductsOfType(ProductType.Dairy)
                .SingleOrDefault(
                    _ => _.ProductCode.Contains("milk", StringComparison.InvariantCultureIgnoreCase));
            
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
            Console.WriteLine(invoice1.Total);
        }

        public void CloneInvoice()
        {
            var banana = _productService.GetProductsOfType(ProductType.Produce)
                .SingleOrDefault(
                    _ => _.ProductCode.Contains("banana", StringComparison.InvariantCultureIgnoreCase));
            
            var orange = _productService.GetProductsOfType(ProductType.Produce)
                .SingleOrDefault(
                    _ => _.ProductCode.Contains("orange", StringComparison.InvariantCultureIgnoreCase));
            
            var milk = _productService.GetProductsOfType(ProductType.Dairy)
                .SingleOrDefault(
                    _ => _.ProductCode.Contains("milk", StringComparison.InvariantCultureIgnoreCase));
            
            var lines = new List<InvoiceLine>
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
            };
            var invoice = _invoiceService.CreateInvoice(lines);

            var clonedInvoice = _invoiceService.Clone(invoice);
            Console.WriteLine(clonedInvoice.Total);
        }

        public void InvoiceToString()
        {

            var orange = _productService.GetProductsOfType(ProductType.Produce)
                .SingleOrDefault(
                    _ => _.ProductCode.Contains("orange", StringComparison.InvariantCultureIgnoreCase));

            var invoice = new Invoice
            {
                InvoiceDate = DateTime.Now,
                InvoiceNumber = 1000,
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