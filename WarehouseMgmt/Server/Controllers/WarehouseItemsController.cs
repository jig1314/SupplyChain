using AutoMapper;
using DotNetCore.CAP;
using DotNetCore.CAP.AzureServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WarehouseMgmt.Server.Data;
using WarehouseMgmt.Server.Models;
using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICapPublisher _capBus;
        private readonly IMapper _mapper;

        public WarehouseItemsController(ApplicationDbContext context, ICapPublisher capPublisher, IMapper mapper)
        {
            _context = context;
            _capBus = capPublisher;
            _mapper = mapper;
        }

        // GET: api/WarehouseItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WarehouseItemDto>>> GetWarehouseItems()
        {
            try
            {
                var warehouseItems = await _context.WarehouseItems.ToListAsync();
                var warehouseItemDtos = _mapper.Map<List<WarehouseItemDto>>(warehouseItems);

                return Ok(warehouseItemDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }
        }

        // GET: api/WarehouseItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WarehouseItemDto>> GetWarehouseItem(int id)
        {
            try
            {
                if (_context.WarehouseItems == null)
                {
                    return NotFound();
                }

                var warehouseItem = await _context.WarehouseItems.Include(i => i.Warehouse)
                                    .Where(i => i.Id == id).FirstOrDefaultAsync();

                if (warehouseItem == null)
                {
                    return NotFound();
                }

                var warehouseItemDto = _mapper.Map<WarehouseItemDto>(warehouseItem);

                return Ok(warehouseItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }
        }

        // PUT: api/WarehouseItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWarehouseItem(int id, WarehouseItemDto warehouseItemDto)
        {
            if (id != warehouseItemDto.Id)
            {
                return BadRequest();
            }

            try
            {
                var warehouseItem = _mapper.Map<WarehouseItem>(warehouseItemDto);
                _context.Entry(warehouseItem).State = EntityState.Modified;

                using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: true))
                {
                    await _context.SaveChangesAsync();

                    Dictionary<string, string?> extraHeaders = new()
                    {
                        { AzureServiceBusHeaders.SessionId, "warehouse-mgmt" }
                    };

                    _capBus.Publish("onWarehouseItemUpdated", warehouseItemDto, extraHeaders);
                }

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WarehouseItemExists(id))
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

        [HttpPut("shipItems/{warehouseId}")]
        public async Task<IActionResult> PutWarehouseItem(int warehouseId, List<int> warehouseItemIds)
        {
            try
            {
                var warehouseItems = await _context.WarehouseItems.Where(i => warehouseItemIds.Contains(i.Id)).ToListAsync();
                foreach (var warehouseItem in warehouseItems)
                {
                    warehouseItem.WarehouseId = warehouseId;
                    _context.Entry(warehouseItem).State = EntityState.Modified;

                    using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: true))
                    {
                        await _context.SaveChangesAsync();

                        Dictionary<string, string?> extraHeaders = new()
                        {
                            { AzureServiceBusHeaders.SessionId, "warehouse-mgmt" }
                        };

                        _capBus.Publish("onWarehouseItemShipped", warehouseItem, extraHeaders);
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }
        }

        // POST: api/WarehouseItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WarehouseItem>> PostWarehouseItem(WarehouseItemDto warehouseItemDto)
        {
            try
            {
                using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: true))
                {
                    if (_context.WarehouseItems == null)
                    {
                        return Problem("Entity set 'ApplicationDbContext.WarehouseItems'  is null.");
                    }

                    var warehouseItem = _mapper.Map<WarehouseItem>(warehouseItemDto);
                    _context.WarehouseItems.Add(warehouseItem);
                    await _context.SaveChangesAsync();

                    Dictionary<string, string?> extraHeaders = new()
                    {
                        { AzureServiceBusHeaders.SessionId, "warehouse-mgmt" }
                    };

                    _capBus.Publish("onWarehouseItemReceived", warehouseItem, extraHeaders);

                    return CreatedAtAction("GetWarehouseItem", new { id = warehouseItem.Id }, warehouseItem);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }
        }

        // DELETE: api/WarehouseItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouseItem(int id)
        {
            try
            {
                if (_context.WarehouseItems == null)
                {
                    return NotFound();
                }

                var warehouseItem = await _context.WarehouseItems.FindAsync(id);

                if (warehouseItem == null)
                {
                    return NotFound();
                }

                using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: true))
                {
                    _context.WarehouseItems.Remove(warehouseItem);
                    await _context.SaveChangesAsync();

                    Dictionary<string, string?> extraHeaders = new()
                    {
                        { AzureServiceBusHeaders.SessionId, "warehouse-mgmt" }
                    };

                    _capBus.Publish("onWarehouseItemDeleted", warehouseItem, extraHeaders);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }
        }

        private bool WarehouseItemExists(int id)
        {
            return (_context.WarehouseItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
