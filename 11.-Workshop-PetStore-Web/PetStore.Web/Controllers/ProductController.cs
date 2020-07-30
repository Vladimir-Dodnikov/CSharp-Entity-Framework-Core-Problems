using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetShop.Services.Interfaces;
using PetStore.ServiceModels.Products;
using PetStore.ServiceModels.Products.OutputModels;
using PetStore.ViewModels;
using PetStore.ViewModels.Product.InputModels;
using PetStore.ViewModels.Product.OutputModels;

namespace PetStore.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;
        //Dependeency Injection
        public ProductController(IProductService productService, IMapper mapper)
        {
            //Db Container search for registration and returns new instance
            this.productService = productService;
            this.mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return this.RedirectToAction("All");
        }

        [HttpGet]
        public IActionResult All()
        {
            var allProducts = this.productService.GetAll();

            ICollection<ListAllProductsViewModel> viewProducts = this.mapper.Map<List<ListAllProductsViewModel>>(allProducts);

            return View(viewProducts);

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProductInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Error", "Home");
            }

            AddProductInputServiceModel serviceModel = this.mapper.Map<AddProductInputServiceModel>(model);

            this.productService.AddProduct(serviceModel);

            return this.RedirectToAction("All");
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            ProductDetailsServiceModel serviceModel = this.productService.GetById(id);

            ProductDetailsViewModel viewModel = this.mapper.Map<ProductDetailsViewModel>(serviceModel);

            return this.View(viewModel);
        }

        [HttpGet]
        public IActionResult Search(string searchWord)
        {
            //if (searchWord == null)
            //{
            //    return this.RedirectToAction("All");
            //}

            ICollection<ListAllProductsByNameServiceModel> serviceModels = this.productService.SearchByName(searchWord, false);

            ICollection<ListAllProductsViewModel> viewModels = this.mapper.Map<List<ListAllProductsViewModel>>(serviceModels);

            return this.View("All", viewModels);
        }
    }
}
