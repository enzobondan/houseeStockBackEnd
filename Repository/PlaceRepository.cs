using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Data;
using api_stock.Dtos.Place;
using api_stock.Interfaces;
using api_stock.Models;
using Microsoft.EntityFrameworkCore;

namespace api_stock.Repository
{
    public class PlaceRepository : PlaceInterface
    {

        private readonly ApplicationDBContext _context;

        public PlaceRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Place> CreatePlaceAsync(CreatePlaceDto placeDto/*, User user*/)
        {

            var tagNames = placeDto.Tags ?? [];

            var existingTags = await _context.Tags
                .Where(t => tagNames.Contains(t.Name))
                .ToListAsync();

            if (existingTags.Count != tagNames.Count)
            {
                throw new InvalidOperationException("One or more tags do not exist.");
            }

            var place = new Place
            {
                Name = placeDto.Name,
                Description = placeDto.Description,
                Tags = existingTags
            };
            await _context.Places.AddAsync(place);
            await _context.SaveChangesAsync();
            return place;
        }

        public async Task<Place?> GetFullPlaceByIdAsync(int placeId)
        {
            var place = await _context.Places.Include(c => c.Tags)
                .FirstOrDefaultAsync(p => p.Id == placeId);
            if (place == null)
                return null;

            var allContainers = await _context.Containers
                .Where(c => c.PlaceId == placeId)
                .Include(c => c.Items)
                .Include(c => c.Tags)
                .ToListAsync();

            var lookup = allContainers.ToLookup(c => c.ParentContainerId);

            List<Container> BuildTree(int? parentId)
            {
                return [.. lookup[parentId]
                    .Select(c =>
                    {
                        c.Containers = BuildTree(c.Id);
                        return c;
                    })];
            }

            place.Containers = BuildTree(null);
            return place;
        }

        public async Task<PlaceDto?> GetPlaceByIdAsync(int placeId)
        {
            var existingPlace = await _context.Places
                .Include(p => p.Tags)
                .Where(p => p.Id == placeId)
                .Select(p => new PlaceDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Tags = p.Tags.Select(t => t.Name).ToList()
                })
                .FirstOrDefaultAsync();
            return existingPlace;
        }

        public async Task<List<Place>> GetPlacesAsync(/*User user*/)
        {

            return await _context.Places//*.Where(p => p.UserId == user.Id)*/
            .AsNoTracking()
            .ToListAsync();

                
        }

        public Task<bool> PlaceExists(int id)
        {
            return _context.Places.AnyAsync(e => e.Id == id);
        }

        public async Task<Place?> SafeDeletePlaceAsync(int placeId/*, User user*/)
        {
            var place = await _context.Places
                .Include(c => c.Containers)
                .FirstOrDefaultAsync(c => c.Id == placeId /*&& p.UserId == user.Id*/);

            if (place == null) return null;

            foreach (var container in place.Containers)
            {
                container.PlaceId = null;
            }

            _context.Places.Remove(place);
            await _context.SaveChangesAsync();
            return place;
        }

        public async Task<Place?> UpdatePlaceAsync(PlaceDto placeDto/*, User user*/)
        {
            var existingPlace = await _context.Places.Include(c => c.Tags).FirstOrDefaultAsync(p => p.Id == placeDto.Id /*&& p.UserId == user.Id*/);
            if (existingPlace == null)
            {
                return null;
            }
            var tagNames = placeDto.Tags ?? [];

            var existingTags = await _context.Tags
                .Where(t => tagNames.Contains(t.Name))
                .ToListAsync();

            if (existingTags.Count != tagNames.Count)
            {
                throw new InvalidOperationException("One or more tags do not exist.");
            }


            existingPlace.Name = placeDto.Name;
            existingPlace.Description = placeDto.Description;
            existingPlace.Tags = existingTags;

            _context.Places.Update(existingPlace);

            await _context.SaveChangesAsync();
            return existingPlace;
        }
    }
}