using InventoryManagement.Repository.Database;
using InventoryManagement.Repository.Interface;
using InventoryManagement.Repository.Models;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IGenericRepository<Member> MemberRepository { get; }
    public IGenericRepository<Inventory> InventoryRepository { get; }
    public IGenericRepository<Booking> BookingRepository { get; }

    public UnitOfWork(ApplicationDbContext context, IGenericRepository<Member> memberRepository, IGenericRepository<Inventory> inventoryRepository, IGenericRepository<Booking> bookingRepository)
    {
        _context = context;
        MemberRepository = memberRepository;
        InventoryRepository = inventoryRepository;
        BookingRepository = bookingRepository;
    }

    public Task BeginTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task RollbackAsync()
    {
        return Task.CompletedTask;
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
