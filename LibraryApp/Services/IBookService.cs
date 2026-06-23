using LibraryApp.Models;

namespace LibraryApp.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<Book?> GetBookByIdAsync(int id);
    Task<Book?> GetBookWithBorrowingsAsync(int id);
    Task<IEnumerable<Author>> GetAllAuthorsAsync();
    Task CreateBookAsync(Book book);
    Task<bool> BookExistsAsync(int id);
}
