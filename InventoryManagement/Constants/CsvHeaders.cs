namespace InventoryManagement.Constants
{
    public class CsvHeaders
    {
        internal static readonly string[] MembersCsvHeaders = new[] { "name", "surname", "booking_count", "date_joined" };
        internal static readonly string[] InventoryCsvHeaders = new[] { "title", "description", "remaining_count", "expiration_date" };
    }
}
