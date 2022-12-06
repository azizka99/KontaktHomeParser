using System;
using System.Collections.Generic;
using System.Text;

namespace KonatkHomeParser.Classes
{
    public class Phone
    {
        public string Name { get; set; }
        public int Memory { get; set; }
        public string MemoryType { get; set; }
        public string Color { get; set; }
        public double Price { get; set; }

        public Phone(string name, int memory, string memoryType, string color, double price)
        {
            Name = name;
            Memory = memory;
            MemoryType = memoryType;
            Color = color;
            Price = price;
        }
    }
}
