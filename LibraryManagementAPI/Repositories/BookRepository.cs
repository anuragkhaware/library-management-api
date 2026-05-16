using Dapper;
using LibraryManagementAPI.Models;
using Microsoft.Data.SqlClient;

namespace LibraryManagementAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;

        public BookRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public List<Book> GetAll()
        {
            using var connection = CreateConnection();
            return connection.Query<Book>("SELECT * FROM Books").ToList();
        }

        public Book? GetById(int id)
        {
            using var connection = CreateConnection();
            return connection.QueryFirstOrDefault<Book>(
                "SELECT * FROM Books WHERE Id = @Id", new { Id = id });
        }

        public Book Add(Book book)
        {
            using var connection = CreateConnection();
            var sql = @"INSERT INTO Books (Title, Author, Genre, IsAvailable) 
                        VALUES (@Title, @Author, @Genre, @IsAvailable);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";
            book.Id = connection.ExecuteScalar<int>(sql, book);
            return book;
        }

        public Book? Update(int id, Book book)
        {
            using var connection = CreateConnection();
            var sql = @"UPDATE Books 
                        SET Title = @Title, Author = @Author, 
                            Genre = @Genre, IsAvailable = @IsAvailable 
                        WHERE Id = @Id";
            book.Id = id;
            var rows = connection.Execute(sql, book);
            return rows == 0 ? null : book;
        }

        public bool Delete(int id)
        {
            using var connection = CreateConnection();
            var rows = connection.Execute(
                "DELETE FROM Books WHERE Id = @Id", new { Id = id });
            return rows > 0;
        }
    }
}