using InventoryManagement.Repository.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Repository.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int InventoryId { get; set; }
        public DateTime BookingDatetime { get; set; }
        public BookingStatus Status { get; set; }
        public Member Member { get; set; }
        public Inventory Inventory { get; set; }
    }
}
