using AutoMapper;
using DotNetCore.CAP;
using InventoryMgmt.Server.Data;
using InventoryMgmt.Server.Models;
using InventoryMgmt.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryMgmt.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public WarehouseItemsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [NonAction]
        [CapSubscribe("onWarehouseItemUpdated")]
        public async Task PutInventory(WarehouseItemDto warehouseItemDto)
        {
            var warehouseItem = await _context.WarehouseItems.Where(i => i.WarehouseItemId == warehouseItemDto.Id).FirstOrDefaultAsync();
            
            if (warehouseItem == null)
            {
                throw new Exception("Inventory record not found.");
            }

            var warehouse = await _context.Warehouses.Where(w => w.WarehouseId == warehouseItemDto.WarehouseId).FirstOrDefaultAsync();

            if (warehouse == null)
            {
                throw new Exception("Warehouse not found.");
            }

            var inventoryWarehouseItem = await _context.InventoryWarehouseItems.Where(i => i.WarehouseItemId == warehouseItem.Id).FirstOrDefaultAsync();
            
            if (inventoryWarehouseItem == null)
            {
                throw new Exception("Inventory record not found.");
            }

            var inventory = await _context.Inventory.Where(i => i.Id == inventoryWarehouseItem.InventoryId).FirstOrDefaultAsync();

            if (inventory == null)
            {
                throw new Exception("Inventory record not found.");
            }

            var inventorySerial = await _context.InventorySerials.Where(i => i.InventoryWarehouseItemId == inventoryWarehouseItem.Id).FirstOrDefaultAsync();

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    if (inventory.PartNumber != warehouseItemDto.PartNumber ||
                        inventory.Description != warehouseItemDto.Description)
                    {
                        inventory.Qty -= warehouseItem.Qty;
                        _context.Entry(inventory).State = EntityState.Modified;
                        _context.InventoryWarehouseItems.Remove(inventoryWarehouseItem);

                        if (inventorySerial != null)
                        {
                            _context.InventorySerials.Remove(inventorySerial);
                        }

                        await _context.SaveChangesAsync();

                        inventory = await _context.Inventory.Include(i => i.Warehouse)
                            .Where(i => i.WarehouseId == warehouse.Id
                                && i.PartNumber == warehouseItemDto.PartNumber
                                && i.Description == warehouseItemDto.Description).FirstOrDefaultAsync();

                        if (inventory == null)
                        {
                            inventory = new Inventory()
                            {
                                WarehouseId = warehouse.Id,
                                PartNumber = warehouseItemDto.PartNumber,
                                Description = warehouseItemDto.Description,
                                Qty = warehouseItemDto.Qty
                            };

                            _context.Inventory.Add(inventory);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            inventory.Qty += warehouseItemDto.Qty;

                            _context.Entry(inventory).State = EntityState.Modified;
                        }

                        warehouseItem.StorageLocation = warehouseItemDto.StorageLocation;
                        warehouseItem.PartNumber = warehouseItemDto.PartNumber;
                        warehouseItem.Description = warehouseItemDto.Description;
                        warehouseItem.SerialNumber = warehouseItemDto.SerialNumber;
                        warehouseItem.Qty = warehouseItemDto.Qty;

                        _context.Entry(warehouseItem).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        inventoryWarehouseItem = new InventoryWarehouseItem()
                        {
                            InventoryId = inventory.Id,
                            WarehouseItemId = warehouseItem.Id,
                            StorageLocation = warehouseItem.StorageLocation
                        };

                        _context.InventoryWarehouseItems.Add(inventoryWarehouseItem);
                        await _context.SaveChangesAsync();

                        if (!string.IsNullOrWhiteSpace(warehouseItem.SerialNumber))
                        {
                            inventorySerial = new InventorySerial()
                            {
                                InventoryWarehouseItemId = inventoryWarehouseItem.Id,
                                SerialNumber = warehouseItem.SerialNumber
                            };

                            _context.InventorySerials.Add(inventorySerial);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else if (warehouseItem.Qty != warehouseItemDto.Qty)
                    {
                        inventory.Qty -= (warehouseItem.Qty - warehouseItemDto.Qty);
                        warehouseItem.Qty = warehouseItemDto.Qty;

                        _context.Entry(inventory).State = EntityState.Modified;
                        _context.Entry(warehouseItem).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }

                    if(warehouseItem.StorageLocation != warehouseItemDto.StorageLocation)
                    {
                        warehouseItem.StorageLocation = warehouseItemDto.StorageLocation;
                        inventoryWarehouseItem.StorageLocation = warehouseItemDto.StorageLocation;

                        _context.Entry(warehouseItem).State = EntityState.Modified;
                        _context.Entry(inventoryWarehouseItem).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }

                    if (warehouseItem.SerialNumber != warehouseItemDto.SerialNumber)
                    {
                        if (string.IsNullOrWhiteSpace(warehouseItemDto.SerialNumber))
                        {
                            if (inventorySerial != null)
                            {
                                _context.InventorySerials.Remove(inventorySerial);
                            }
                        }
                        else
                        {
                            if (inventorySerial != null)
                            {
                                inventorySerial.SerialNumber = warehouseItemDto.SerialNumber;
                                _context.Entry(inventorySerial).State = EntityState.Modified;
                            }
                            else
                            {
                                inventorySerial = new InventorySerial()
                                {
                                    InventoryWarehouseItemId = inventoryWarehouseItem.Id,
                                    SerialNumber = warehouseItem.SerialNumber
                                };

                                _context.InventorySerials.Add(inventorySerial);
                            }
                        }

                        warehouseItem.SerialNumber = warehouseItemDto.SerialNumber;
                        _context.Entry(warehouseItem).State = EntityState.Modified;

                        await _context.SaveChangesAsync();
                    }

                    await trans.CommitAsync();
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                }
            }
        }

        [NonAction]
        [CapSubscribe("onWarehouseItemShipped")]
        public async Task MoveInventory(WarehouseItemDto warehouseItemDto)
        {
            var warehouseItem = await _context.WarehouseItems.Where(i => i.WarehouseItemId == warehouseItemDto.Id).FirstOrDefaultAsync();

            if (warehouseItem == null)
            {
                throw new Exception("Inventory record not found.");
            }

            var warehouse = await _context.Warehouses.Where(w => w.WarehouseId == warehouseItemDto.WarehouseId).FirstOrDefaultAsync();

            if (warehouse == null)
            {
                throw new Exception("Warehouse not found.");
            }

            var inventoryWarehouseItem = await _context.InventoryWarehouseItems.Where(i => i.WarehouseItemId == warehouseItem.Id).FirstOrDefaultAsync();

            if (inventoryWarehouseItem == null)
            {
                throw new Exception("Inventory record not found.");
            }

            var inventory = await _context.Inventory.Where(i => i.Id == inventoryWarehouseItem.InventoryId).FirstOrDefaultAsync();

            if (inventory == null)
            {
                throw new Exception("Inventory record not found.");
            }

            var inventorySerial = await _context.InventorySerials.Where(i => i.InventoryWarehouseItemId == inventoryWarehouseItem.Id).FirstOrDefaultAsync();

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    inventory.Qty -= warehouseItem.Qty;
                    _context.Entry(inventory).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    inventory = await _context.Inventory.Include(i => i.Warehouse)
                            .Where(i => i.WarehouseId == warehouse.Id
                                && i.PartNumber == warehouseItemDto.PartNumber
                                && i.Description == warehouseItemDto.Description).FirstOrDefaultAsync();

                    if (inventory == null)
                    {
                        inventory = new Inventory()
                        {
                            WarehouseId = warehouse.Id,
                            PartNumber = warehouseItemDto.PartNumber,
                            Description = warehouseItemDto.Description,
                            Qty = warehouseItemDto.Qty
                        };

                        _context.Inventory.Add(inventory);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        inventory.Qty += warehouseItemDto.Qty;

                        _context.Entry(inventory).State = EntityState.Modified;
                    }

                    inventoryWarehouseItem.InventoryId = inventory.Id;
                    _context.Entry(inventoryWarehouseItem).State = EntityState.Modified;

                    warehouseItem.WarehouseId = warehouse.Id;
                    _context.Entry(warehouseItem).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    await trans.CommitAsync();
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                }
            }
        }

        [NonAction]
        [CapSubscribe("onWarehouseItemReceived")]
        public async Task ReceiveInventory(WarehouseItemDto warehouseItemDto)
        {
            if (_context.Warehouses == null || _context.Inventory == null || _context.InventorySerials == null)
            {
                throw new Exception("Entity set is null.");
            }

            var warehouse = await _context.Warehouses.Where(w => w.WarehouseId == warehouseItemDto.WarehouseId).FirstOrDefaultAsync();
            
            if (warehouse == null)
            {
                throw new Exception("Warehouse not found.");
            }
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var inventory = await _context.Inventory.Include(i => i.Warehouse)
                        .Where(i => i.WarehouseId == warehouse.Id
                            && i.PartNumber == warehouseItemDto.PartNumber
                            && i.Description == warehouseItemDto.Description).FirstOrDefaultAsync();

                    if (inventory == null)
                    {
                        inventory = new Inventory()
                        {
                            WarehouseId = warehouse.Id,
                            PartNumber = warehouseItemDto.PartNumber,
                            Description = warehouseItemDto.Description,
                            Qty = warehouseItemDto.Qty
                        };

                        _context.Inventory.Add(inventory);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        inventory.Qty += warehouseItemDto.Qty;

                        _context.Entry(inventory).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }

                    var warehouseItem = _mapper.Map<WarehouseItem>(warehouseItemDto);
                    warehouseItem.WarehouseId = warehouse.Id;

                    _context.WarehouseItems.Add(warehouseItem);
                    await _context.SaveChangesAsync();

                    var inventoryWarehouseItem = new InventoryWarehouseItem()
                    {
                        InventoryId = inventory.Id,
                        WarehouseItemId = warehouseItem.Id,
                        StorageLocation = warehouseItem.StorageLocation
                    };

                    _context.InventoryWarehouseItems.Add(inventoryWarehouseItem);
                    await _context.SaveChangesAsync();

                    if (!string.IsNullOrWhiteSpace(warehouseItem.SerialNumber))
                    {
                        var inventorySerial = new InventorySerial()
                        {
                            InventoryWarehouseItemId = inventoryWarehouseItem.Id,
                            SerialNumber = warehouseItem.SerialNumber
                        };

                        _context.InventorySerials.Add(inventorySerial);
                        await _context.SaveChangesAsync();
                    }

                    await trans.CommitAsync();
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                }
            }
        }

        [NonAction]
        [CapSubscribe("onWarehouseItemDeleted")]
        public async Task DeleteInventory(WarehouseItemDto warehouseItemDto)
        {
            var warehouseItem = await _context.WarehouseItems.Where(i => i.WarehouseItemId == warehouseItemDto.Id).FirstOrDefaultAsync();

            if (warehouseItem == null)
            {
                throw new Exception("Inventory record not found.");
            }

            var inventoryWarehouseItem = await _context.InventoryWarehouseItems.Where(i => i.WarehouseItemId == warehouseItem.Id).FirstOrDefaultAsync();

            if (inventoryWarehouseItem == null)
            {
                throw new Exception("Inventory record not found.");
            }

            var inventory = await _context.Inventory.Where(i => i.Id == inventoryWarehouseItem.InventoryId).FirstOrDefaultAsync();

            if (inventory == null)
            {
                throw new Exception("Inventory record not found.");
            }

            var inventorySerial = await _context.InventorySerials.Where(i => i.InventoryWarehouseItemId == inventoryWarehouseItem.Id).FirstOrDefaultAsync();

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    inventory.Qty -= warehouseItem.Qty;
                    _context.Entry(inventory).State = EntityState.Modified;

                    _context.InventoryWarehouseItems.Remove(inventoryWarehouseItem);
                    if (inventorySerial != null)
                    {
                        _context.InventorySerials.Remove(inventorySerial);
                    }
                    _context.WarehouseItems.Remove(warehouseItem);

                    await _context.SaveChangesAsync();

                    await trans.CommitAsync();
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                }
            }
        }
    }
}
