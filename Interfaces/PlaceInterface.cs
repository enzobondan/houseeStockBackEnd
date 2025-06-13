using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Dtos.Place;
using api_stock.Models;

namespace api_stock.Interfaces
{
    public interface PlaceInterface
    {

        Task<List<Place>> GetPlacesAsync();

        Task<Place?> GetFullPlaceByIdAsync(int placeId);

        Task<PlaceDto?> GetPlaceByIdAsync(int placeId);

        Task<Place> CreatePlaceAsync(CreatePlaceDto place);

        Task<Place?> SafeDeletePlaceAsync(int placeId);

        Task<Place?> UpdatePlaceAsync(PlaceDto place);

        Task<bool> PlaceExists(int id);
    }
}