using LibraryApp.Models;

namespace LibraryApp.Services;

public interface IBorrowingService
{
    Task<IEnumerable<Borrowing>> GetBorrowingsForBookAsync(int bookId);
    /// <summary>
    /// Creates a borrowing. Returns false if the book does not exist.
    /// </summary>
    Task<bool> CreateBorrowingAsync(Borrowing borrowing);
}
