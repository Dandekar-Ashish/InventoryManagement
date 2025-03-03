using InventoryManagement.Repository.Interface;
using InventoryManagement.Repository.Models;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Member> MemberRepository { get; }
    IGenericRepository<Inventory> InventoryRepository { get; }
    IGenericRepository<Booking> BookingRepository { get; }
    Task<int> SaveAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
