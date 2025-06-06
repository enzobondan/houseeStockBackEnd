using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_stock.Dtos.Container
{
    public class CreateContainerDto
    {
        public required string Name { get; set; }

        public required string Description { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public int? ParentContainerId { get; set; }

        public int? PlaceId { get; set; }
        
        public string? ImagePath { get; set; }
    }
}