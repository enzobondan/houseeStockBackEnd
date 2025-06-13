using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Data;
using api_stock.Dtos.Tag;
using api_stock.Interfaces;
using api_stock.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api_stock.Repository
{
    public class TagRepository : TagInterface
    {
        
        private readonly ApplicationDBContext _context;

        public TagRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Tag> CreateTagAsync(string TagName)
        {
            Tag newTag = new()
            {
                Name = TagName
            };
            _context.Tags.Add(newTag);
            _context.SaveChanges();
            return await Task.FromResult(newTag);
        }

        public async Task<bool?> ExistingTagAsync(string TagName)
        {
            bool exists = await _context.Tags.AnyAsync(t => t.Name == TagName);
            return exists;
        }

        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            return tag;
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            var tags = _context.Tags.ToList();
            return await Task.FromResult(tags);
        }

        public async Task<Tag?> GetTagWithAssociationByNameAsync(string TagName)
        {
            var tag = await _context.Tags
            .Where(t => t.Name == TagName)
            .Include(t => t.Items)
            .Include(t => t.Containers)
            .Include(t => t.Places)
            .FirstOrDefaultAsync();
            
            return tag;
        }

        public async Task<Tag?> SafeDeleteTagAsync(string TagName)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == TagName) ?? throw new InvalidOperationException("Tag not found.");
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<TagDto> UpdateTagAsync(TagDto tagDto)
        {
            var tag = await GetTagByIdAsync(tagDto.Id) 
                ?? throw new InvalidOperationException("Tag not found.");

            tag.Name = tagDto.Name;
            _context.Tags.Update(tag);

            await _context.SaveChangesAsync();

            return tagDto;
        }
    }
}