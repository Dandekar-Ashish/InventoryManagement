using InventoryManagement.DataTransferModel;

namespace InventoryManagement.Service.Validator
{
    public class BookingValidator
    {
        public static (bool isValid, string errorMessage) ValidateBooking(BookingRequest bookingRequest, MemberDto member, InventoryDto inventory)
        {
            if (member is null)
            {
                return  (false, $"Member with Id {bookingRequest.MemberId} not found.");
            }

            if (member.BookingCount >= 2)
            {
                return (false, $"Member {bookingRequest.MemberId} has reached maximum booking limit 2.");
            }

            if (inventory is null)
            {
                return (false, $"Inventory with Id {bookingRequest.InventoryId} not found.");
            }

            if (inventory.RemainingCount <= 0)
            {
                return (false, $"Inventory {bookingRequest.InventoryId} not found.");
            }
            return (true, "");
        }
    }
}
