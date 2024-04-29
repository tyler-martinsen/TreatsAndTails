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

        public async Task<Product?> GetItem(int id)
        {
			return await _context.Products.FirstOrDefaultAsync(item => item.Id == id);
		}

        public async Task<bool> SetItem(string? shape,string? flavor,string? size,string? description,decimal? cost,int? quantity, int id = 0)
        {
            Product newItem = new Product();

            // Add product instead of edit
            if(id > 0)
            {
				newItem = (await _context.Products.FirstOrDefaultAsync(item => item.Id == id)) ?? newItem;
			}

			// Update Non-null items
			if (shape != null)
			{
				newItem.InvShape = shape;
			}
			if (flavor != null)
			{
				newItem.InvFlavor = flavor;
			}
			if (size != null)
			{
				newItem.InvSize = size;
			}
			if (description != null)
			{
				newItem.InvDescription = description;
			}
			if (cost != null)
			{
				newItem.Cost = cost ?? newItem.Cost;
			}
			if (quantity != null)
			{
				newItem.Quantity = quantity ?? newItem.Quantity;
			}

			// Ensure All Fields Are Valid
			if(newItem == null)
			{
				return false;
			}
			else if (newItem.InvShape == null)
			{
				return false;
			}
			else if (newItem.InvFlavor == null)
			{
				return false;
			}
			else if (newItem.InvSize == null)
			{
				return false;
			}
			else if (newItem.InvDescription == null)
			{
				return false;
			}
			else if (newItem.Cost < 0)
			{
				return false;
			}
			else if (newItem.Quantity < 0)
			{
				return false;
			}

			if (id <= 0)
			{
				await _context.Products.AddAsync(newItem);
			}

			return await _context.SaveChangesAsync() > 0;
		}
    }
}
