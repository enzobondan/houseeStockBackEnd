using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_stock.Models
{
    public class Container
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public string? Description { get; set; }

        //origin
        public int? PlaceId { get; set; }

        public Place? Place { get; set; }

        //child
        public List<Item> Items { get; set; } = new List<Item>();

        public List<Container> Containers { get; set; } = new List<Container>();

        //possible parent
        public int? ParentContainerId { get; set; }
        public Container? ParentContainer { get; set; }

        public string? ImagePath { get; set; }
        
        //public required string UserId { get; set; }

        //public required User User { get; set; }
    }
}