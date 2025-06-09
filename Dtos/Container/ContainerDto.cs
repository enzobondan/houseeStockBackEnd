using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Models;

namespace api_stock.Dtos.Container
{
    public class ContainerDto
    {        
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public List<string> Tags { get; set; } = [];

        public int? ParentContainerId { get; set; }

        public int? PlaceId { get; set; }

        public string? ImagePath { get; set; }
    }
}