using CarDealer.Data;
using CarDealer.Datasets;
using CarDealer.DTO;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XmlFacade;

namespace CarDealer
{
    public class StartUp
    {
        private const string directoryPath = "../../../Results";
        public static void Main(string[] args)
        {
            CarDealerContext db = new CarDealerContext();
            //Problem - 00
            //ResetDatabase(db);

            //Problem - 09
            //string inputXml = File.ReadAllText(@"../../../Datasets/suppliers.xml");
            //string result = ImportSuppliers(db, inputXml);
            //Console.WriteLine(result);

            //Problem - 10
            //string inputXml = File.ReadAllText(@"../../../Datasets/parts.xml");
            //string result = ImportParts(db, inputXml);
            //Console.WriteLine(result);

            //Problem - 11
            //string inputXml = File.ReadAllText(@"../../../Datasets/cars.xml");
            //string result = ImportCars(db, inputXml);
            //Console.WriteLine(result);

            //Problem - 12
            //string inputXml = File.ReadAllText(@"../../../Datasets/customers.xml");
            //string result = ImportCustomers(db, inputXml);
            //Console.WriteLine(result);

            //Problem - 13
            //string inputXml = File.ReadAllText(@"../../../Datasets/sales.xml");
            //string result = ImportSales(db, inputXml);
            //Console.WriteLine(result);

            //Problem - 14
            //EnsureDirectoryPath(directoryPath);
            //string xml = GetCarsWithDistance(db);
            //File.WriteAllText(directoryPath + "/cars.xml", xml);

            //Problem - 15
            //EnsureDirectoryPath(directoryPath);
            //string xml = GetCarsFromMakeBmw(db);
            //File.WriteAllText(directoryPath + "/bmw-cars.xml", xml);

            //Problem - 16
            //EnsureDirectoryPath(directoryPath);
            //string xml = GetLocalSuppliers(db);
            //File.WriteAllText(directoryPath + "/local-suppliers.xml", xml);

            //Problem - 17
            //EnsureDirectoryPath(directoryPath);
            //string xml = GetCarsWithTheirListOfParts(db);
            //File.WriteAllText(directoryPath + "/cars-and-parts.xml", xml);

            //Problem - 18
            //EnsureDirectoryPath(directoryPath);
            //string xml = GetTotalSalesByCustomer(db);
            //File.WriteAllText(directoryPath + "/customers-total-sales.xml", xml);

            //Problem - 19
            EnsureDirectoryPath(directoryPath);
            string xml = GetSalesWithAppliedDiscount(db);
            File.WriteAllText(directoryPath + "/sales-discounts.xml", xml);
        }
        private static void EnsureDirectoryPath(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        //Problem - 00
        private static void ResetDatabase(CarDealerContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database was successfully deleted.");

            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfully created.");
        }
        //Problem - 09
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            const string rootElement = "Suppliers";
            var suppliersDTOs = XmlConverter.Deserializer<ImportSupplierDTO>(inputXml, rootElement);

            var suppliers = suppliersDTOs
                .Select(s => new Supplier
                {
                    Name = s.Name,
                    IsImporter = s.IsImporter
                })
                .ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }
        //Problem - 10
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            const string rootElement = "Parts";

            int supplierCount = context.Suppliers.Count();

            ImportPartDTO[] partsDTOs = XmlConverter.Deserializer<ImportPartDTO>(inputXml, rootElement);

            var parts = partsDTOs
                .Where(p => p.SupplierId <= supplierCount)
                .Select(p => new Part
                {
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    SupplierId = p.SupplierId
                })
                .ToArray();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}";
        }
        ////Problem - 11
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            const string rootElement = "Cars";
            ImportCarDTO[] carsDTOs = XmlConverter.Deserializer<ImportCarDTO>(inputXml, rootElement);

            List<Car> newCars = new List<Car>();
            List<PartCar> newParts = new List<PartCar>();
            foreach (var car in carsDTOs)
            {
                Car newCar = new Car()
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TraveledDistance,

                };

                newCars.Add(newCar);

                foreach (var part in car.CarParts.Select(x => new { partId = x.PartId }).Distinct())
                {
                    PartCar newPart = new PartCar()
                    {
                        PartId = part.partId,
                        Car = newCar
                    };

                    newParts.Add(newPart);
                }
            }

            context.Cars.AddRange(newCars);
            context.PartCars.AddRange(newParts);
            context.SaveChanges();

            return $"Successfully imported {carsDTOs.Count()}";
        }
        //Problem - 12
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            const string rootElement = "Customers";
            ImportCustomerDTO[] customerDTOs = XmlConverter.Deserializer<ImportCustomerDTO>(inputXml, rootElement).ToArray();

            List<Customer> customers = new List<Customer>();
            foreach (var customerDto in customerDTOs)
            {
                var customer = new Customer
                {
                    Name = customerDto.Name,
                    BirthDate = customerDto.BirthDate,
                    IsYoungDriver = customerDto.IsYoungDriver
                };
                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }
        //Problem - 13
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            const string rootElement = "Sales";
            ImportSaleDTO[] saleDTOs = XmlConverter.Deserializer<ImportSaleDTO>(inputXml, rootElement).ToArray();

            var carsCount = context.Cars.Count();
            List<Sale> sales = new List<Sale>();
            foreach (var saleDto in saleDTOs)
            {
                if (saleDto.CarId <= carsCount)
                {
                    var sale = new Sale
                    {
                        CarId = saleDto.CarId,
                        CustomerId = saleDto.CustomerId,
                        Discount = saleDto.Discount
                    };

                    sales.Add(sale);
                }
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }
        //Problem - 14
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            const string rootElement = "cars";
            var cars = context
                .Cars
                .Where(c=>c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Select(c => new ExportCarDTO
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .Take(10)
                .ToList();

            string xml = XmlConverter.Serialize(cars, rootElement);

            return xml;
        }
        //Problem - 15
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            const string rootElement = "cars";
            var cars = context
                .Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new ExportCarWithMakeDTO
                {
                    TravelledDistance = c.TravelledDistance,
                    Model = c.Model,
                    Id = c.Id
                })
                .ToList();

            var xml = XmlConverter.Serialize(cars, rootElement);

            return xml;
        }
        //Problem - 16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            const string rootElement = "suppliers";
            var localSuppliers = context
                .Suppliers
                .Where(p => p.IsImporter == false)
                .Select(p => new ExportLocalSupplierDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    PartsCount = p.Parts.Count()
                })
                .ToList();

            var xml = XmlConverter.Serialize(localSuppliers, rootElement);

            return xml;
        }
        //Problem - 17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            const string rootElement = "cars";
            var carsAndParts = context
                .Cars
                .Select(c => new ExportCarWithListOfParts
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    CarParts = c.PartCars.Select(p => new ExportPartsOfCar
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                    .OrderByDescending(p => p.Price)
                    .ToList()
                })
                .OrderByDescending(x => x.TravelledDistance)
                .ThenBy(x=>x.Model)
                .Take(5)
                .ToList();

            var xml = XmlConverter.Serialize(carsAndParts, rootElement);

            return xml;
        }
        //Problem - 18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            const string rootElement = "customers";
            var customers = context
                .Sales
                .Where(c => c.Customer.Sales.Count()>0)
                .Select(c => new ExportCustomerWithCarsDTO
                {
                    FullName = c.Customer.Name,
                    BoughtCars = c.Customer.Sales.Count(),
                    SpentMoney = c.Car.PartCars.Sum(p=>p.Part.Price)
                })
                .OrderByDescending(m => m.SpentMoney)
                .ThenByDescending(c => c.BoughtCars)
                .ToList();

            var xml = XmlConverter.Serialize(customers, rootElement);

            return xml;
        }
        //Problem - 19
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            const string rootElement = "sales";
            var sales = context
                .Sales
                .Select(s => new ExportSaleDTO
                {
                    Car = new ExportCarWithAttribute
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    Name = s.Customer.Name,
                    Discount = s.Discount,
                    Price = s.Car.PartCars.Sum(p => p.Part.Price),
                    PriceWithDiscount = (s.Car.PartCars.Sum(p => p.Part.Price) - s.Car.PartCars.Sum(p => p.Part.Price) * s.Discount / 100.0M)
                })
                .ToList();

            var json = XmlConverter.Serialize(sales, rootElement);

            return json;
        }
    }
}