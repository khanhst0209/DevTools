using MyWebAPI.Dto;
using MyWebAPI.data;
using MyWebAPI.Helper;

namespace MyWebAPI.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Item>> GetAllAsync(ItemQuerry querry);
        Task<Item?> GetByIdAsync(int id);
        Task AddItemAsync(CreateItemDTO item);
        Task UpdateItemAsync(int id,UpdateItemDTO item);

        Task RemovebyId(int id);
    }
}