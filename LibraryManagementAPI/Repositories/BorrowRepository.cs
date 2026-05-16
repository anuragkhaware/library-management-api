using Dapper;
using LibraryManagementAPI.Models;
using Microsoft.Data.SqlClient;

namespace LibraryManagementAPI.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly string _connectionString;

        public BorrowRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public BorrowRecord Borrow(BorrowRecord record)
        {
            using var connection = CreateConnection();
            var sql = @"
                INSERT INTO BorrowRecords (BookId, MemberId, BorrowedAt, DueDate, IsReturned)
                VALUES (@BookId, @MemberId, @BorrowedAt, @DueDate, @IsReturned);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            record.Id = connection.ExecuteScalar<int>(sql, record);

            // Mark book as unavailable
            connection.Execute(
                "UPDATE Books SET IsAvailable = 0 WHERE Id = @Id",
                new { Id = record.BookId });

            return record;
        }

        public BorrowRecord? Return(int borrowId)
        {
            using var connection = CreateConnection();
            var record = connection.QueryFirstOrDefault<BorrowRecord>(
                "SELECT * FROM BorrowRecords WHERE Id = @Id", new { Id = borrowId });

            if (record == null || record.IsReturned) return null;

            connection.Execute(@"
                UPDATE BorrowRecords 
                SET IsReturned = 1, ReturnedAt = @ReturnedAt 
                WHERE Id = @Id",
                new { ReturnedAt = DateTime.UtcNow, Id = borrowId });

            // Mark book as available again
            connection.Execute(
                "UPDATE Books SET IsAvailable = 1 WHERE Id = @Id",
                new { Id = record.BookId });

            record.IsReturned = true;
            record.ReturnedAt = DateTime.UtcNow;
            return record;
        }

        public List<BorrowRecord> GetOverdue()
        {
            using var connection = CreateConnection();
            return connection.Query<BorrowRecord>(@"
                SELECT * FROM BorrowRecords 
                WHERE IsReturned = 0 AND DueDate < GETDATE()").ToList();
        }

        public List<BorrowRecord> GetByMember(int memberId)
        {
            using var connection = CreateConnection();
            return connection.Query<BorrowRecord>(
                "SELECT * FROM BorrowRecords WHERE MemberId = @MemberId",
                new { MemberId = memberId }).ToList();
        }
    }
}