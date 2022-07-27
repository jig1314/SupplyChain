using AutoMapper;
using WarehouseMgmt.Client.ViewModels;
using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Client.MappingProfiles
{
    public class WarehouseItemProfile : Profile
    {
        public WarehouseItemProfile()
        {
            CreateMap<WarehouseItemDto, WarehouseItemViewModel>()
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
                    opt => opt.MapFrom(src => src.Qty.ToString()))
                .ForMember(
                    dest => dest.WarehouseName,
                    opt => opt.MapFrom(src => src.Warehouse))
                .ForMember(
                    dest => dest.HasSerialNumber,
                    opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.SerialNumber)));

            CreateMap<WarehouseItemViewModel, WarehouseItemDto>()
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
                    opt => opt.MapFrom(src => int.Parse(src.Qty)));
        }
    }
}
