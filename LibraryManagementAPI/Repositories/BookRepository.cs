using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly List<Book> _books = new()
        {
            new Book { Id = 1, Title = "Clean Code", Author = "Robert C. Martin", Genre = "Programming", IsAvailable = true },
            new Book { Id = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Genre = "Programming", IsAvailable = true },
            new Book { Id = 3, Title = "Design Patterns", Author = "Gang of Four", Genre = "Programming", IsAvailable = false }
        };

        private int _nextId = 4;

        public List<Book> GetAll() => _books;

        public Book? GetById(int id) => _books.FirstOrDefault(b => b.Id == id);

        public Book Add(Book book)
        {
            book.Id = _nextId++;
            _books.Add(book);
            return book;
        }

        public Book? Update(int id, Book book)
        {
            var existing = GetById(id);
            if (existing == null) return null;

            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.Genre = book.Genre;
            existing.IsAvailable = book.IsAvailable;
            return existing;
        }

        public bool Delete(int id)
        {
            var book = GetById(id);
            if (book == null) return false;

            _books.Remove(book);
            return true;
        }
    }
}