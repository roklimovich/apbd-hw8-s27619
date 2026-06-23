using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => RedirectToAction("Index", "Books");
}
