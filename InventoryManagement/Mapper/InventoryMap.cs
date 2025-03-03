using CsvHelper.Configuration;
using InventoryManagement.DataTransferModel;

namespace InventoryManagement.Mapper
{
    public class InventoryMap : ClassMap<InventoryDto>
    {
        public InventoryMap()
        {
            Map(m => m.Title).Name("title");
            Map(m => m.Description).Name("description");
            Map(m => m.RemainingCount).Name("remaining_count");
            Map(m => m.ExpirationDate).Name("expiration_date").TypeConverterOption.Format("dd/MM/yyyy");
        }
    }
}
