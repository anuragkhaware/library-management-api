using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Repositories
{
    public interface IMemberRepository
    {
        Member? GetByEmail(string email);
        Member Add(Member member);
        bool EmailExists(string email);
    }
}