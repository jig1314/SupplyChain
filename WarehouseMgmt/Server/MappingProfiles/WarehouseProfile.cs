using AutoMapper;
using WarehouseMgmt.Server.Models;
using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Server.MappingProfiles
{
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            CreateMap<Warehouse, WarehouseDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(
                    dest => dest.StreetAddress,
                    opt => opt.MapFrom(src => src.StreetAddress))
                .ForMember(
                    dest => dest.City,
                    opt => opt.MapFrom(src => src.City))
                .ForMember(
                    dest => dest.State,
                    opt => opt.MapFrom(src => src.State))
                .ForMember(
                    dest => dest.ZipCode,
                    opt => opt.MapFrom(src => src.ZipCode))
                .ForMember(
                    dest => dest.Country,
                    opt => opt.MapFrom(src => src.Country));

            CreateMap<WarehouseDto, Warehouse>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(
                    dest => dest.StreetAddress,
                    opt => opt.MapFrom(src => src.StreetAddress))
                .ForMember(
                    dest => dest.City,
                    opt => opt.MapFrom(src => src.City))
                .ForMember(
                    dest => dest.State,
                    opt => opt.MapFrom(src => src.State))
                .ForMember(
                    dest => dest.ZipCode,
                    opt => opt.MapFrom(src => src.ZipCode))
                .ForMember(
                    dest => dest.Country,
                    opt => opt.MapFrom(src => src.Country));
        }
    }
}
