using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Interfaces;
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

        private readonly ContainerInterface _containerRepository;

        private readonly PlaceInterface _placeRepository;

        public InventoryController(
            /*UserManager<User> userManager,*/
            ItemInterface itemRepository,
            ContainerInterface containerRepository,
            PlaceInterface placeRepository)
        {
            //_userManager = userManager;
            _itemRepository = itemRepository;
            _containerRepository = containerRepository;
            _placeRepository = placeRepository;
        }

        [HttpGet("places")]
        public async Task<IActionResult> GetPlaces()
        {
            var places = await _placeRepository.GetPlacesAsync();
            return Ok(places);
        }
        [HttpGet("places/{placeId}")]
        public async Task<IActionResult> GetPlaceById(int placeId)
        {
            var place = await _placeRepository.GetFullPlaceByIdAsync(placeId);
            if (place == null)
            {
                return NotFound();
            }
            return Ok(place);
        }

        [HttpGet("AllPossesions")]
        public async Task<IActionResult> GetAllPossessions()
        {
            var containers = await _containerRepository.GetAllContainers();
            var items = await _itemRepository.GetItemsByUserAsync();

            return Ok(new
            {
                Containers = containers,
                Items = items
            });

        }
    }
}