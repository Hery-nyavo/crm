using crm_dashboard.Models;
using crm_dashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace crm_dashboard.Controllers;

public class AuthController : Controller
{
    private readonly SpringBootService _service;

    public AuthController(SpringBootService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest model)
    {
        var result = await _service.LoginAsync(model);
        if (result != null)
        {
            HttpContext.Session.SetString("Token", result.Token);
            HttpContext.Session.SetString("Username", result.Username);
            return RedirectToAction("Index", "Dashboard");

        }
        ViewBag.Error = "Login failed! Eamil or password incorrect!";
        return View();
    }
    
    public IActionResult UserInfo()
    {
        var token = HttpContext.Session.GetString("Token");
        var username = HttpContext.Session.GetString("Username");

        ViewBag.Token = token;
        ViewBag.Username = username;

        return View();
    }

    
}