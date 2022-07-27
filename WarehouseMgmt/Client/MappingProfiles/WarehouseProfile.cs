using AutoMapper;
using WarehouseMgmt.Client.ViewModels;
using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Client.MappingProfiles
{
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            CreateMap<WarehouseDto, WarehouseViewModel>()
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

            CreateMap<WarehouseViewModel, WarehouseDto>()
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
