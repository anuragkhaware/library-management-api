using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Repositories;

namespace LibraryManagementAPI.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;
        private readonly IBookRepository _bookRepository;

        public BorrowService(IBorrowRepository borrowRepository, IBookRepository bookRepository)
        {
            _borrowRepository = borrowRepository;
            _bookRepository = bookRepository;
        }

        public BorrowRecord? Borrow(BorrowDto borrowDto)
        {
            // Check if book exists and is available
            var book = _bookRepository.GetById(borrowDto.BookId);
            if (book == null || !book.IsAvailable) return null;

            var record = new BorrowRecord
            {
                BookId = borrowDto.BookId,
                MemberId = borrowDto.MemberId,
                BorrowedAt = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(borrowDto.DueDays),
                IsReturned = false
            };

            return _borrowRepository.Borrow(record);
        }

        public BorrowRecord? Return(int borrowId) => _borrowRepository.Return(borrowId);

        public List<BorrowRecord> GetOverdue() => _borrowRepository.GetOverdue();

        public List<BorrowRecord> GetByMember(int memberId) =>
            _borrowRepository.GetByMember(memberId);
    }
}