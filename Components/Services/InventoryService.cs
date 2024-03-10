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

        public List<Product> GetInventory()
        {
            return _context.Products.ToList();
        }
    }
}
