using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectUs.Model.Entities;

namespace ConnectUs.Model
{
    public class ItemViewModel
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}