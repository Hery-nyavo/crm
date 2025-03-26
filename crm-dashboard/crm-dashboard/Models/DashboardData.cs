namespace crm_dashboard.Models;

public class DashboardData
{
    public List<Lead> Leads { get; set; }
    public List<Ticket> Tickets { get; set; }
    public List<Budget> Budgets { get; set; }
    
    public List<int> GetLeadsData()
    {
        var statuses = new List<string> 
        { 
            "meeting-to-schedule", 
            "scheduled", 
            "archived", 
            "success", 
            "assign-to-sales" 
        };
        var leadsData = statuses.Select(status =>
            Leads.Count(l => l.status.Equals(status, StringComparison.OrdinalIgnoreCase))
        ).ToList();

        return leadsData;
    }
    
    public List<int> GetTicketsData()
    {
        var priorities = new List<string> 
        { 
            "low", 
            "medium", 
            "high", 
            "closed", 
            "urgent", 
            "critical" 
        };
        var ticketsData = priorities.Select(priority =>
            Tickets.Count(t => t.priority.Equals(priority, StringComparison.OrdinalIgnoreCase))
        ).ToList();

        return ticketsData;
    } 
    public List<int> GetTicketsDataStatus()
    {
        var status = new List<string> 
        { 
            "Open", 
            "Assigned", 
            "On Hold", 
            "In Progress", 
            "Resolved", 
            "Closed",
            "Closed",
            "Reopened",
            "Pending Customer Responses",
            "Escalated",
            "Archived"
        };
        var ticketsData = status.Select(statu =>
            Tickets.Count(t => t.status.Equals(statu, StringComparison.OrdinalIgnoreCase))
        ).ToList();

        return ticketsData;
    }
    public double GetTotalTicketAmount()
    {
        return Tickets.Sum(t => t.depense);
    }
    public double GetTotalLeadAmount()
    {
        return Leads.Sum(l => l.depense);
    }
    public double GetTotalBudgetAmount()
    {
        return Budgets.Sum(b => b.amount);
    }
    public double GetActualBudgetAmount()
    {
        var all=GetTotalBudgetAmount();
        double outbudget=GetTotalTicketAmount();
        outbudget+=GetTotalLeadAmount();
        return all-outbudget;
    }
    public List<Ticket> GetTicketsByPriority(string priority)
    {
        return Tickets
            .Where(t => string.Equals(t.priority, priority, StringComparison.OrdinalIgnoreCase))
            .ToList();
    } public List<Ticket> GetTicketsByStatus(string status)
    {
        return Tickets
            .Where(t => string.Equals(t.status, status, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
    public List<Lead> GetLeadsByStatus( string status)
    {
        return Leads
            .Where(l => string.Equals(l.status, status, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public double GetNumberLeads()
    {
        return Leads.Count;
    }

    public double GetNumberTickets()
    {
        return Tickets.Count;
    }
   

}