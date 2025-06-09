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

        private readonly TagInterface _tagRepository;

        public AlterationController(
         ItemInterface itemRepository,
        ContainerInterface containerRepository,
        PlaceInterface placeRepository,
        TagInterface tagRepository)
        {
            _itemRepository = itemRepository;
            _containerRepository = containerRepository;
            _placeRepository = placeRepository;
            _tagRepository = tagRepository;
        }

        //////////////////////////////////// ITEM ////////////////////////////////////

        [HttpPost("newItem")]
        public async Task<IActionResult> CreateItem(CreateItemDto itemDto)
        {
            if (itemDto == null) return BadRequest("Item não pode ser nulo");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (itemDto.ContainerId.HasValue)
            {
                if (!await _containerRepository.ContainerExists(itemDto.ContainerId.Value)) return BadRequest("Container não existe");
            }

            await _itemRepository.CreateItemAsync(itemDto);
            return Created();
        }

        [HttpPost("updateItem")]
        public async Task<IActionResult> UpdateItem(ItemDto itemDto)
        {
            if (itemDto == null) return BadRequest("Item não pode ser nulo");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _itemRepository.ItemExists(itemDto.Id)) return NotFound("Item não existe");
            if (itemDto.ContainerId.HasValue)
            {
                if (!await _containerRepository.ContainerExists(itemDto.ContainerId.Value)) return BadRequest("Container não existe");
            }
            var itemModel = await _itemRepository.UpdateItemAsync(itemDto);
            if (itemModel == null) return NotFound();
            return Ok();
        }

        [HttpDelete("deleteItem")]

        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var itemExists = await _itemRepository.ItemExists(itemId);
            if (!itemExists) return NotFound("Item não existe");
            var deletedItem = await _itemRepository.DeleteItemAsync(itemId);
            if (deletedItem == null) return NotFound("Item não encontrado");
            return Ok(new { message = "Item deletado com sucesso", item = deletedItem });
        }




        //////////////////////////////////// CONTAINER ////////////////////////////////////

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


            await _containerRepository.CreateContainerAsync(containerDto);
            return Created();

        }

        [HttpPost("updateContainer")]
        public async Task<IActionResult> UpdateContainer(ContainerDto containerDto)
        {
            if (containerDto == null) return BadRequest("Container não pode ser nulo");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _containerRepository.UpdateContainerAsync(containerDto);
            if (result == null) return NotFound("Container não existe");

            if (result == false) return BadRequest("Dados inválidos para atualização");

            return Ok();
        }

        [HttpDelete("deleteContainer")]
        public async Task<IActionResult> DeleteContainer(int containerId)
        {
            var containerExists = await _containerRepository.ContainerExists(containerId);
            if (!containerExists) return NotFound("Container não existe");
            var deletedContainer = await _containerRepository.SafeDeleteContainerAsync(containerId);
            if (deletedContainer == null) return NotFound("Container não encontrado");
            return Ok(new { message = "Container deletado com sucesso", container = deletedContainer });
        }







        //////////////////////////////////// PLACE ////////////////////////////////////


        [HttpPost("newPlace")]
        public async Task<IActionResult> CreatePlace(CreatePlaceDto placeDto)
        {
            if (placeDto == null) return BadRequest("Local não pode ser nulo");
            if (!ModelState.IsValid) return BadRequest(ModelState);


            await _placeRepository.CreatePlaceAsync(placeDto);
            return Created();
        }

        [HttpPost("updatePlace")]
        public async Task<IActionResult> UpdatePlace(PlaceDto placeDto)
        {
            if (placeDto == null) return BadRequest("Local não pode ser nulo");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _placeRepository.PlaceExists(placeDto.Id)) return NotFound("Local não existe");

            var result = await _placeRepository.UpdatePlaceAsync(placeDto);
            if (result == null) return NotFound("Local não encontrado");
            return Ok();
        }


        [HttpDelete("deletePlace")]
        public async Task<IActionResult> DeletePlace(int placeId)
        {
            var placeExists = await _placeRepository.PlaceExists(placeId);
            if (!placeExists) return NotFound("Local não existe");
            var deletedPlace = await _placeRepository.SafeDeletePlaceAsync(placeId);
            if (deletedPlace == null) return NotFound("Local não encontrado");
            return Ok(new { message = "Local deletado com sucesso", place = deletedPlace });
        }






        //////////////////////////////////// TAG ////////////////////////////////////


        [HttpPost("newTag")]
        public async Task<IActionResult> CreateTag(string TagName)
        {
            var existingTag = await _tagRepository.ExistingTagAsync(TagName);

            if (existingTag == true)
            {
                return BadRequest("Tag already exists.");
            }

            var createTag = await _tagRepository.CreateTagAsync(TagName);

            if (createTag == null)
            {
                return BadRequest("Failed to create tag.");
            }
            return Created("Tag created successfully.", new { tag = createTag });


        }
        
        [HttpDelete("deleteTag")]
        public async Task<IActionResult> DeleteTag(string tagName)
        {
            var tag = await _tagRepository.ExistingTagAsync(tagName);
            if (tag == null)
                return NotFound("Tag não encontrada");

            await _tagRepository.SafeDeleteTagAsync(tagName);
            return Ok(new { message = "Tag deletada com sucesso" });
        }
    }
}