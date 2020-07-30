using PetStore.Comman;
using System.ComponentModel.DataAnnotations;

namespace PetStore.ViewModels.Product.InputModels
{
    public class CreateProductInputModel
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
