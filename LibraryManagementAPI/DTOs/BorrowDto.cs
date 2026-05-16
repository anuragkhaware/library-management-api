namespace LibraryManagementAPI.DTOs
{
    public class BorrowDto
    {
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public int DueDays { get; set; } = 14;
    }
}