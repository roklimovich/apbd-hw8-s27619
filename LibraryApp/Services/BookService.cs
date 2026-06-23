using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class BookService : IBookService
{
    private readonly LibraryDbContext _db;

    public BookService(LibraryDbContext db) => _db = db;

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
        => await _db.Books
                    .Include(b => b.Author)
                    .OrderBy(b => b.Title)
                    .ToListAsync();

    public async Task<Book?> GetBookByIdAsync(int id)
        => await _db.Books
                    .Include(b => b.Author)
                    .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<Book?> GetBookWithBorrowingsAsync(int id)
        => await _db.Books
                    .Include(b => b.Author)
                    .Include(b => b.Borrowings)
                    .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        => await _db.Authors.OrderBy(a => a.LastName).ToListAsync();

    public async Task CreateBookAsync(Book book)
    {
        _db.Books.Add(book);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> BookExistsAsync(int id)
        => await _db.Books.AnyAsync(b => b.Id == id);
}
