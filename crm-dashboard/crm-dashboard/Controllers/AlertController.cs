namespace crm_dashboard.Controllers;

using crm_dashboard.Services;
using Microsoft.AspNetCore.Mvc;

public class AlertController : Controller
{
    
    private readonly SpringBootService _service;

    public AlertController(SpringBootService service)
    {
        _service = service;
    }
    // GET
   [HttpGet]
    public IActionResult Index()
    {
        ViewBag.Statut = "";
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        return View("Alert");
    }
    [HttpPost]
    public async Task<IActionResult> UpdateAlert(string rate,string actual)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }

        ViewBag.rate = actual;
        Console.WriteLine(rate);
        double rate2 = Double.Parse(rate);
        if (rate2 > 0)
        {
          
              var newRate =  await  _service.UpdateAlertInBackend(rate2);
              ViewBag.Rate = newRate;
                ViewBag.Statut = "Rate updated successfully";

                return View("Alert");
            
        }else if (rate2 < 0)
        {
            ViewBag.Error = "Rate should be greater than zero";
        }
        else
        {
            ViewBag.Error = "Rate should not be empty";
            
        }
       
        return View("Alert");
    } 
    [HttpGet]
    public async Task<IActionResult> GetAlert()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
              var newRate =  await  _service.GetAlert();
              ViewBag.Rate = newRate;

                return View("Alert");
                
    }
}