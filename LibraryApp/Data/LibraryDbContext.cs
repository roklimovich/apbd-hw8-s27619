using Microsoft.EntityFrameworkCore;
using LibraryApp.Models;

namespace LibraryApp.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Borrowing> Borrowings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(a => a.LastName).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title).IsRequired().HasMaxLength(300);
            entity.Property(b => b.Isbn).IsRequired().HasMaxLength(20);

            entity.HasOne(b => b.Author)
                  .WithMany(a => a.Books)
                  .HasForeignKey(b => b.AuthorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Borrowing>(entity =>
        {
            entity.HasKey(br => br.Id);
            entity.Property(br => br.BorrowerName).IsRequired().HasMaxLength(200);

            entity.HasOne(br => br.Book)
                  .WithMany(b => b.Borrowings)
                  .HasForeignKey(br => br.BookId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, FirstName = "Frank",   LastName = "Herbert" },
            new Author { Id = 2, FirstName = "Ursula",  LastName = "Le Guin" },
            new Author { Id = 3, FirstName = "Isaac",   LastName = "Asimov"  }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "Dune",                        Isbn = "978-0-441-17271-9", PublishedYear = 1965, AuthorId = 1 },
            new Book { Id = 2, Title = "Dune Messiah",                Isbn = "978-0-441-17269-6", PublishedYear = 1969, AuthorId = 1 },
            new Book { Id = 3, Title = "The Left Hand of Darkness",   Isbn = "978-0-441-47812-5", PublishedYear = 1969, AuthorId = 2 },
            new Book { Id = 4, Title = "The Dispossessed",            Isbn = "978-0-061-05488-9", PublishedYear = 1974, AuthorId = 2 },
            new Book { Id = 5, Title = "Foundation",                  Isbn = "978-0-553-29335-7", PublishedYear = 1951, AuthorId = 3 }
        );
    }
}
