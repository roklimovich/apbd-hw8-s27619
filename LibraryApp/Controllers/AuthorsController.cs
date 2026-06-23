using Microsoft.AspNetCore.Mvc;
using LibraryApp.Services;

namespace LibraryApp.Controllers;

public class AuthorsController : Controller
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService) => _authorService = authorService;

    // GET /Authors
    public async Task<IActionResult> Index()
    {
        var authors = await _authorService.GetAllAuthorsAsync();
        return View(authors);
    }

    // GET /Authors/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var author = await _authorService.GetAuthorByIdAsync(id);
        if (author is null)
            return NotFound();

        return View(author);
    }
}
