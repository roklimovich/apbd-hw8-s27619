using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LibraryApp.Models;
using LibraryApp.Services;

namespace LibraryApp.Controllers;

public class BooksController : Controller
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService) => _bookService = bookService;

    // GET /Books
    public async Task<IActionResult> Index()
    {
        var books = await _bookService.GetAllBooksAsync();
        return View(books);
    }

    // GET /Books/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var book = await _bookService.GetBookWithBorrowingsAsync(id);
        if (book is null)
            return NotFound();

        return View(book);
    }

    // GET /Books/Create
    public async Task<IActionResult> Create()
    {
        await PopulateAuthorsDropdown();
        return View();
    }

    // POST /Books/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Book book)
    {
        if (!ModelState.IsValid)
        {
            await PopulateAuthorsDropdown(book.AuthorId);
            return View(book);
        }

        await _bookService.CreateBookAsync(book);
        TempData["Success"] = $"Book \"{book.Title}\" added successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateAuthorsDropdown(int? selectedId = null)
    {
        var authors = await _bookService.GetAllAuthorsAsync();
        ViewBag.AuthorId = new SelectList(authors, "Id", "FullName", selectedId);
    }
}
