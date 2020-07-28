using PetStore.Comman;
using PetStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetStore.ServiceModels.Products
{
    //Like DTO
    public class AddProductInputServiceModel
    {
        [Required]
        [MinLength(GlobalConstant.ProductNameMinLength)]
        public string Name { get; set; }

        [Required]
        public string ProductType { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
