using crm_dashboard.Models;
using crm_dashboard.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace crm_dashboard.Controllers;

public class DashboardController: Controller
{
    private readonly SpringBootService _service;

    public DashboardController(SpringBootService service)
    {
        _service = service;
    }

    /*public ActionResult Index()
    {
        // Simulating data, replace this with actual data from your database
        var leadsData = new List<int> { 10, 5, 7, 3 }; // New, Contacted, In Progress, Closed
        var ticketsData = new List<int> { 4, 3, 8, 2 }; // Low, Medium, High, Urgent
        var budgetData = new List<decimal> { 5000, 4000, 3500 }; // Total Budget, Actual Budget, Remaining Budget

        // Pass data to the view
        ViewBag.LeadsData = leadsData;
        ViewBag.TicketsData = ticketsData;
        ViewBag.BudgetData = budgetData;

        return View();
    }*/
    
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult ShowDashboardResult()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        // Get the DashboardData from the session
        var dashboardData = GetDashboardDataFromSession();

        if (dashboardData == null)
        {
            return View("Error"); // Show an error page if no data is found in session
        }

        // Pass the DashboardData object to the view
        return View(dashboardData);
    }
    [HttpPost]
    public async Task<IActionResult> Dashboard(DateParameter model)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        var result = await _service.GetData(model);
        Console.WriteLine(result);
        HttpContext.Session.SetString("DashboardResult", JsonConvert.SerializeObject(result));
        if (result != null)
        {
        ViewBag.LeadsData = result.GetLeadsData();
        ViewBag.TicketsData = result.GetTicketsDataStatus();
        ViewBag.BudgetData = new List<double> { result.GetTotalBudgetAmount(), result.GetActualBudgetAmount() };
        ViewBag.LeadsNumber = result.GetNumberLeads();
        ViewBag.TicketsNumber = result.GetNumberTickets(); 
        ViewBag.LeadsTotal = result.GetTotalLeadAmount();
        ViewBag.TicketsTotal = result.GetTotalTicketAmount();
        }
        return View("Index", model);
    }
    [Route("Dashboard/Tickets/{priority}")]
    public IActionResult TicketsByPriority(string priority)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        var dashboardData = GetDashboardDataFromSession();

        if (dashboardData != null)
        {
            ViewBag.TicketData = dashboardData.GetTicketsByStatus(priority);
        }
        return View("~/Views/Ticket/Show.cshtml");
    }
    [Route("Dashboard/Leads/{status}")]
    public IActionResult LeadsByStatus(string status)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        var dashboardData = GetDashboardDataFromSession();

        if (dashboardData != null)
        {
            ViewBag.LeadsData = dashboardData.GetLeadsByStatus(status);
        }
        return View("~/Views/Lead/Show.cshtml");
 
    }
    public DashboardData? GetDashboardDataFromSession()
    {
      
        // Retrieve the JSON string from the session
        var sessionData = HttpContext.Session.GetString("DashboardResult");

        if (string.IsNullOrEmpty(sessionData))
        {
            return null; 
        }

        // Deserialize the JSON string to DashboardData
        var result = JsonConvert.DeserializeObject<DashboardData>(sessionData);

        return result;
    }
    public IActionResult UpdateTicket(int ticketId,string priority)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        var dashboardData = GetDashboardDataFromSession();
    
        if (dashboardData != null)
        {
            var ticket = dashboardData.Tickets.FirstOrDefault(t => t.ticketId == ticketId);
            if (ticket != null)
            {
               
                return View("~/Views/Ticket/Update.cshtml", ticket);

            }
        }

        return RedirectToAction("TicketsByPriority", new { priority = priority });
    }

    public async Task<IActionResult> DeleteTicket(int ticketId,string priority)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        var dashboardData = GetDashboardDataFromSession();

        if (dashboardData != null)
        {
            var ticket = dashboardData.Tickets.FirstOrDefault(t => t.ticketId == ticketId);
            if (ticket != null)
            {
                // Logic to delete the ticket
                dashboardData.Tickets.Remove(ticket);
                await _service.DeleteTicketInBackend(ticketId);
                // Update the session with the new data
                HttpContext.Session.SetString("DashboardResult", JsonConvert.SerializeObject(dashboardData));
            
                return RedirectToAction("TicketsByPriority", new { priority = priority });
            }
        }

        return RedirectToAction("TicketsByPriority", new { priority = priority });
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateTicket(Ticket updatedTicket)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        var dashboardData = GetDashboardDataFromSession();

        if (dashboardData != null)
        {
            var ticket = dashboardData.Tickets.FirstOrDefault(t => t.ticketId == updatedTicket.ticketId);
            if (ticket != null)
            {
                // Update the ticket details
                ticket.depense = updatedTicket.depense;
                await _service.UpdateTicketInDashboard(ticket);
                // Update the session with the new data
                HttpContext.Session.SetString("DashboardResult", JsonConvert.SerializeObject(dashboardData));

                return RedirectToAction("TicketsByPriority", new { priority = ticket.priority });
            }
        }

        return RedirectToAction("TicketsByPriority", new { priority =updatedTicket.priority });
    }
    public IActionResult UpdateLead(int leadId, string status)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        var dashboardData = GetDashboardDataFromSession();

        if (dashboardData != null)
        {
            var lead = dashboardData.Leads.FirstOrDefault(l => l.leadId == leadId);
            if (lead != null)
            {
                return View("~/Views/Lead/Update.cshtml", lead);
            }
        }

        return RedirectToAction("LeadsByStatus", new { status = status });
    }
    public async Task<IActionResult> DeleteLead(int leadId,string status)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        var dashboardData = GetDashboardDataFromSession();

        if (dashboardData != null)
        {
            var lead = dashboardData.Leads.FirstOrDefault(l => l.leadId == leadId);
            if (lead != null)
            {
                dashboardData.Leads.Remove(lead); // Remove the lead from the list
                await _service.DeleteLeadInBackend(leadId);
                // Update the session with the new data
                HttpContext.Session.SetString("DashboardResult", JsonConvert.SerializeObject(dashboardData));

                return RedirectToAction("LeadsByStatus", new { status = status });
            }
        }

        return RedirectToAction("LeadsByStatus", new { status = status });
    }
    [HttpPost]
    public async Task<IActionResult> UpdateLead(Lead updatedLead)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        var dashboardData = GetDashboardDataFromSession();
    
        if (dashboardData != null)
        {
            var lead = dashboardData.Leads.FirstOrDefault(l => l.leadId == updatedLead.leadId);
            if (lead != null)
            {
                lead.depense = updatedLead.depense;
                await _service.UpdateLeadInDashboard(lead);
                HttpContext.Session.SetString("DashboardResult", JsonConvert.SerializeObject(dashboardData));

                return RedirectToAction("LeadsByStatus", new { status = lead.status });
            }
        }

        return RedirectToAction("LeadsByStatus", new { status = updatedLead.status });
    }
    
    [Route("Dashboard/Budget/{filtre}")]
    public IActionResult BudgetData(string filtre)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        var dashboardData = GetDashboardDataFromSession();

        if (dashboardData != null)
        {
            ViewBag.BudgetData = dashboardData.Budgets;
            ViewBag.TicketData = dashboardData.Tickets;
            ViewBag.LeadData = dashboardData.Leads;
        }

        if (filtre.Equals("Total Budget",StringComparison.OrdinalIgnoreCase))
        {
            return View("~/Views/Budget/Show.cshtml");
        }
        return View("~/Views/Budget/Actual.cshtml");
    }
    public IActionResult UpdateBudget(int budgeId)
    {
        
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        var dashboardData = GetDashboardDataFromSession();
    
        if (dashboardData != null)
        {
            var budget = dashboardData.Budgets.FirstOrDefault(b => b.budgetId == budgeId);
            if (budget != null)
            {
               
                return View("~/Views/Budget/Update.cshtml", budget);

            }
        }

        return RedirectToAction("BudgetData", new { filtre = "Total Budget" });
    }
    
    public async Task<IActionResult> DeleteBudget(int budgetId)
    {
        
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        var dashboardData = GetDashboardDataFromSession();

        if (dashboardData != null)
        {
            var budget = dashboardData.Budgets.FirstOrDefault(t => t.budgetId == budgetId);
            if (budget != null)
            {
                // Logic to delete the ticket
                dashboardData.Budgets.Remove(budget);
                await _service.DeleteBudgetInBackend(budgetId);
                // Update the session with the new data
                HttpContext.Session.SetString("DashboardResult", JsonConvert.SerializeObject(dashboardData));
            
                return RedirectToAction("BudgetData", new { filtre = "Total Budget" });
            }
        }

        return RedirectToAction("BudgetData", new { filtre = "Total Budget" });
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateBudget(Budget updatedBudget)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        var dashboardData = GetDashboardDataFromSession();

        if (dashboardData != null)
        {
            var budget = dashboardData.Budgets.FirstOrDefault(t => t.budgetId == updatedBudget.budgetId);
            if (budget != null)
            {
                // Update the ticket details
                budget.amount = updatedBudget.amount;
                Console.WriteLine(budget.amount);
                await _service.UpdateBudgetInDashboard(budget);
                // Update the session with the new data
                HttpContext.Session.SetString("DashboardResult", JsonConvert.SerializeObject(dashboardData));

                return RedirectToAction("BudgetData", new { filtre = "Total Budget" });
            }
        }

        return RedirectToAction("BudgetData", new { filtre = "Total Budget" });
    }
    
    [HttpGet]
    public IActionResult Logout()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
        {
            return RedirectToAction("Login", "Auth");
        }
        HttpContext.Session.Clear(); 
        return View("~/Views/Auth/Login.cshtml");

    }


}