using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Data;
using api_stock.Dtos.Container;
using api_stock.Interfaces;
using api_stock.Models;
using Microsoft.EntityFrameworkCore;

namespace api_stock.Repository
{
    
    public class ContainerRepository : ContainerInterface
    {
        private readonly ApplicationDBContext _context;

        public ContainerRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Container> CreateContainerAsync(CreateContainerDto containerDto/*,User user*/)
        {

            var tagNames = containerDto.Tags ?? [];

            var existingTags = await _context.Tags
                .Where(t => tagNames.Contains(t.Name))
                .ToListAsync();

            if (existingTags.Count != tagNames.Count)
            {
                throw new InvalidOperationException("One or more tags do not exist.");
            }


            var container = new Container
            {
                Name = containerDto.Name,
                Description = containerDto.Description,
                Tags = existingTags,
                ParentContainerId = containerDto.ParentContainerId,
                PlaceId = containerDto.PlaceId,

                ImagePath = containerDto.ImagePath
            };

            await _context.Containers.AddAsync(container);
            await _context.SaveChangesAsync();
            return container;
        }

        public async Task<List<Container>> GetAllContainers()
        {
            return await _context.Containers
            .Include(c => c.Tags)
            .Select(c => new Container
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ParentContainerId = c.ParentContainerId,
                PlaceId = c.PlaceId,
                ImagePath = c.ImagePath,
                Tags = c.Tags
            }).ToListAsync();
        }

        public Task<bool> ContainerExists(int id)
        {
            return _context.Containers.AnyAsync(e => e.Id == id);
        }

        public async Task<Container?> SafeDeleteContainerAsync(int containerId/*,User user*/)
        {
            var container = await _context.Containers
                .Include(c => c.Containers)
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == containerId /*&& c.UserId == user.Id*/);

            if (container == null) return null;

            foreach (var child in container.Containers)
            {
                child.ParentContainerId = null;
            }

            foreach (var item in container.Items)
            {
                item.ContainerId = null;
            }

            _context.Containers.Remove(container);
            await _context.SaveChangesAsync();
            return container;
        }

        public async Task<bool?> UpdateContainerAsync(ContainerDto dto)
        {
            var existing = await _context.Containers
            .Include(c => c.Tags)
            .FirstOrDefaultAsync(c => c.Id == dto.Id);
            if (existing == null) return null;


            if (dto.ParentContainerId.HasValue && dto.ParentContainerId.Value == dto.Id)
            {
                throw new InvalidOperationException("A container cannot be its own parent.");
            }

            if (dto.ParentContainerId.HasValue)
            {
                var parentExists = await _context.Containers.FirstOrDefaultAsync(c => c.Id == dto.ParentContainerId.Value);
                if (parentExists == null) return false;
                if (parentExists.ParentContainerId == dto.Id) throw new InvalidOperationException("A container cannot be its child parent.");
            }

            if (dto.PlaceId.HasValue)
            {
                var placeExists = await _context.Places.AnyAsync(p => p.Id == dto.PlaceId.Value);
                if (!placeExists) return false;
            }

            var tagNames = dto.Tags ?? [];

            var existingTags = await _context.Tags
                .Where(t => tagNames.Contains(t.Name))
                .ToListAsync();

            if (existingTags.Count != tagNames.Count)
            {
                throw new InvalidOperationException("One or more tags do not exist.");
            }

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Tags = existingTags;
            existing.ParentContainerId = dto.ParentContainerId;
            existing.PlaceId = dto.PlaceId;
            existing.ImagePath = dto.ImagePath;

            _context.Containers.Update(existing);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Container?> GetContainerByIdAsync(int containerId)
        {
            var root = await _context.Containers
                .Include(c => c.Items)
                .Include(c => c.Tags)
                    
                .FirstOrDefaultAsync(c => c.Id == containerId);

            if (root == null)
                return null;

            var allContainers = await _context.Containers
                .Include(c => c.Items)
                .Include(c => c.Tags)
                .ToListAsync();

            var lookup = allContainers.ToLookup(c => c.ParentContainerId);

            void BuildTree(Container parent)
            {
                parent.Containers = lookup[parent.Id].ToList();
                foreach (var child in parent.Containers)
                {
                    BuildTree(child);
                }
            }

            BuildTree(root);
            return root;
        }
    }
}