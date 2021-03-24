using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Models.Shop
{
    public class ProductModel
    {   
            public int ProductId { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }
            public string ImageId { get; set; }
            public string Category { get; set; }
    }
}
