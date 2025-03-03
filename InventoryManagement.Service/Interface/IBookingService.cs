using InventoryManagement.DataTransferModel;

namespace InventoryManagement.Service.Interface
{
    public interface IBookingService
    {
        Task<(bool isSuccess, BookingDto? bookingDto, string errorMessage)> AddBooking(BookingRequest bookingRequest);
        Task<IEnumerable<BookingDto>> GetAllBookings();
        Task<BookingDto> GetBookingById(int Id);
        Task<(bool isSuccess, BookingDto? bookingDto, string errorMessage)> CancelBooking(int Id);
    }
}
