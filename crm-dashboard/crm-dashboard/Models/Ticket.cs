namespace crm_dashboard.Models;

public class Ticket
{
    public Ticket() { }
    
    public int ticketId { get; set; }
    public string subject { get; set; }
    public string status { get; set; }
    public string priority { get; set; }
    public Customer customer { get; set; }
    public DateTime createdAt { get; set; }
    public double depense { get; set; }

}