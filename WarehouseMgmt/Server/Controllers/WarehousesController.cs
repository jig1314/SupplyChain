using AutoMapper;
using DotNetCore.CAP;
using DotNetCore.CAP.AzureServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseMgmt.Server.Data;
using WarehouseMgmt.Server.Models;
using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICapPublisher _capBus;
        private readonly IMapper _mapper;

        public WarehousesController(ApplicationDbContext context, ICapPublisher capPublisher, IMapper mapper)
        {
            _context = context;
            _capBus = capPublisher;
            _mapper = mapper;
        }

        // GET: api/Warehouses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WarehouseDto>>> GetWarehouses()
        {
            try
            {
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
            try
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

                return Ok(warehouseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }
        }

        // GET: api/Warehouses/GetWarehouseItems
        [HttpGet("getWarehouseItems/{warehouseId}")]
        public async Task<ActionResult<IEnumerable<WarehouseItemDto>>> GetWarehouseItems(string warehouseId)
        {
            try
            {
                var warehouseItems = await _context.WarehouseItems.Where(i => i.WarehouseId == int.Parse(warehouseId)).ToListAsync();
                var warehouseItemDtos = _mapper.Map<List<WarehouseItemDto>>(warehouseItems);

                return Ok(warehouseItemDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }
        }

        // PUT: api/Warehouses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWarehouse(int id, WarehouseDto warehouseDto)
        {
            if (id != warehouseDto.Id)
            {
                return BadRequest();
            }

            try
            {
                var warehouse = _mapper.Map<Warehouse>(warehouseDto);
                _context.Entry(warehouse).State = EntityState.Modified;

                using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: true))
                {
                    await _context.SaveChangesAsync();

                    Dictionary<string, string?> extraHeaders = new()
                    {
                        { AzureServiceBusHeaders.SessionId, "warehouse-mgmt" }
                    };

                    _capBus.Publish("onWarehouseUpdated", warehouse, extraHeaders);
                }

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WarehouseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }
        }

        // POST: api/Warehouses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WarehouseDto>> PostWarehouse(WarehouseDto warehouseDto)
        {
            try
            {
                using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: true))
                {
                    if (_context.Warehouses == null)
                    {
                        return Problem("Entity set 'ApplicationDbContext.Warehouses'  is null.");
                    }

                    var warehouse = _mapper.Map<Warehouse>(warehouseDto);
                    _context.Warehouses.Add(warehouse);
                    await _context.SaveChangesAsync();

                    Dictionary<string, string?> extraHeaders = new()
                    {
                        { AzureServiceBusHeaders.SessionId, "warehouse-mgmt" }
                    };

                    _capBus.Publish("onWarehouseCreated", warehouse, extraHeaders);

                    return CreatedAtAction("GetWarehouse", new { id = warehouse.Id }, warehouse);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }
        }

        // DELETE: api/Warehouses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            try
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

                using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: true))
                {
                    _context.Warehouses.Remove(warehouse);
                    await _context.SaveChangesAsync();

                    Dictionary<string, string?> extraHeaders = new()
                    {
                        { AzureServiceBusHeaders.SessionId, "warehouse-mgmt" }
                    };

                    _capBus.Publish("onWarehouseDeleted", warehouse, extraHeaders);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }
        }

        private bool WarehouseExists(int id)
        {
            return (_context.Warehouses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
