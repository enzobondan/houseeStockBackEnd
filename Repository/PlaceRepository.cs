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
        public async Task<Place> CreatePlaceAsync(Place place/*, User user*/)
        {
            await _context.Places.AddAsync(place);
            await _context.SaveChangesAsync();
            return place;
        }

        public async Task<Place?> GetFullPlaceByIdAsync(int placeId)
        {
            var place = await _context.Places
                .FirstOrDefaultAsync(p => p.Id == placeId);
            if (place == null)
                return null;

            var allContainers = await _context.Containers
                .Where(c => c.PlaceId == placeId)
                .Include(c => c.Items)
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

        public async Task<Place?> UpdatePlaceAsync(UpdatePlaceDto placeDto/*, User user*/)
        {
            var existingPlace = await _context.Places.FirstOrDefaultAsync(p => p.Id == placeDto.Id /*&& p.UserId == user.Id*/);
            if (existingPlace == null)
            {
                return null;
            }
            existingPlace.Name = placeDto.Name;
            existingPlace.Description = placeDto.Description;

            await _context.SaveChangesAsync();
            return existingPlace;
        }
    }
}