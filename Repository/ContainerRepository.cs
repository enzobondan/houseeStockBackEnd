using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Data;
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

        public async Task<Container> CreateContainerAsync(Container container/*,User user*/)
        {
            await _context.Containers.AddAsync(container);
            await _context.SaveChangesAsync();
            return container;
        }

        public async Task<List<Container>> GetAllContainers()
        {
            return await _context.Containers.Select(c => new Container
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ParentContainerId = c.ParentContainerId,
                PlaceId = c.PlaceId,
                ImagePath = c.ImagePath,
            }).ToListAsync();
        }

        public Task<bool> ContainerExists(int id)
        {
            return _context.Containers.AnyAsync(e => e.Id == id);
        }


        public Task<List<Container>> GetContainersAndItemsAsync(/*User user*/)
        {
            throw new NotImplementedException();
        }

        public Task<List<Container>> GetContainersHierarchyAsync()
        {
            throw new NotImplementedException();
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

        public Task<Container?> UpdateContainerAsync(Container container/*,User user*/)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateContainerParentAsync(int containerId, int? newParentId/*,User user*/)
        {
            var container = await _context.Containers.FirstOrDefaultAsync(c => c.Id == containerId /*&& c.UserId == user.Id*/);
            if (container != null)
            {
                container.ParentContainerId = newParentId;
                await _context.SaveChangesAsync();
            }
        }
    }
}