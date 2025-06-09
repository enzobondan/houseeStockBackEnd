using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Dtos;
using api_stock.Dtos.Item;
using api_stock.Models;

namespace api_stock.Interfaces
{
    public interface ItemInterface
    {
        Task<List<Item>> GetItemsByUserAsync(/*User user*/);

        Task<Item> GetItemByIdAsync(int itemId/*,User user*/);

        Task<Item> CreateItemAsync(CreateItemDto item);

        Task<Item?> DeleteItemAsync(int itemId);

        Task<Item> UpdateItemAsync(ItemDto item);
        Task<bool> ItemExists(int id);

    }
}