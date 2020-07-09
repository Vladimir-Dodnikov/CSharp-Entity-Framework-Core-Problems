﻿using P03_SalesDatabase.Data.IOManagement.Contracts;
using P03_SalesDatabase.Data.Models;
using P03_SalesDatabase.Data.Seeding.Contracts;
using System.Linq;

namespace P03_SalesDatabase.Data.Seeding
{
    public class StoreSeeder : ISeeder
    {
        private readonly SalesContext dbContext;

        private readonly IWriter writer;

        public StoreSeeder(SalesContext context, IWriter writer)
        {
            this.dbContext = context;
            this.writer = writer;   
        }

        public void Seed()
        {
            Store[] stores = new Store[]
            {
                new Store() {Name = "PcTech Sofia"},
                new Store() {Name = "PcTech Plodiv"},
                new Store() {Name = "PcTech Varna"},
                new Store() {Name =  "InnovativeTech Sofia"},
                new Store() {Name = "InnovativeTech Plovdiv"}
            };

            this.writer.WriteLine($"{stores.Length} were added to the Database.");

            this.dbContext.Stores.AddRange(stores);
            this.dbContext.SaveChanges();
        }
    }
}
