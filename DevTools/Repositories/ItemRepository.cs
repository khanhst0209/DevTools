using Microsoft.EntityFrameworkCore;
using MyWebAPI.data;
using MyWebAPI.Dto;
using MyWebAPI.Helper;
using MyWebAPI.Repositories.Interfaces;

namespace MyWebAPI.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly MyDbContext _context;
        public ItemRepository(MyDbContext _context)
        {
            this._context = _context;
        }

        public async Task AddItemAsync(CreateItemDTO item)
        {
            var newItem = new Item
            {
                Name = item.Name,
                Price = item.Price,
            };

            await _context.Items.AddAsync(newItem);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Item>> GetAllAsync(ItemQuerry querry)
        {
            var items = _context.Items.AsQueryable();

            if(querry.Name != null)
                items = items.Where(x => x.Name.Contains(querry.Name));

            if(querry.Price != null)
                items = items.Where(x => x.Price <= querry.Price);
            
            if(querry.SortBy != null)
            {
                if(querry.SortBy == "Price")
                {
                    items = querry.IsDescending ? items.OrderByDescending(x => x.Price) : items.OrderBy(x => x.Price);
                }
            }
            var skip_number = (querry.Pagenumbers - 1) * querry.Page_Size;
            
            return await items.Skip(skip_number).Take(querry.Page_Size).ToListAsync();
        }


        public async Task<Item?> GetByIdAsync(int id) // Đổi tên hàm
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task RemovebyId(int id)
        {
            var existingItem  = await _context.Items.FindAsync(id);
            if(existingItem != null)
            {
                _context.Items.Remove(existingItem);
                await _context.SaveChangesAsync();
            }
        }


        public async Task UpdateItemAsync(int id, UpdateItemDTO item)
        {
            var existingItem = await _context.Items.FindAsync(id);
            if (existingItem != null)
            {
                existingItem.Name = item.Name;
                existingItem.Price = item.Price;

                await _context.SaveChangesAsync();
            }
        }
    }
}
