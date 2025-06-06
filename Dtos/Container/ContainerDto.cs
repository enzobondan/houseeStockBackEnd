using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_stock.Dtos.Container
{
    public class ContainerDto
    {        
        public int? PlaceId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public string? ImagePath { get; set; }
    }
}