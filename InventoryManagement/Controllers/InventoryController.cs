using CsvHelper;
using InventoryManagement.Constants;
using InventoryManagement.DataTransferModel;
using InventoryManagement.Mapper;
using InventoryManagement.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace InventoryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var inventoryDtos = await _inventoryService.GetAllInventory();
            return Ok(inventoryDtos);
        }

        [HttpGet("{inventoryId}")]
        public async Task<ActionResult> Get(int inventoryId)
        {
            var inventory = await _inventoryService.GetInventoryById(inventoryId);
            return Ok(inventory);
        }

        [HttpPost]
        public async Task<ActionResult> Create(InventoryDto inventoryDto)
        {
            var member = await _inventoryService.AddInventory(inventoryDto);
            return Ok(member);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadInventory(IFormFile file)
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
                    csv.Context.RegisterClassMap<InventoryMap>();

                    var headers = csv.HeaderRecord;

                    if (!headers.SequenceEqual(CsvHeaders.InventoryCsvHeaders))
                    {
                        return BadRequest("CSV headers are incorrect. Expected headers: " + string.Join(", ", CsvHeaders.MembersCsvHeaders));
                    }
                    var records = csv.GetRecords<InventoryDto>().ToList();

                    if (records.Any())
                    {
                        await _inventoryService.AddInventories(records);
                    }
                    else
                    {
                        return BadRequest("No records found for uploading Inventories.");
                    }

                    return Ok(new { message = $"{records.Count} Inventories successfully uploaded." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error parsing the file: {ex.Message}");
            }
        }
    }
}
