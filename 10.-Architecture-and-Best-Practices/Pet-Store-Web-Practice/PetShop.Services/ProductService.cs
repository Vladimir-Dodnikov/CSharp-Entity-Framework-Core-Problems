using AutoMapper;
using AutoMapper.QueryableExtensions;
using PetShop.Services.Interfaces;
using PetStore.Comman;
using PetStore.Data;
using PetStore.Models;
using PetStore.Models.Enums;
using PetStore.ServiceModels.Products;
using PetStore.ServiceModels.Products.InputModels;
using PetStore.ServiceModels.Products.OutputModels;

using System;
using System.Collections.Generic;
using System.Linq;

namespace PetShop.Services
{
    public class ProductService : IProductService
    {
        //Initiliaze by ASP.NET Core
        private readonly PetStoreDbContext dbContext;
        private readonly IMapper mapper;

        public ProductService(PetStoreDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        public void AddProduct(AddProductInputServiceModel model)
        {
            try
            {
                Product product = this.mapper.Map<Product>(model);

                this.dbContext.Products.Add(product);
                this.dbContext.SaveChanges();
            }
            catch (Exception)
            {

                throw new ArgumentException(ExceptionMessages.InvalidProductType);
            }
        }

        public ICollection<ListAllProductsByProductTypeServiceModel> ListAllProductsByProductTypes(string type)
        {
            ProductType productType;

            bool hasParsed = Enum.TryParse<ProductType>(type, true, out productType);

            if (!hasParsed)
            {
                throw new ArgumentException(ExceptionMessages.InvalidProductType);
            }

            var productsServiceModel = this.dbContext.Products
                .Where(x => x.ProductType == productType)
                .ProjectTo<ListAllProductsByProductTypeServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

            return productsServiceModel;
        }

        public ICollection<ListAllProductsServiceModel> GetAll(string type)
        {
            var products = this.dbContext.Products
                .ProjectTo<ListAllProductsServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

            return products;

        }

        public bool RemoveById(string id)
        {
            Product productToRemove = this.dbContext.Products.Find(id);
            if (productToRemove == null)
            {
                throw new ArgumentException(ExceptionMessages.ProductNotFound);
            }

            this.dbContext.Products.Remove(productToRemove);

            var affectedRows = this.dbContext.SaveChanges();

            bool wasDeleted = affectedRows == 1;

            return wasDeleted;
        }

        public bool RemoveByName(string name)
        {
            Product productToRemove = this.dbContext.Products
                .FirstOrDefault(x=>x.Name == name);

            if (productToRemove == null)
            {
                throw new ArgumentException(ExceptionMessages.ProductNotFound);
            }

            this.dbContext.Products.Remove(productToRemove);

            var affectedRows = this.dbContext.SaveChanges();

            bool wasDeleted = affectedRows == 1;

            return wasDeleted;
        }

        public ICollection<ListAllProductsByNameServiceModel> SearchByName(string searchStr, bool caseSensitive)
        {
            ICollection<ListAllProductsByNameServiceModel> products;

            if (caseSensitive)
            {
                products = this.dbContext.Products.Where(x => x.Name.Contains(searchStr))
                    .ProjectTo<ListAllProductsByNameServiceModel>(this.mapper.ConfigurationProvider)
                    .ToList(); 
            }
            else
            {
                products = this.dbContext.Products.Where(x => x.Name.ToLower().Contains(searchStr.ToLower()))
                   .ProjectTo<ListAllProductsByNameServiceModel>(this.mapper.ConfigurationProvider)
                   .ToList();
            }

            return products;
        }

        public void EditProduct(string id, EditProductInputServiceModel model)
        {
            try
            {
                Product product = this.mapper.Map<Product>(model);

                Product productToUpdate = this.dbContext.Products.Find(id);

                if (productToUpdate == null)
                {
                    throw new ArgumentException(ExceptionMessages.ProductNotFound);
                }

                productToUpdate.Name = product.Name;
                productToUpdate.ProductType = product.ProductType;
                productToUpdate.Price = product.Price;

                this.dbContext.SaveChanges();

            }
            catch (ArgumentException ae)
            {
                throw ae;
            }
            catch (Exception)
            {

                throw new ArgumentException(ExceptionMessages.InvalidProductType);
            }
        }
    }
}
