using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    public class ImportCarDTO
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public long TravelledDistance { get; set; }

        public virtual ICollection<int> PartsId { get; set; }
    }
}
