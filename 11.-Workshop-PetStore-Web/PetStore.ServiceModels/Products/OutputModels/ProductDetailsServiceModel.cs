﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.ViewModels.Product.OutputModels
{
    public class ProductDetailsServiceModel
    {
        public string Name { get; set; }

        public string ProductType { get; set; }

        public decimal Price { get; set; }
    }
}
