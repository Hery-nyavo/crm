namespace crm_dashboard.Models;

public class Lead
{
    public int leadId { get; set; }
    public string name { get; set; }
    public string status { get; set; }
    public Customer customer { get; set; }
    public DateTime createdAt { get; set; }
    public double depense { get; set; }
}