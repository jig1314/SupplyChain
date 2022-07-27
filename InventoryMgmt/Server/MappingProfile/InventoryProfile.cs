using AutoMapper;
using InventoryMgmt.Server.Models;
using InventoryMgmt.Shared.DTOs;

namespace InventoryMgmt.Server.MappingProfile
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<Inventory, InventoryDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.PartNumber,
                    opt => opt.MapFrom(src => src.PartNumber))
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description))
                .ForMember(
                    dest => dest.Qty,
                    opt => opt.MapFrom(src => src.Qty))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
