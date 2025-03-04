using CsvHelper;
using InventoryManagement.Constants;
using InventoryManagement.DataTransferModel;
using InventoryManagement.Mapper;
using InventoryManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Globalization;

namespace InventoryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var members = await _memberService.GetAllMember();
            return Ok(members);
        }

        [HttpGet("{memberId}")]
        public async Task<ActionResult> Get(int memberId)
        {
            var member = await _memberService.GetMemberById(memberId);
            return Ok(member);
        }

        [HttpPost]
        public async Task<ActionResult> Create(MemberDto memberDto)
        {
            var member = await _memberService.AddMember(memberDto);
            return Ok(member);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadMembers(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(stream, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();
                    csv.Context.RegisterClassMap<MembersMap>();

                    var headers = csv.HeaderRecord;

                    if (!headers.SequenceEqual(CsvHeaders.MembersCsvHeaders))
                    {
                        return BadRequest("CSV headers are incorrect. Expected headers: " + string.Join(", ", CsvHeaders.MembersCsvHeaders));
                    }

                    var records = csv.GetRecords<MemberDto>().ToList();

                    if (records.Any())
                    {
                        await _memberService.AddMembers(records);
                    }
                    else
                    {
                        return BadRequest("No records found for uploading members.");
                    }

                    return Ok(new { message = $"{records.Count} Members successfully uploaded." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error parsing the file: {ex.Message}");
            }
        }
    }
}
