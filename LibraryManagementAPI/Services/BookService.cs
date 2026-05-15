using LibraryManagementAPI.Models;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Repositories;

namespace LibraryManagementAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;

        public BookService(IBookRepository repository)
        {
            _repository = repository;
        }

        public List<Book> GetAll() => _repository.GetAll();

        public Book? GetById(int id) => _repository.GetById(id);

        public Book Add(BookDto bookDto)
        {
            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Genre = bookDto.Genre,
                IsAvailable = true
            };
            return _repository.Add(book);
        }

        public Book? Update(int id, BookDto bookDto)
        {
            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Genre = bookDto.Genre
            };
            return _repository.Update(id, book);
        }

        public bool Delete(int id) => _repository.Delete(id);
    }
}