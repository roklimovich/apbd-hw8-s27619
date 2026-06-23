using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LibraryApp.Models;
using LibraryApp.Services;

namespace LibraryApp.Controllers;

public class BorrowingsController : Controller
{
    private readonly IBorrowingService _borrowingService;
    private readonly IBookService _bookService;

    public BorrowingsController(IBorrowingService borrowingService, IBookService bookService)
    {
        _borrowingService = borrowingService;
        _bookService = bookService;
    }

    // GET /Borrowings/Create?bookId=5
    public async Task<IActionResult> Create(int? bookId)
    {
        await PopulateBooksDropdown(bookId);
        var borrowing = bookId.HasValue ? new Borrowing { BookId = bookId.Value } : new Borrowing();
        return View(borrowing);
    }

    // POST /Borrowings/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Borrowing borrowing)
    {
        if (!ModelState.IsValid)
        {
            await PopulateBooksDropdown(borrowing.BookId);
            return View(borrowing);
        }

        bool success = await _borrowingService.CreateBorrowingAsync(borrowing);
        if (!success)
        {
            ModelState.AddModelError("BookId", "The selected book does not exist.");
            await PopulateBooksDropdown(borrowing.BookId);
            return View(borrowing);
        }

        TempData["Success"] = $"Borrowing recorded for \"{borrowing.BorrowerName}\".";
        return RedirectToAction("Details", "Books", new { id = borrowing.BookId });
    }

    private async Task PopulateBooksDropdown(int? selectedId = null)
    {
        var books = await _bookService.GetAllBooksAsync();
        ViewBag.BookId = new SelectList(books, "Id", "Title", selectedId);
    }
}
