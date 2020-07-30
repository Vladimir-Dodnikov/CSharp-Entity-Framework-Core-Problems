using PetStore.ServiceModels.Products;
using PetStore.ServiceModels.Products.InputModels;
using PetStore.ServiceModels.Products.OutputModels;
using PetStore.ViewModels.Product.OutputModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Services.Interfaces
{
    public interface IProductService
    {
        void AddProduct(AddProductInputServiceModel model);

        ICollection<ListAllProductsByProductTypeServiceModel> ListAllProductsByProductTypes(string type);

        ICollection<ListAllProductsServiceModel> GetAll();

        ICollection<ListAllProductsByNameServiceModel> SearchByName(string searchStr, bool caseSensitive);

        bool RemoveById(string id);

        bool RemoveByName(string name);

        void EditProduct(string id, EditProductInputServiceModel model);

        ProductDetailsServiceModel GetById(string id);
    }
}
