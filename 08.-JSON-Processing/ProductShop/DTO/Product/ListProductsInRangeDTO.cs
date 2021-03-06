﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DTO.Product
{
    public class ListProductsInRangeDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("seller")]
        public string Seller { get; set; }
    }
}
