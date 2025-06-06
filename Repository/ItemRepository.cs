using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Data;
using api_stock.Dtos.Item;
using api_stock.Interfaces;
using api_stock.Models;
using Microsoft.EntityFrameworkCore;

namespace api_stock.Repository
{

    public class ItemRepository : ItemInterface
    {
        private readonly ApplicationDBContext _context;

        public ItemRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Item> CreateItemAsync(Item item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public Task<Item?> DeleteItemAsync(int itemId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Item>> GetItemsByUserAsync(/*User user*/)
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item> GetItemByIdAsync(int itemId/*, User user*/)
        {

            var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId /*&& i.UserId == user.Id*/);
            if (existingItem == null)
            {
                throw new InvalidOperationException("Item not found or does not belong to the user.");
            }
            return existingItem;
        }

        public async Task<Item> UpdateItemAsync(UpdateItemDto itemDto/*, User user*/)
        {
            var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemDto.Id /*&& i.UserId == user.Id*/) ?? throw new InvalidOperationException("Item not found or does not belong to the user.");

            existingItem.Name = itemDto.Name;
            existingItem.Description = itemDto.Description;
            existingItem.Tags = itemDto.Tags;
            existingItem.ContainerId = itemDto.ContainerId;
            existingItem.Amount = itemDto.Amount;
            existingItem.ImagePath = itemDto.ImagePath;

             _context.Items.Update(existingItem);
            await _context.SaveChangesAsync();

            return existingItem;
        }

        public Task<bool> ItemExists(int id)
        {
            return _context.Items.AnyAsync(e => e.Id == id);
        }
    }
}