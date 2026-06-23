using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models;

public class Book
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(300)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "ISBN is required.")]
    [MaxLength(20)]
    public string Isbn { get; set; } = string.Empty;

    [Range(1901, 2100, ErrorMessage = "Published year must be greater than 1900.")]
    public int PublishedYear { get; set; }

    public int AuthorId { get; set; }

    // Navigation properties
    public Author? Author { get; set; }
    public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
}
