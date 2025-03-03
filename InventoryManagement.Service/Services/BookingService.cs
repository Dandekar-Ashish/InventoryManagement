using AutoMapper;
using InventoryManagement.DataTransferModel;
using InventoryManagement.Repository.Interface;
using InventoryManagement.Repository.Models;
using InventoryManagement.Repository.Models.Enums;
using InventoryManagement.Service.Interface;
using InventoryManagement.Service.Validator;

namespace InventoryManagement.Service.Services
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IMemberService _memberService;
        private readonly IInventoryService _inventoryService;
        private readonly IUnitOfWork _unitOfWork;


        public BookingService(IGenericRepository<Booking> bookingRepository, IMemberService memberService, IInventoryService inventoryService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _bookingRepository = bookingRepository;
            _memberService = memberService;
            _inventoryService = inventoryService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<BookingDto>> GetAllBookings()
        {
            var bookings = await _bookingRepository.GetAllAsync(o => o.Member, o => o.Inventory);
            var bookingDto = _mapper.Map<IEnumerable<BookingDto>>(bookings);
            return bookingDto;
        }
        public async Task<BookingDto> GetBookingById(int Id)
        {
            var bookings = await _bookingRepository.GetByIdAsync(Id, o => o.Member, o => o.Inventory);
            var bookingDto = _mapper.Map<BookingDto>(bookings);
            return bookingDto;
        }
        public async Task<(bool isSuccess, BookingDto? bookingDto, string errorMessage)> AddBooking(BookingRequest bookingRequest)
        {
            var memberDto = await _memberService.GetMemberById(bookingRequest.MemberId);
            var inventoryDto = await _inventoryService.GetInventoryById(bookingRequest.InventoryId);

            var (isValid, validationMessage) = BookingValidator.ValidateBooking(bookingRequest, memberDto, inventoryDto);

            if (isValid)
            {
                await _unitOfWork.BeginTransactionAsync();

                memberDto.BookingCount += 1;
                var member = _mapper.Map<Member>(memberDto);
                await _unitOfWork.MemberRepository.UpdateAsync(member);

                inventoryDto.RemainingCount -= 1;
                var inventory = _mapper.Map<Inventory>(inventoryDto);
                await _unitOfWork.InventoryRepository.UpdateAsync(inventory);

                Booking booking = new Booking
                {
                    InventoryId = bookingRequest.InventoryId,
                    MemberId = bookingRequest.MemberId,
                    BookingDatetime = DateTime.Now,
                    Status = BookingStatus.Active
                };

                var result = await _unitOfWork.BookingRepository.AddAsync(booking);

                booking = await _bookingRepository.GetByIdAsync(result.Id, o => o.Member, o => o.Inventory);
                BookingDto bookingDto = _mapper.Map<BookingDto>(booking);
                
                return (true, bookingDto, "");
            }
            else
            {
                return (false, null, validationMessage);
            }

        }

        public async Task<(bool isSuccess, BookingDto? bookingDto, string errorMessage)> CancelBooking(int Id)
        {
            var booking = await _bookingRepository.GetByIdAsync(Id);

            if (booking.Status == BookingStatus.Cancelled)
            {
                return (false, null, "No active booking. Booking already cancelled.");
            }
            booking.Status = BookingStatus.Cancelled;
            await _bookingRepository.UpdateAsync(booking);

            booking = await _bookingRepository.GetByIdAsync(Id, o => o.Member, o => o.Inventory);
            BookingDto bookingDto = _mapper.Map<BookingDto>(booking);

            return (true, bookingDto, "");
        }
    }
}
