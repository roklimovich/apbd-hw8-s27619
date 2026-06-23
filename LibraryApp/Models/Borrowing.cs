using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models;

public class Borrowing
{
    public int Id { get; set; }

    public int BookId { get; set; }

    [Required(ErrorMessage = "Borrower name is required.")]
    [MaxLength(200)]
    public string BorrowerName { get; set; } = string.Empty;

    public DateTime BorrowedAt { get; set; }

    public DateTime? ReturnedAt { get; set; }

    // Navigation property
    public Book? Book { get; set; }

    public bool IsReturned => ReturnedAt.HasValue;
}
