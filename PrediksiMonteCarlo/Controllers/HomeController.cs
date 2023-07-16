using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PrediksiMonteCarlo.Data;
using PrediksiMonteCarlo.Models;
using PrediksiMonteCarlo.SD;

namespace PrediksiMonteCarlo.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if(StaticDetails_Login.Roles == null)
        {
            TempData["error"] = "Silahkan login untuk melanjutkan !!";

            return RedirectToAction("Login");
        }

        //var data = _db.Penggunas.Where(x => x.Username == png.Username).FirstOrDefault();

        //var role = data.Roles;

        //StaticDetails_Login.Roles = role;

        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Logout()
    {
        StaticDetails_Login.Username = null;
        StaticDetails_Login.Roles = null;

        return RedirectToAction("Login", "Home");
    }

    [HttpPost]
    public IActionResult Login(Pengguna png)
    {
        var check = _db.Penggunas.Where(x => x.Username == png.Username && x.Password == png.Password).FirstOrDefault();

        if (check != null)
        {
            var role = check.Roles;

            StaticDetails_Login.Roles = role;

            return RedirectToAction("Index");
        }
        else if(png.Username ==null|| png.Password==null)
        {

            return View();
        }

        TempData["error"] = "Password atau Username Salah !!";

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

