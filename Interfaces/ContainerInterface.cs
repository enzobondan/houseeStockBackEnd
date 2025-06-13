using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Dtos.Container;
using api_stock.Models;

namespace api_stock.Interfaces
{
    public interface ContainerInterface
    {

        Task<List<Container>> GetAllContainers(/*User user*/);

        Task<Container?> GetContainerByIdAsync(int containerId/*,User user*/);

        Task<Container> CreateContainerAsync(CreateContainerDto container/*,User user*/);

        Task<bool?> UpdateContainerAsync(ContainerDto container/*,User user*/);

        Task<Container?> SafeDeleteContainerAsync(int containerId/*,User user*/);

        Task<bool> ContainerExists(int id);
    }
}