namespace crm_dashboard.Models;

public class Budget
{
    public int budgetId { get; set; }
    public string budgetDesc { get; set; }
    public Customer customer { get; set; }
    public double amount { get; set; }
    public DateTime createdAt { get; set; }
}