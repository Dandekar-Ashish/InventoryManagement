using CsvHelper.Configuration;
using InventoryManagement.DataTransferModel;

namespace InventoryManagement.Mapper
{
    public class MembersMap : ClassMap<MemberDto>
    {
        public MembersMap()
        {
            Map(m => m.Name).Name("name");
            Map(m => m.Surname).Name("surname");
            Map(m => m.BookingCount).Name("booking_count");
            Map(m => m.DateJoined).Name("date_joined");
        }
    }
}
