using InventoryMgmt.Server.Data;
using InventoryMgmt.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryMgmt.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{inventoryId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<InventoryDistributionDto>>> GetInventory(int inventoryId)
        {
            try
            {
                var inventoryWarehouseItems = await _context.InventoryWarehouseItems
                    .Include(i => i.InventorySerial)
                    .Include(i => i.WarehouseItem)
                    .Where(i => i.InventoryId == inventoryId).ToListAsync();

                if (inventoryWarehouseItems == null || inventoryWarehouseItems.Count == 0)
                {
                    return NotFound();
                }

                var inventoryDistribution = inventoryWarehouseItems.Select(i =>
                {
                    return new InventoryDistributionDto()
                    {
                        StorageLocation = i.StorageLocation,
                        PartNumber = i.WarehouseItem?.PartNumber,
                        Description = i.WarehouseItem?.Description,
                        Qty = i.WarehouseItem?.Qty,
                        SerialNumber = i.InventorySerial?.SerialNumber
                    };
                });

                return Ok(inventoryDistribution);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }
        }
    }
}
