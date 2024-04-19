using Microsoft.EntityFrameworkCore;
using TreatsAndTails.Models;

namespace TreatsAndTails.Components.Services
{
    public class InventoryService
    {

        private readonly TatContext _context;

        public InventoryService(TatContext context)
        {
            this._context = context;
        }

        public async Task<List<Product>> GetInventory()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
