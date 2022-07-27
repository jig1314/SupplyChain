using AutoMapper;
using DotNetCore.CAP;
using InventoryMgmt.Server.Data;
using InventoryMgmt.Server.Models;
using InventoryMgmt.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryMgmt.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public WarehousesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Warehouses
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<WarehouseDto>>> GetWarehouses()
        {
            try
            {
                if (_context.Warehouses == null)
                {
                    return NotFound();
                }

                var warehouses = await _context.Warehouses.ToListAsync();
                var warehouseDtos = _mapper.Map<List<WarehouseDto>>(warehouses);

                return Ok(warehouseDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }
        }

        // GET: api/Warehouses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WarehouseDto>> GetWarehouse(int id)
        {
            if (_context.Warehouses == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses.FindAsync(id);

            if (warehouse == null)
            {
                return NotFound();
            }

            var warehouseDto = _mapper.Map<WarehouseDto>(warehouse);

            return warehouseDto;
        }

        [HttpGet("getInventory/{warehouseId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<InventoryDto>>> GetInventory(int warehouseId)
        {
            try
            {
                if (_context.Inventory == null)
                {
                    return NotFound();
                }

                var inventory = await _context.Inventory.Where(i => i.WarehouseId == warehouseId).ToListAsync();
                var inventoryDtos = _mapper.Map<List<InventoryDto>>(inventory);

                return Ok(inventoryDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }
        }

        [NonAction]
        [CapSubscribe("onWarehouseUpdated")]
        public async Task PutWarehouse(Warehouse warehouseToUpdate)
        {
            var warehouse = await _context.Warehouses.Where(w => w.WarehouseId == warehouseToUpdate.Id).FirstOrDefaultAsync();
            
            if (warehouse == null)
            {
                throw new Exception("Warehouse not found.");
            }

            warehouse.Name = warehouseToUpdate.Name;
            warehouse.StreetAddress = warehouseToUpdate.StreetAddress;
            warehouse.City = warehouseToUpdate.City;
            warehouse.State = warehouseToUpdate.State;
            warehouse.ZipCode = warehouseToUpdate.ZipCode;
            warehouse.Country = warehouseToUpdate.Country;

            _context.Entry(warehouse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WarehouseExists(warehouseToUpdate.WarehouseId))
                {
                    throw new Exception("Warehouse not found.");
                }
                else
                {
                    throw;
                }
            }
        }

        [NonAction]
        [CapSubscribe("onWarehouseCreated")]
        public async Task PostWarehouse(Warehouse warehouse)
        {
            if (_context.Warehouses == null)
            {
                throw new Exception("Entity set 'ApplicationDbContext.Warehouses'  is null.");
            }

            var newWarehouse = new Warehouse()
            {
                WarehouseId = warehouse.Id,
                Name = warehouse.Name,
                StreetAddress = warehouse.StreetAddress,
                City = warehouse.City,
                State = warehouse.State,
                ZipCode = warehouse.ZipCode,
                Country = warehouse.Country
            };

            _context.Warehouses.Add(newWarehouse);
            await _context.SaveChangesAsync();
        }

        [NonAction]
        [CapSubscribe("onWarehouseDeleted")]
        public async Task DeleteWarehouse(Warehouse warehouseToDelete)
        {
            if (_context.Warehouses == null)
            {
                throw new Exception("Warehouse not found.");
            }
            var warehouse = await _context.Warehouses.Where(w => w.WarehouseId == warehouseToDelete.Id).FirstOrDefaultAsync();

            if (warehouse == null)
            {
                throw new Exception("Warehouse not found.");
            }

            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();
        }

        private bool WarehouseExists(int id)
        {
            return (_context.Warehouses?.Any(e => e.WarehouseId == id)).GetValueOrDefault();
        }
    }
}
