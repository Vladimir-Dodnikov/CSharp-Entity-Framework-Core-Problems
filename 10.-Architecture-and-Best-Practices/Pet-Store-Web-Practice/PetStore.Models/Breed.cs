﻿using PetStore.Comman;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models
{
    public class Breed
    {
        public Breed()
        {
            this.Pets = new HashSet<Pet>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(GlobalConstant.BreedNameMinLength), MaxLength(GlobalConstant.BreedNameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }
    }
}