using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Dtos;
using api_stock.Dtos.Container;
using api_stock.Dtos.Item;
using api_stock.Dtos.Place;
using api_stock.Interfaces;
using api_stock.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_stock.Controllers
{
    [Route("alteration")]
    [ApiController]
    public class AlterationController : ControllerBase
    {
        private readonly ItemInterface _itemRepository;

        private readonly ContainerInterface _containerRepository;

        private readonly PlaceInterface _placeRepository;

        public AlterationController(
         ItemInterface itemRepository,
        ContainerInterface containerRepository,
        PlaceInterface placeRepository)
        {
            _itemRepository = itemRepository;
            _containerRepository = containerRepository;
            _placeRepository = placeRepository;
        }

        [HttpPost("newItem")]
        public async Task<IActionResult> CreateItem(CreateItemDto itemDto)
        {
            if (itemDto == null) return BadRequest("Item não pode ser nulo");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (itemDto.ContainerId.HasValue)
            {
                if (!await _containerRepository.ContainerExists(itemDto.ContainerId.Value)) return BadRequest("Container não existe");
            }

            var item = new Item
            {
                Name = itemDto.Name,
                Description = itemDto.Description,
                Tags = itemDto.Tags,
                ContainerId = itemDto.ContainerId,
                Amount = itemDto.Amount,
                ImagePath = itemDto.ImagePath
            };

            await _itemRepository.CreateItemAsync(item);
            return Created();
        }

        [HttpPost("newContainer")]
        public async Task<IActionResult> CreateContainer(CreateContainerDto containerDto)
        {
            if (containerDto == null) return BadRequest("Container não pode ser nulo");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (containerDto.ParentContainerId.HasValue)
            {
                if (!await _containerRepository.ContainerExists(containerDto.ParentContainerId.Value)) return BadRequest("Container pai não existe");
            }
            if (containerDto.PlaceId.HasValue)
            {
                if (!await _placeRepository.PlaceExists(containerDto.PlaceId.Value)) return BadRequest("Local não existe");
            }
            var container = new Container
            {
                Name = containerDto.Name,
                Description = containerDto.Description,
                Tags = containerDto.Tags,
                ParentContainerId = containerDto.ParentContainerId,
                PlaceId = containerDto.PlaceId,

                ImagePath = containerDto.ImagePath
            };

            await _containerRepository.CreateContainerAsync(container);
            return Created();

        }

        [HttpPost("newPlace")]
        public async Task<IActionResult> CreatePlace(CreatePlaceDto placeDto)
        {
            if (placeDto == null) return BadRequest("Local não pode ser nulo");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var place = new Place
            {
                Name = placeDto.Name,
                Description = placeDto.Description,
                Tags = placeDto.Tags
            };
            await _placeRepository.CreatePlaceAsync(place);
            return Created();
        }
            


    }
}