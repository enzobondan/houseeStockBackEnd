using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Dtos.Item;
using api_stock.Models;

namespace api_stock.Interfaces
{
    public interface ItemInterface
    {
        Task<List<Item>> GetItemsByUserAsync(/*User user*/);

        Task<Item> CreateItemAsync(Item item);

        Task<Item?> DeleteItemAsync(int itemId);

        Task<Item> UpdateItemAsync(UpdateItemDto item);

        Task UpdateItemContainerAsync(int itemId, int newContainerId/*, User user*/);

    }
}