using AutoMapper;
using InventoryManagement.DataTransferModel;
using InventoryManagement.Repository.Interface;
using InventoryManagement.Repository.Models;
using InventoryManagement.Service.Interface;

namespace InventoryManagement.Service.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Member> _memberRepository;

        public MemberService(IGenericRepository<Member> memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }
        public async Task<MemberDto> AddMember(MemberDto memberdto)
        {
            var member = _mapper.Map<Member>(memberdto);
            member = await _memberRepository.AddAsync(member);
            memberdto = _mapper.Map<MemberDto>(member);
            return memberdto;
        }

        public async Task<IEnumerable<MemberDto>> AddMembers(IEnumerable<MemberDto> membersDto)
        {
            var members = _mapper.Map<IEnumerable<Member>>(membersDto);
            members = await _memberRepository.AddRangeAsync(members);
            membersDto = _mapper.Map<IEnumerable<MemberDto>>(members);
            return membersDto;
        }

        public async Task<IEnumerable<MemberDto>> GetAllMember()
        {
            var members = await _memberRepository.GetAllAsync();
            var membersDto = _mapper.Map<IEnumerable<MemberDto>>(members);
            return membersDto;
        }

        public async Task<MemberDto> GetMemberById(int Id)
        {
            var member = await _memberRepository.GetByIdAsync(Id);
            var membersDto = _mapper.Map<MemberDto>(member);
            return membersDto;
        }

        public async Task<MemberDto> UpdateMember(MemberDto memberDto)
        {
            var member = _mapper.Map<Member>(memberDto);
            member = await _memberRepository.UpdateAsync(member);
            memberDto = _mapper.Map<MemberDto>(member);
            return memberDto;
        }
    }
}
