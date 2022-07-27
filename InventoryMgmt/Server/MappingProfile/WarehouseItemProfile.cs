using AutoMapper;
using InventoryMgmt.Server.Models;
using InventoryMgmt.Shared.DTOs;

namespace InventoryMgmt.Server.MappingProfile
{
    public class WarehouseItemProfile : Profile
    {
        public WarehouseItemProfile()
        {
            CreateMap<WarehouseItemDto, WarehouseItem>()
                .ForMember(
                    dest => dest.WarehouseItemId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.StorageLocation,
                    opt => opt.MapFrom(src => src.StorageLocation))
                .ForMember(
                    dest => dest.PartNumber,
                    opt => opt.MapFrom(src => src.PartNumber))
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description))
                .ForMember(
                    dest => dest.SerialNumber,
                    opt => opt.MapFrom(src => src.SerialNumber))
                .ForMember(
                    dest => dest.Qty,
                    opt => opt.MapFrom(src => src.Qty))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
