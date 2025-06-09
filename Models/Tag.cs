using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_stock.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();

        public List<Container> Containers { get; set; } = new List<Container>();

        public List<Place> Places { get; set; } = new List<Place>();
        
    }
}