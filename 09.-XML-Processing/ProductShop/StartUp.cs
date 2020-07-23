using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using XmlFacade;

namespace ProductShop
{
    public class StartUp
    {
        private static string directoryPath = "../../../ExportFiles";

        public static void Main(string[] args)
        {
            ProductShopContext db = new ProductShopContext();
            //Problem - 00
            //ResetDatabase(db);

            //Problem - 01
            //string inputXml = File.ReadAllText(@"../../../Datasets/users.xml");
            //string result = ImportUsers(db, inputXml);
            //Console.WriteLine(result);

            //Problem - 02
            //string inputXml = File.ReadAllText(@"../../../Datasets/products.xml");
            //string result = ImportProducts(db, inputXml);
            //Console.WriteLine(result);

            //Problem - 03
            //string inputXml = File.ReadAllText(@"../../../Datasets/categories.xml");
            //string result = ImportCategories(db, inputXml);
            //Console.WriteLine(result);

            //Problem - 04
            //string inputXml = File.ReadAllText(@"../../../Datasets/categories-products.xml");
            //string result = ImportCategoryProducts(db, inputXml);
            //Console.WriteLine(result);

            //Problem - 05
            //string xml = GetProductsInRange(db);
            //EnsureDirectoryPath(directoryPath);
            //File.WriteAllText(directoryPath + "/products-in-range.xml", xml);

            //Problem - 06
            //string xml = GetSoldProducts(db);
            //EnsureDirectoryPath(directoryPath);
            //File.WriteAllText(directoryPath + "/users-sold-products.xml", xml);

            //Problem - 07
            //string xml = GetCategoriesByProductsCount(db);
            //EnsureDirectoryPath(directoryPath);
            //File.WriteAllText(directoryPath + "/categories-by-products.xml", xml);

            //Problem - 08
            string xml = GetUsersWithProducts(db);
            EnsureDirectoryPath(directoryPath);
            File.WriteAllText(directoryPath + "/users-and-products.xml", xml);
        }

        private static void EnsureDirectoryPath(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        //Problem - 00
        private static void ResetDatabase(ProductShopContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database was successfully deleted.");

            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfully created.");
        }
        //Problem - 01
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            string rootElement = "Users";

            var usersDTO = XmlConverter.Deserializer<ImportUserDTO>(inputXml, rootElement);
            ;
            var users = usersDTO
                .Select(u => new User
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age
                })
                .ToList();

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";

            //var users = new List<User>();
            //foreach (var user in usersDTO)
            //{
            //    var newUser = new User
            //    {
            //        FirstName = user.FirstName,
            //        LastName = user.LastName,
            //        Age = user.Age
            //    };

            //    users.Add(newUser);
            //}
            //context.Users.AddRange(users);
            //context.SaveChanges();

            //return $"Successfully imported {users.Count}";
        }
        //Problem - 02
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            string rootElement = "Products";

            var productDTOs = XmlConverter.Deserializer<ImportProductDTO>(inputXml, rootElement);
            ;
            var products = productDTOs
                .Select(p => new Product
                {
                    Name = p.Name,
                    Price = p.Price,
                    SellerId = p.SellerId,
                    BuyerId = p.BuyerId
                })
                .ToList();

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }
        //Problem - 03
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            string rootElement = "Categories";

            var categoryDTOs = XmlConverter.Deserializer<ImportCategoryDTO>(inputXml, rootElement);
            ;
            var categories = categoryDTOs
                .Where(c => c.Name != null)
                .Select(c => new Category
                {
                    Name = c.Name
                })
                .ToList();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }
        //Problem - 04
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            string rootElement = "CategoryProducts";

            var categoryProductDTOs = XmlConverter.Deserializer<ImportCategoryProductDTO>(inputXml, rootElement);

            var categoriesCount = context.Categories.Count();
            var productsCount = context.Products.Count();

            var categoriesProducts = categoryProductDTOs
                .Where(x => x.CategoryId <= categoriesCount && x.ProductId <= productsCount)
                .Select(cp => new CategoryProduct
                {
                    CategoryId = cp.CategoryId,
                    ProductId = cp.ProductId
                })
                .ToList();

            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count}";
        }
        //Problem - 05
        public static string GetProductsInRange(ProductShopContext context)
        {
            string rootElement = "Products";

            var products = context
                .Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new ExportProductDTO
                {
                    Name = x.Name,
                    Price = x.Price,
                    Buyer = x.Buyer.FirstName + " " + x.Buyer.LastName,
                })
                .OrderBy(x => x.Price)
                .Take(10)
                .ToList();

            string xml = XmlConverter.Serialize(products, rootElement);

            return xml;
        }
        //Problem - 06
        public static string GetSoldProducts(ProductShopContext context)
        {
            const string rootElement = "Users";

            var users = context
              .Users
              .Where(x => x.ProductsSold.Any(y => y.Buyer != null))
              .Select(u => new ExportSoldProductDTO
              {
                  FirstName = u.FirstName,
                  LastName = u.LastName,
                  SoldProducts = u.ProductsSold
                      .Where(x => x.Buyer != null)
                      .Select(b => new ExportBuyerWirhSoldProductDTO
                      {
                          Name = b.Name,
                          Price = b.Price
                      }).ToArray()
              })
              .OrderBy(x => x.LastName)
              .ThenBy(x => x.FirstName)
              .Take(5)
              .ToList();

            string xml = XmlConverter.Serialize(users, rootElement);

            return xml;
        }
        //Problem - 07
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            const string rootElement = "Categories";
            var categories = context
                .Categories
                .Select(c => new ExportCategoryDTO
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .OrderByDescending(cp => cp.Count)
                .ThenBy(cp=>cp.TotalRevenue)
                .ToArray();

            string xml = XmlConverter.Serialize(categories, rootElement);

            return xml;
        }
        //Problem - 08
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            const string rootElement = "Users";

            var users = context
                .Users
                .AsEnumerable()
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .Select(u => new ExportUserDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new ExportProductCountDTO
                    {
                        Count = u.ProductsSold.Count(p => p.Buyer != null),
                        Products = u.ProductsSold
                            .Where(p => p.Buyer != null)
                            .Select(p => new ExportProductInfoDTO
                            {
                                Name = p.Name,
                                Price = p.Price
                            })
                            .OrderByDescending(p=>p.Price)
                            .ToArray()
                    }
                })
                .OrderByDescending(x => x.SoldProducts.Count)
                .Take(10)
                .ToArray();
            
            var xmlResult = new ExportUserCountDTO
            {
                Count = context.Users.Count(x=>x.ProductsSold.Any()),
                Users = users
            };

            string xml = XmlConverter.Serialize(xmlResult, rootElement);

            return xml;
        }
    }
}