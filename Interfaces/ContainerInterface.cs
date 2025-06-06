using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Models;

namespace api_stock.Interfaces
{
    public interface ContainerInterface
    {

        Task<List<Container>> GetAllContainers(/*User user*/);

        Task<List<Container>> GetContainersHierarchyAsync();

        Task<Container> CreateContainerAsync(Container container/*,User user*/);

        Task<Container?> UpdateContainerAsync(Container container/*,User user*/);

        Task<Container?> SafeDeleteContainerAsync(int containerId/*,User user*/);

        Task UpdateContainerParentAsync(int containerId, int? newParentId/*,User user*/);

        Task<bool> ContainerExists(int id);
    }
}