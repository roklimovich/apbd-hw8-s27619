using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class BorrowingService : IBorrowingService
{
    private readonly LibraryDbContext _db;

    public BorrowingService(LibraryDbContext db) => _db = db;

    public async Task<IEnumerable<Borrowing>> GetBorrowingsForBookAsync(int bookId)
        => await _db.Borrowings
                    .Where(br => br.BookId == bookId)
                    .OrderByDescending(br => br.BorrowedAt)
                    .ToListAsync();

    public async Task<bool> CreateBorrowingAsync(Borrowing borrowing)
    {
        // Business rule: book must exist
        bool bookExists = await _db.Books.AnyAsync(b => b.Id == borrowing.BookId);
        if (!bookExists)
            return false;

        borrowing.BorrowedAt = DateTime.UtcNow;
        _db.Borrowings.Add(borrowing);
        await _db.SaveChangesAsync();
        return true;
    }
}
