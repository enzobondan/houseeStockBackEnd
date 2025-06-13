using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Dtos.Tag;
using api_stock.Models;

namespace api_stock.Interfaces
{
    public interface TagInterface
    {
        Task<List<Tag>> GetTagsAsync();

        Task<Tag?> GetTagWithAssociationByNameAsync(string TagName);

        Task<Tag> CreateTagAsync(string TagName);

        Task<Tag?> SafeDeleteTagAsync(string TagName);

        Task<TagDto> UpdateTagAsync(TagDto tagDto);

        Task<bool?> ExistingTagAsync(string TagName);

        Task<Tag?> GetTagByIdAsync(int id);
    }
}