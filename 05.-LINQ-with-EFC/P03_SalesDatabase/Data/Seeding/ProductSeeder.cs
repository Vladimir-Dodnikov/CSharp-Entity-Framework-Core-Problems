using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.IOManagement.Contracts;
using P03_SalesDatabase.Data.Models;
using P03_SalesDatabase.Data.Seeding.Contracts;
using System;
using System.Collections.Generic;

namespace P03_SalesDatabase.Data.Seeding
{
    public class ProductSeeder : ISeeder
    {
        private readonly Random random;
        private readonly SalesContext dbContext;

        private readonly IWriter writer;

        public ProductSeeder(SalesContext context, Random random, IWriter writer)
        {
            this.dbContext = context;
            this.random = random;
            this.writer = writer;   
        }
        public void Seed()
        {
            ICollection<Product> products = new List<Product>();

            string[] names = new string[]
            {
                "CPU",
                "Motherboard",
                "GPU",
                "RAM",
                "SSD",
                "HDD",
                "CD-RW",
                "Air Cooler",
                "Water Cooler",
                "Termopaste"
            };

            for (int i = 0; i < 50; i++)
            {
                int nameIndex = this.random.Next(names.Length);
                string currPrName = names[nameIndex];

                double quantity = this.random.Next(1000);
                decimal price = this.random.Next(5000)*1.133m;

                Product product = new Product()
                {
                    Name = currPrName,
                    Price = price,
                    Quantity = quantity
                };

                products.Add(product);

                this.writer.WriteLine($"Product - Name: {currPrName}, {quantity}, {price} was added to Database.");
            }

            this.dbContext.Products.AddRange(products);

            this.dbContext.SaveChanges();
        }
    }
}
