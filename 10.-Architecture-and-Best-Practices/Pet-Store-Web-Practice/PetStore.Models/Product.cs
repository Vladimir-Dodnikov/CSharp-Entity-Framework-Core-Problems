using PetStore.Comman;
using PetStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetStore.Models
{
    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MinLength(GlobalConstant.ProductNameMinLength)]
        public string Name { get; set; }

        [Required]
        public ProductType ProductType{ get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
