using System;
using System.Collections.Generic;
using System.Text;

namespace KonatkHomeParser.Classes
{
    public class MyItem
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Code { get; set; }

        public MyItem(string name, double price, string code)
        {
            Name = name;
            Price = price;
            Code = code;
        }
        public MyItem()
        {

        }
    }
}
