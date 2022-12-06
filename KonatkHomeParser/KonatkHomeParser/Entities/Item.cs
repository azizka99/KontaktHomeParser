using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KonatkHomeParser.Entities
{
    public class Item
    {
        public int Id { get; set; }

        
        [MaxLength(1300)]
        public string Name { get; set; }
        public double Price { get; set; }

        [MaxLength(1300)]
        public string Code { get; set; }
        [MaxLength(1300)]
        public string LinkToItem { get; set; }
    }
}
