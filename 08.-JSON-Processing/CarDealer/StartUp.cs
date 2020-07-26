using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        private static string directoryPath = "../../../ExportFiles";

        public static void Main(string[] args)
        {
            CarDealerContext db = new CarDealerContext();
            //Problem - 01
            //ResetDatabase(db);

            //Problem - 10
            //string inputJson = File.ReadAllText(@"../../../Datasets/suppliers.json");
            //string result = ImportSuppliers(db, inputJson);
            //Console.WriteLine(result);

            //Problem - 11
            //string inputJson = File.ReadAllText(@"../../../Datasets/parts.json");
            //string result = ImportParts(db, inputJson);
            //Console.WriteLine(result);

            //Problem - 12
            //string inputJson = File.ReadAllText(@"../../../Datasets/cars.json");
            //string result = ImportCars(db, inputJson);
            //Console.WriteLine(result);

            //Problem - 13
            //string inputJson = File.ReadAllText(@"../../../Datasets/customers.json");
            //string result = ImportCustomers(db, inputJson);
            //Console.WriteLine(result);

            //Problem - 14
            //string inputJson = File.ReadAllText(@"../../../Datasets/sales.json");
            //string result = ImportSales(db, inputJson);
            //Console.WriteLine(result);

            //Problem - 15
            //EnsureDirectoryPath(directoryPath);
            //string json = GetOrderedCustomers(db);
            //File.WriteAllText(directoryPath + "/ordered-customers.json", json);

            //Problem - 16
            //EnsureDirectoryPath(directoryPath);
            //string json = GetCarsFromMakeToyota(db);
            //File.WriteAllText(directoryPath + "/toyota-cars.json", json);

            //Problem - 17
            EnsureDirectoryPath(directoryPath);
            string json = GetLocalSuppliers(db);
            File.WriteAllText(directoryPath + "/local-suppliers.json", json);

            //Problem - 18
            //EnsureDirectoryPath(directoryPath);
            //string json = GetCarsWithTheirListOfParts(db);
            //File.WriteAllText(directoryPath + "/cars-and-parts.json", json);

            //Problem - 19
            //EnsureDirectoryPath(directoryPath);
            //string json = GetTotalSalesByCustomer(db);
            //File.WriteAllText(directoryPath + "/customers-total-sales.json", json);

            //Problem - 20
            //EnsureDirectoryPath(directoryPath);
            //string json = GetSalesWithAppliedDiscount(db);
            //File.WriteAllText(directoryPath + "/sales-discounts.json", json);

        }
        private static void EnsureDirectoryPath(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        //Problem - 01
        private static void ResetDatabase(CarDealerContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database was successfully deleted.");

            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfully created.");
        }
        //Problem - 10
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            Supplier[] suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}.";
        }
        //Problem - 11
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            int supplierCount = context.Suppliers.Count();

            Part[] parts = JsonConvert.DeserializeObject<Part[]>(inputJson).Where(p => p.SupplierId <= supplierCount).ToArray();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}.";
        }
        //Problem - 12
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            ImportCarDTO[] cars = JsonConvert.DeserializeObject<ImportCarDTO[]>(inputJson);

            List<Car> newCars = new List<Car>();
            List<PartCar> newParts = new List<PartCar>();
            foreach (var car in cars)
            {
                Car newCar = new Car()
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance,

                };

                newCars.Add(newCar);

                foreach (var part in car.PartsId.Distinct())
                {
                    PartCar newPart = new PartCar()
                    {
                        PartId = part,
                        Car = newCar
                    };

                    newParts.Add(newPart);
                }
            }

            context.Cars.AddRange(newCars);
            context.PartCars.AddRange(newParts);
            context.SaveChanges();

            return $"Successfully imported {cars.Count()}.";
        }
        //Problem - 13
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            Customer[] customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}.";
        }
        //Problem - 14
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            Sale[] sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}.";
        }
        //Problem - 15
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context
                .Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToList();

            string json = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return json;
        }
        //Problem - 16
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TravelledDistance
                })
                .ToList();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return json;
        }
        //Problem - 17
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context
                .Parts
                .Where(p => p.Supplier.IsImporter == false)
                .OrderBy(p => p.Supplier.Name)
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.Supplier.Name,
                    PartsCount = p.Supplier.Parts.Count()
                })
                .ToList();

            var json = JsonConvert.SerializeObject(localSuppliers, Formatting.Indented);

            return json;
        }
        //Problem - 18
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsAndParts = context
                .Cars
                .Select(c => new
                {
                    car = new ExportCarDTO
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance
                    },
                    parts = c.PartCars.Select(p => new ExportCarPartDTO
                    {
                        Name = p.Part.Name,
                        Price = $"{p.Part.Price:f2}"
                    })
                })
                .ToArray();

            var json = JsonConvert.SerializeObject(carsAndParts, Formatting.Indented);

            return json;
        }
        //Problem - 19
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context
                .Customers
                .Where(c => c.Sales.Count() > 0)
                .Select(c => new ExportCustomerWithCarsDTO
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count(),
                    SpentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(x=>x.Part.Price))
                })
                .OrderByDescending(m => m.SpentMoney)
                .ThenByDescending(c => c.BoughtCars)
                .ToList();

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return json;
        }
        //Problem - 20
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context
                .Sales
                .Take(10)
                .Select(s => new
                {
                    car = new ExportCarDTO
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    customerName = s.Customer.Name,
                    Discount = $"{s.Discount:F2}",
                    price = $"{s.Car.PartCars.Sum(p => p.Part.Price):F2}",
                    priceWithDiscount = $@"{(s.Car.PartCars.Sum(p => p.Part.Price) * (1.0M - s.Discount / 100.0M)):F2}"
                })
                .ToList();

            var json = JsonConvert.SerializeObject(sales, Formatting.Indented);

            return json;
        }
    }
}