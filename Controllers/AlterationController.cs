using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Dtos;
using api_stock.Dtos.Container;
using api_stock.Dtos.Item;
using api_stock.Dtos.Place;
using api_stock.Dtos.Tag;
using api_stock.Interfaces;
using api_stock.Models;
using api_stock.Repository;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpPut("updateItem")]
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

        [HttpPatch("updateItemField/{id}")]
        public async Task<IActionResult> PartialUpdateItem(int id, [FromBody] JsonPatchDocument<ItemDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Atualizações não podem ser nulas ou vazias");

            var existingItem = await _itemRepository.GetItemByIdAsync(id);
            if (existingItem == null)
                return NotFound("Item não existe");


            patchDoc.ApplyTo(existingItem, ModelState);

            await _itemRepository.UpdateItemAsync(existingItem);

            return Ok(existingItem);
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

        [HttpPut("updateContainer")]
        public async Task<IActionResult> UpdateContainer(ContainerDto containerDto)
        {
            if (containerDto == null) return BadRequest("Container não pode ser nulo");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _containerRepository.UpdateContainerAsync(containerDto);
            if (result == null) return NotFound("Container não existe");

            if (result == false) return BadRequest("Dados inválidos para atualização");

            return Ok();
        }


        [HttpPatch("updateContainerField/{id}")]
        public async Task<IActionResult> PartialUpdateContainer(int id, [FromBody] JsonPatchDocument<ContainerDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Atualizações não podem ser nulas ou vazias");

            var existingContainer = await _containerRepository.GetContainerByIdAsync(id);
            if (existingContainer == null)
                return NotFound("Container não existe");

            var containerDto = new ContainerDto
            {
                Id = existingContainer.Id,
                Name = existingContainer.Name,
                Description = existingContainer.Description,
                Tags = existingContainer.Tags?.Select(t => t.Name).ToList() ?? [],
                PlaceId = existingContainer.PlaceId,
                ParentContainerId = existingContainer.ParentContainerId,
                ImagePath = existingContainer.ImagePath
            }; 


            patchDoc.ApplyTo(containerDto, ModelState);


            Console.WriteLine($"ContainerDto after patch: {containerDto}");

            await _containerRepository.UpdateContainerAsync(containerDto);

            return Ok(containerDto);
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

        [HttpPut("updatePlace")]
        public async Task<IActionResult> UpdatePlace(PlaceDto placeDto)
        {
            if (placeDto == null) return BadRequest("Local não pode ser nulo");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _placeRepository.PlaceExists(placeDto.Id)) return NotFound("Local não existe");

            var result = await _placeRepository.UpdatePlaceAsync(placeDto);
            if (result == null) return NotFound("Local não encontrado");
            return Ok();
        }
        [HttpPatch("updatePlaceField/{id}")]
        public async Task<IActionResult> PartialUpdatePlace(int id, [FromBody] JsonPatchDocument<PlaceDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Atualizações não podem ser nulas ou vazias");

            var existingPlace = await _placeRepository.GetPlaceByIdAsync(id);
            if (existingPlace == null)
                return NotFound("Item não existe");


            patchDoc.ApplyTo(existingPlace, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _placeRepository.UpdatePlaceAsync(existingPlace);
            return Ok(existingPlace);
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

        [HttpPut("updateTag")]
        public async Task<IActionResult> UpdateTag(TagDto newTag)
        {
            if (newTag == null) return BadRequest("Tag não pode ser nulo");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingTag = await _tagRepository.GetTagByIdAsync(newTag.Id);
            if (existingTag == null)
            {
                return NotFound("Tag não existe");
            }

            var updatedTag = await _tagRepository.UpdateTagAsync(newTag);

            if (updatedTag == null)
            {
                return BadRequest("Failed to update tag.");
            }

            return Ok(new { message = "Tag updated successfully", tag = updatedTag });
        }
        
    }
}