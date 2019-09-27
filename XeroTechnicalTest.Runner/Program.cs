/*
    Welcome to the Xero technical excercise!
    ---------------------------------------------------------------------------------
    The test consists of a small invoice application that has a number of issues.

    Your job is to fix them and make sure you can perform the functions in each method below.

    Note your first job is to get the solution compiling! 
	
    Rules
    ---------------------------------------------------------------------------------
    * The entire solution must be written in C# (any version)
    * You can modify any of the code in this solution, split out classes, add projects etc
    * You can modify Invoice and InvoiceLine, rename and add methods, change property types (hint) 
    * Feel free to use any libraries or frameworks you like as long as they are .net based
    * Feel free to write tests (hint) 
    * Show off your skills! 

    Good luck :) 

    When you have finished the solution please zip it up and email it back to the recruiter or developer who sent it to you
*/

using System;
using Microsoft.Extensions.DependencyInjection;
using XeroTechnicalTest.Domain.Services.Invoice;
using XeroTechnicalTest.Domain.Services.Product;

namespace XeroTechnicalTest.Runner
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Xero Tech Test!");
            var runner = new Runner(new InvoiceService(), new ProductService());
            runner.CreateInvoiceWithOneItem();
            runner.CreateInvoiceWithMultipleItemsAndQuantities();
            runner.RemoveItem();
            runner.MergeInvoices();
            runner.CloneInvoice();
            runner.InvoiceToString();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IInvoiceService, InvoiceService>();
            return services.BuildServiceProvider();
        }
    }
}
