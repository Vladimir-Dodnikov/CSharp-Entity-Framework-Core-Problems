using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Data.SqlClient.Server;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTO.Product;
using ProductShop.DTO.Users;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static string directoryPath = "../../../ExportFiles";
        public static void Main(string[] args)
        {
            ProductShopContext db = new ProductShopContext();
            //Problem - 01
            //ResetDatabase(db);

            //Problem - 02
            //string inputJson = File.ReadAllText(@"../../../Datasets/users.json");
            //string result = ImportUsers(db, inputJson);
            //Console.WriteLine(result);

            //Problem - 02 with AutoMapper and DTO
            //string inputJson = File.ReadAllText(@"../../../Datasets/users.json");
            //InitializeMapper();
            //string result = ImportUsers(db, inputJson);
            //Console.WriteLine(result);

            //Problem - 03
            //string inputJson = File.ReadAllText(@"../../../Datasets/products.json");
            //string result = ImportProducts(db, inputJson);
            //Console.WriteLine(result);

            //Problem - 04
            //string inputJson = File.ReadAllText(@"../../../Datasets/categories.json");
            //string result = ImportCategories(db, inputJson);
            //Console.WriteLine(result);

            //Problem - 05
            //string inputJson = File.ReadAllText(@"../../../Datasets/categories-products.json");
            //string result = ImportCategoryProducts(db, inputJson);
            //Console.WriteLine(result);

            //Problem - 06
            //string json = GetProductsInRange(db);
            //EnsureDirectoryPath(directoryPath);
            //File.WriteAllText(directoryPath + "/products-in-range.json", json);

            //Problem - 06 - with static AutoMapper and DTO
            //InitializeMapper();
            //string json = GetProductsInRange(db);
            //EnsureDirectoryPath(directoryPath);
            //File.WriteAllText(directoryPath + "/products-in-rangeDTO.json", json);

            //Problem - 07
            //string json = GetSoldProducts(db);
            //EnsureDirectoryPath(directoryPath);
            //File.WriteAllText(directoryPath + "/users-sold-products.json", json);

            //Problem - 07 - with static AutoMapper and DTO
            //InitializeMapper();
            //string json = GetSoldProducts(db);
            //EnsureDirectoryPath(directoryPath);
            //File.WriteAllText(directoryPath + "/users-sold-productsDTO.json", json);

            //Problem - 08
            //string json = GetCategoriesByProductsCount(db);
            //EnsureDirectoryPath(directoryPath);
            //File.WriteAllText(directoryPath + "/categories-by-products.json", json);

            //Problem - 09
            //string json = GetUsersWithProducts(db);
            //EnsureDirectoryPath(directoryPath);
            //File.WriteAllText(directoryPath + "/users-and-products.json", json);
        }

        //Option with static Mapper !
        private static void EnsureDirectoryPath(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());
        }

        private static void ResetDatabase(ProductShopContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database was successfully deleted.");

            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfully created.");
        }
        //Problem 02
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            User[] users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";

            //With DTO
            //UserImportDTO[] usersDTO = JsonConvert.DeserializeObject<UserImportDTO[]>(inputJson);
            //User[] users = usersDTO.Select(udto => Mapper.Map<User>(udto)).ToArray();
            //context.Users.AddRange(users);
            //context.SaveChanges();
            //return $"Successfully imported {users.Length}";
        }
        //Problem - 03
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            Product[] products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }
        //Problem - 04
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            
            Category[] categories = JsonConvert
                .DeserializeObject<Category[]>(inputJson)
                .Where(c => c.Name != null)
                .ToArray();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
            //JsonSerializerSettings settings = new JsonSerializerSettings()
            //{
            //    NullValueHandling = NullValueHandling.Ignore
            //};
            //Category[] categories = JsonConvert.DeserializeObject<Category[]>(inputJson, settings);

            //context.Categories.AddRange(categories);
            //context.SaveChanges();

            //return $"Successfully imported {categories.Length}";
        }
        //Problem - 05
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            CategoryProduct[] categoriesProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Length}";
        }
        //Problem - 06
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context
                .Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .OrderBy(x => x.price)
                .ToList();

            string json = JsonConvert.SerializeObject(products, Formatting.Indented);

            return json;

            //Option - DTO
            //var products = context
            //    .Products
            //    .Where(p => p.Price >= 500 && p.Price <= 1000)
            //    .OrderBy(p => p.Price)
            //    .Select(x => new ListProductsInRangeDTO
            //    {
            //        Name = x.Name,
            //        Price = x.Price.ToString("f2"),
            //        Seller = x.Seller.FirstName + " " + x.Seller.LastName
            //    })
            //    .ToArray();

            //string json = JsonConvert.SerializeObject(products, Formatting.Indented);

            //return json;

            //Option - AUTOMAPPER AND DTO
            //var products = context
            //    .Products
            //    .Where(p => p.Price >= 500 && p.Price <= 1000)
            //    .OrderBy(p => p.Price)
            //    .ProjectTo<ListProductsInRangeDTO>()
            //    .ToArray();

            //string json = JsonConvert.SerializeObject(products, Formatting.Indented);

            //return json;
        }
        //Problem - 07
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context
               .Users
               .Where(x => x.ProductsSold.Any(y => y.Buyer != null))
               .Select(u => new
               {
                   firstName = u.FirstName,
                   lastName = u.LastName,
                   soldProducts = u.ProductsSold
                       .Where(x => x.Buyer != null)
                       .Select(b => new
                       {
                           name = b.Name,
                           price = b.Price,
                           buyerFirstName = b.Buyer.FirstName,
                           buyerLastName = b.Buyer.LastName
                       })
               })
               .OrderBy(x => x.lastName)
               .ThenBy(x => x.firstName)
               .ToList();

            string json = JsonConvert.SerializeObject(users, Formatting.Indented);

            return json;
            //var users = context
            //   .Users
            //   .Where(u => u.ProductsSold.Any(b => b.Buyer != null))
            //   .OrderBy(u => u.LastName)
            //   .ThenBy(u => u.FirstName)
            //   .ProjectTo<UserWithSoldProductsDTO>()
            //   .ToArray();
            //string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            //return json;
        }
        //Problem - 08
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context
                .Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count,
                    averagePrice = c.CategoryProducts.Average(cp => cp.Product.Price).ToString("f2"),
                    totalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price).ToString("f2")
                })
                .OrderByDescending(cp => cp.productsCount)
                .ToArray();

            string json = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return json;
        }
        //Problem - 09
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context
                .Users
                .AsEnumerable()
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Count(p => p.Buyer != null),
                        products = u.ProductsSold
                            .Where(p => p.Buyer != null)
                            .Select(p => new
                            {
                                name = p.Name,
                                price = p.Price
                            })
                            .ToArray()
                    }
                })
                .OrderByDescending(x => x.soldProducts.count)
                .ToArray();

            var resultObj = new
            {
                usersCount = users.Length,
                users = users
            };

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(resultObj, settings);

            return json;
        }
    }
}