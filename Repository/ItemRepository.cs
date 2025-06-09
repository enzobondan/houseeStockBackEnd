using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Data;
using api_stock.Dtos;
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

        public async Task<Item> CreateItemAsync(CreateItemDto itemDto)
        {
            var tagNames = itemDto.Tags
            ?.Select(t => t.Trim())
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList()
            ?? [];

            var existingTags = await _context.Tags
                .Where(t => tagNames.Contains(t.Name))
                .ToListAsync();

            if (existingTags.Count != tagNames.Count)
            {
                throw new InvalidOperationException("One or more tags do not exist.");
            }
            var existingTagNames = existingTags.Select(t => t.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var missingTags = tagNames.Where(name => !existingTagNames.Contains(name)).ToList();

            if (missingTags.Count != 0)
            {
                throw new InvalidOperationException($"As tags não existem: {string.Join(", ", missingTags)}");
            }

            var item = new Item
            {
                Name = itemDto.Name,
                Description = itemDto.Description,
                Tags = existingTags,
                ContainerId = itemDto.ContainerId,
                Amount = itemDto.Amount,
                ImagePath = itemDto.ImagePath
            };
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public Task<Item?> DeleteItemAsync(int itemId)
        {
            var existingItem = _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);
            if (existingItem == null)
            {
                throw new InvalidOperationException("Item not found.");
            }

            _context.Items.Remove(existingItem.Result);
            _context.SaveChangesAsync();
            return existingItem;
        }

        public async Task<List<Item>> GetItemsByUserAsync(/*User user*/)
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item> GetItemByIdAsync(int itemId/*, User user*/)
        {

            var existingItem = await _context.Items.Include(c => c.Tags).FirstOrDefaultAsync(i => i.Id == itemId /*&& i.UserId == user.Id*/);
            if (existingItem == null)
            {
                throw new InvalidOperationException("Item not found or does not belong to the user.");
            }
            return existingItem;
        }

        public async Task<Item> UpdateItemAsync(ItemDto itemDto/*, User user*/)
        {
            var existingItem = await _context.Items.Include(c => c.Tags).FirstOrDefaultAsync(i => i.Id == itemDto.Id /*&& i.UserId == user.Id*/) ?? throw new InvalidOperationException("Item not found or does not belong to the user.");


            var tagNames = itemDto.Tags ?? [];

            var existingTags = await _context.Tags
                .Where(t => tagNames.Contains(t.Name))
                .ToListAsync();

            if (existingTags.Count != tagNames.Count)
            {
                throw new InvalidOperationException("One or more tags do not exist.");
            }
            existingItem.Name = itemDto.Name;
            existingItem.Description = itemDto.Description;
            existingItem.Tags = existingTags;
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