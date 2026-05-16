using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Services
{
    public interface IBorrowService
    {
        BorrowRecord? Borrow(BorrowDto borrowDto);
        BorrowRecord? Return(int borrowId);
        List<BorrowRecord> GetOverdue();
        List<BorrowRecord> GetByMember(int memberId);
    }
}

