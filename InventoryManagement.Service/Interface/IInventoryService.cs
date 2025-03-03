using InventoryManagement.DataTransferModel;

namespace InventoryManagement.Service.Interface
{
    public interface IInventoryService
    {
        Task<InventoryDto> AddInventory(InventoryDto inventoryDto);
        Task<InventoryDto> UpdateInventory(InventoryDto inventoryDto);
        Task<InventoryDto> GetInventoryById(int Id);
        Task<IEnumerable<InventoryDto>> GetAllInventory();
        Task<IEnumerable<InventoryDto>> AddInventories(IEnumerable<InventoryDto> inventoryDtos);
    }
}
