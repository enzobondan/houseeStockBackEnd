using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Dtos.Container;
using api_stock.Dtos.Tag;
using api_stock.Interfaces;
using api_stock.Models;
using api_stock.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api_stock.Controllers
{
    [Route("inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        //private readonly UserManager<User> _userManager;

        private readonly ItemInterface _itemRepository;

        private readonly TagInterface _tagRepository;

        private readonly ContainerInterface _containerRepository;

        private readonly PlaceInterface _placeRepository;

        public InventoryController(
            /*UserManager<User> userManager,*/
            ItemInterface itemRepository,
            ContainerInterface containerRepository,
            PlaceInterface placeRepository,
            TagInterface tagRepository)

        {
            //_userManager = userManager;
            _itemRepository = itemRepository;
            _containerRepository = containerRepository;
            _placeRepository = placeRepository;
            _tagRepository = tagRepository;
        }

        [HttpGet("places")]
        public async Task<IActionResult> GetPlaces()
        {
            var places = await _placeRepository.GetPlacesAsync();
            return Ok(places);
        }
        [HttpGet("placeById")]
        public async Task<IActionResult> GetPlaceById(int placeId)
        {
            var place = await _placeRepository.GetFullPlaceByIdAsync(placeId);
            if (place == null)
            {
                return NotFound();
            }
            return Ok(place);
        }

        [HttpGet("container")]
        public async Task<IActionResult> GetContainers(int containerId)
        {
            var containerExists = await _containerRepository.ContainerExists(containerId);
            if (!containerExists)
            {
                return NotFound("Container not found");
            }
            var container = await _containerRepository.GetContainerByIdAsync(containerId);
            if (container == null)
            {
                return NotFound("Container not found");
            }
            return Ok(container);
        }

        [HttpGet("ItemById")]
        public async Task<IActionResult> GetItemById(int itemId)
        {
            var item = await _itemRepository.ItemExists(itemId);
            if (item)
            {
                var existingItem = await _itemRepository.GetItemByIdAsync(itemId);
                return Ok(existingItem);
            }
            else
            {
                return NotFound("Item not found");
            }
        }



        [HttpGet("AllPossessions")]
        public async Task<IActionResult> GetAllPossessions()
        {
            var containers = await _containerRepository.GetAllContainers();

            var containerDtos = containers.Select(c => new
            {
                c.Id,
                c.Name,
                c.Description,
                c.Tags,
                c.PlaceId,
                c.ParentContainerId,
                c.ImagePath
            }).ToList();
            var items = await _itemRepository.GetItemsByUserAsync();

            var itemDtos = items.Select(i => new
            {
                i.Id,
                i.Name,
                i.Description,
                i.Tags,
                i.ContainerId,
                i.Amount,
                i.ImagePath
            }).ToList();

            return Ok(new
            {
                Containers = containerDtos,
                Items = itemDtos
            });

        }

        [HttpGet("tags")]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _tagRepository.GetTagsAsync();
            var tagDtos = tags.Select(t => new
            {
                t.Name
            }).ToList();

            return Ok(tagDtos);
        }

        [HttpGet("tagByName")]
        public async Task<IActionResult> GetTagWithAssociations(string name)
        {
            var tag = await _tagRepository.GetTagWithAssociationByNameAsync(name);
            if (tag == null) return NotFound();

            return Ok(tag);
        }
    }
}