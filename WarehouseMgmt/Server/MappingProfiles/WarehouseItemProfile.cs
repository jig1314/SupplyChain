using AutoMapper;
using WarehouseMgmt.Server.Models;
using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Server.MappingProfiles
{
    public class WarehouseItemProfile : Profile
    {
        public WarehouseItemProfile()
        {
            CreateMap<WarehouseItem, WarehouseItemDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.WarehouseId,
                    opt => opt.MapFrom(src => src.WarehouseId))
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
                .ForMember(
                    dest => dest.Warehouse,
                    opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.Name : ""));

            CreateMap<WarehouseItemDto, WarehouseItem>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.WarehouseId,
                    opt => opt.MapFrom(src => src.WarehouseId))
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
