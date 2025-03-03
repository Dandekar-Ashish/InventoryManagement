namespace InventoryManagement.DataTransferModel
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public int InventoryId { get; set; }
        public string InventoryDescription { get; set; }
        public DateTime BookingDateTime { get; set; }
        public string Status { get; set; }
    }
}
