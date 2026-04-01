using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBookStoreSystem.Mvc;

public sealed class HomeController : Controller
{
    public IActionResult Index() => View();
}
