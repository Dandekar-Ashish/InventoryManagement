using InventoryManagement.DataTransferModel;
using InventoryManagement.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookingService _bookingService;
        public BookController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var bookingDtos = await _bookingService.GetAllBookings();
            return Ok(bookingDtos);
        }

        [HttpGet("{bookingId}")]
        public async Task<ActionResult> Get(int bookingId)
        {
            var bookingDto = await _bookingService.GetBookingById(bookingId);
            return Ok(bookingDto);
        }

        [HttpPost]
        public async Task<ActionResult> Create(BookingRequest bookingRequest)
        {
            var result = await _bookingService.AddBooking(bookingRequest);
            
            if (!result.isSuccess)
            {
                return BadRequest(result.errorMessage);
            }

            return Ok(result.bookingDto);
        }

        [HttpPut]
        [Route("cancel/{bookingId}")]
        public async Task<IActionResult> Cancel(int bookingId)
        {
            var result = await _bookingService.CancelBooking(bookingId);
            
            if (!result.isSuccess)
            {
                return BadRequest(result.errorMessage);
            }

            return Ok(result.bookingDto);
        }
    }
}
