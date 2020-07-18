using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext db = new ProductShopContext();
            //Problem - 01
            //ResetDatabase(db);

            ////Problem - 02
            //string inputJson = File.ReadAllText(@"../../../Datasets/users.json");
            //string result = ImportUsers(db, inputJson);
            //Console.WriteLine(result);

            //Problem - 03
            //string inputJson = File.ReadAllText(@"../../../Datasets/products.json");
            //string result = ImportProducts(db, inputJson);
            //Console.WriteLine(result);

            //Problem - 04
            string inputJson = File.ReadAllText(@"../../../Datasets/categories.json");
            string result = ImportCategories(db, inputJson);
            Console.WriteLine(result);





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
            //2 options - with DTO (AutoMapper)
            User[] users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
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

    }
}