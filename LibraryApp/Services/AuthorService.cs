using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class AuthorService : IAuthorService
{
    private readonly LibraryDbContext _db;

    public AuthorService(LibraryDbContext db) => _db = db;

    public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        => await _db.Authors
                    .Include(a => a.Books)
                    .OrderBy(a => a.LastName)
                    .ToListAsync();

    public async Task<Author?> GetAuthorByIdAsync(int id)
        => await _db.Authors
                    .Include(a => a.Books)
                    .FirstOrDefaultAsync(a => a.Id == id);
}
