using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Repositories
{
    public interface IBorrowRepository
    {
        BorrowRecord Borrow(BorrowRecord record);
        BorrowRecord? Return(int borrowId);
        List<BorrowRecord> GetOverdue();
        List<BorrowRecord> GetByMember(int memberId);
    }
}