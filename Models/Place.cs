using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_stock.Models
{
    public class Place
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public List<Container> Containers { get; set; } = new List<Container>();

        //public required string UserId { get; set; }

        //public required User User { get; set; }
    }
}