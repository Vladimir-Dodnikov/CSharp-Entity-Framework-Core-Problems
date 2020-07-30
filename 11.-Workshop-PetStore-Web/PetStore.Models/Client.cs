using PetStore.Comman;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models
{
    public class Client
    {
        public Client()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Pets = new HashSet<Pet>();
        }
        [Key]
        public string Id { get; set; }

        [Required]
        [MinLength(GlobalConstant.ClientUsernameMinLength)]
        public string Username { get; set; }

        [Required]
        [MinLength(GlobalConstant.ClientPasswordMinLength), MaxLength(GlobalConstant.ClientPasswordMaxLength)]
        public string Password { get; set; }

        [Required]
        [MaxLength(GlobalConstant.ClientEmailMaxLength)]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }
        public ICollection<ClientProduct> BoughtProducts { get; set; }
    }
}