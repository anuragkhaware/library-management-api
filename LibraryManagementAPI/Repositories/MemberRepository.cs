using Dapper;
using LibraryManagementAPI.Models;
using Microsoft.Data.SqlClient;

namespace LibraryManagementAPI.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly string _connectionString;

        public MemberRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public Member? GetByEmail(string email)
        {
            using var connection = CreateConnection();
            return connection.QueryFirstOrDefault<Member>(
                "SELECT * FROM Members WHERE Email = @Email", new { Email = email });
        }

        public Member Add(Member member)
        {
            using var connection = CreateConnection();
            var sql = @"INSERT INTO Members (FullName, Email, PasswordHash, Role, CreatedAt)
                        VALUES (@FullName, @Email, @PasswordHash, @Role, @CreatedAt);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";
            member.Id = connection.ExecuteScalar<int>(sql, member);
            return member;
        }

        public bool EmailExists(string email)
        {
            using var connection = CreateConnection();
            return connection.ExecuteScalar<bool>(
                "SELECT COUNT(1) FROM Members WHERE Email = @Email", new { Email = email });
        }
    }
}