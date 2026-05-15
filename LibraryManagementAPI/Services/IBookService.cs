using LibraryManagementAPI.Models;
using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Services
{
    public interface IBookService
    {
        List<Book> GetAll();
        Book? GetById(int id);
        Book Add(BookDto bookDto);
        Book? Update(int id, BookDto bookDto);
        bool Delete(int id);
    }
}