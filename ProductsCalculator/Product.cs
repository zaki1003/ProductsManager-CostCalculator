﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsCalculator
{
    public class Product

    {

        public Product(int id, string name,string unit, string store, double price)
        {
            Id = id;
            Name = name;
            Unit = unit;
            Store = store;
            Price = price;
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public string Unit { get; set; }
        public string Store { get; set; }
        public double Price { get; set; }
    }
}
