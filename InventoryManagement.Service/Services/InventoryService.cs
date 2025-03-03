using AutoMapper;
using InventoryManagement.DataTransferModel;
using InventoryManagement.Repository.Interface;
using InventoryManagement.Repository.Models;
using InventoryManagement.Service.Interface;

namespace InventoryManagement.Service.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Inventory> _inventoryRepository;

        public InventoryService(IGenericRepository<Inventory> inventoryRepository, IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }
        public async Task<InventoryDto> AddInventory(InventoryDto Inventorydto)
        {
            var Inventory = _mapper.Map<Inventory>(Inventorydto);
            Inventory = await _inventoryRepository.AddAsync(Inventory);
            Inventorydto = _mapper.Map<InventoryDto>(Inventory);
            return Inventorydto;
        }

        public async Task<IEnumerable<InventoryDto>> AddInventories(IEnumerable<InventoryDto> inventoryDtos)
        {
            var Inventorys = _mapper.Map<IEnumerable<Inventory>>(inventoryDtos);
            Inventorys = await _inventoryRepository.AddRangeAsync(Inventorys);
            inventoryDtos = _mapper.Map<IEnumerable<InventoryDto>>(Inventorys);
            return inventoryDtos;
        }

        public async Task<IEnumerable<InventoryDto>> GetAllInventory()
        {
            var Inventorys = await _inventoryRepository.GetAllAsync();
            var InventorysDto = _mapper.Map<IEnumerable<InventoryDto>>(Inventorys);
            return InventorysDto;
        }

        public async Task<InventoryDto> GetInventoryById(int id)
        {
            var Inventory = await _inventoryRepository.GetByIdAsync(id);
            var InventorysDto = _mapper.Map<InventoryDto>(Inventory);
            return InventorysDto;
        }

        public async Task<InventoryDto> UpdateInventory(InventoryDto inventoryDto)
        {
            var Inventory = _mapper.Map<Inventory>(inventoryDto);
            Inventory = await _inventoryRepository.UpdateAsync(Inventory);
            inventoryDto = _mapper.Map<InventoryDto>(Inventory);
            return inventoryDto;
        }
    }
}
