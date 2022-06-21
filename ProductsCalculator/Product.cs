using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsCalculator
{
    internal class Product

    {

        public Product(int id, string name, string store, float price)
        {
            Id = id;
            Name = name;
            Store = store;
            Price = price;
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public string Store { get; set; }
        public float Price { get; set; }
    }
}
