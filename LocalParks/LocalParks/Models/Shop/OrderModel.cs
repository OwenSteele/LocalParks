using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Models.Shop
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public DateTime DeliveryDate { get; set; }
        [Required]
        [MinLength(5)]
        public string OrderNumber { get; set; }
        public ICollection<OrderItemModel> Items { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
