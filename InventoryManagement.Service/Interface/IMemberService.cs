using InventoryManagement.DataTransferModel;

namespace InventoryManagement.Service.Interface
{
    public interface IMemberService
    {
        Task<MemberDto> AddMember(MemberDto member);
        Task<MemberDto> UpdateMember(MemberDto member);
        Task<MemberDto> GetMemberById(int Id);
        Task<IEnumerable<MemberDto>> GetAllMember();
        Task<IEnumerable<MemberDto>> AddMembers(IEnumerable<MemberDto> members);
    }
}
