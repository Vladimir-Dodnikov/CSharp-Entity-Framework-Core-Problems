using PetStore.Comman;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PetStore.Models
{
    public class Order
    {
        public Order()
        {
            this.Id = Guid.NewGuid().ToString();

            this.ClientProducts = new HashSet<ClientProduct>();
        }
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(GlobalConstant.OrderTownNameMaxLength)]
        public string Town { get; set; }

        [Required]
        [MaxLength(GlobalConstant.OrderAddressMaxLength)]
        public string Address { get; set; }

        public string Notes { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public virtual ICollection<ClientProduct> ClientProducts { get; set; }

        public decimal TotalPrice => this.ClientProducts.Sum(x => x.Product.Price * x.Quantity);
    }
}
