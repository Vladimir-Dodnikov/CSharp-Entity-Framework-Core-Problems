namespace PetStore.Models
{
    using PetStore.Comman;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Pet
    {
        public Pet()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        //GUID - string
        [Key]
        public string Id { get; set; }

        [Required, MinLength(GlobalConstant.PetNameMinLength)]
        public string Name { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [MinLength(GlobalConstant.MinimumPetAge), MaxLength(GlobalConstant.MaximumPetAge)]
        public byte Age { get; set; }

        [Required]
        public bool IsSold { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [ForeignKey(nameof(Breed))]
        public int BreedId { get; set; }
        public virtual Breed Breed { get; set; }


        [Required]
        [ForeignKey(nameof(Client))]
        public string ClientId { get; set; }
        public Client Client { get; set; }

    }
}
