using AutoMapper;
using InventoryManagement.DataTransferModel;
using InventoryManagement.Repository.Models;
using Member = InventoryManagement.Repository.Models.Member;

namespace InventoryManagement.Service.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Member, MemberDto>();
            CreateMap<MemberDto, Member>();

            CreateMap<InventoryDto, Inventory>();
            CreateMap<Inventory, InventoryDto>();

            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.InventoryDescription, opt => opt.MapFrom(src => src.Inventory.Description))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<BookingDto, Booking>();
        }
    }
}
