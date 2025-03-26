using Newtonsoft.Json;

namespace crm_dashboard.Services;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using crm_dashboard.Models;
using System.Text.Json;
using System.Globalization;


public class SpringBootService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SpringBootService(IHttpClientFactory factory)
    {
        _httpClientFactory = factory;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var client = _httpClientFactory.CreateClient("SpringBootAPI");
        var response = await client.PostAsJsonAsync("api/auth/login", request);

        var content = await response.Content.ReadAsStringAsync();  // Read raw response
        Console.WriteLine($"Response Status: {response.StatusCode}");
        Console.WriteLine($"Response Content: {content}");

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<LoginResponse>(content);
                if (result != null)
                {
                    Console.WriteLine($"Result token: {result.Token}");
                    Console.WriteLine($"Result username: {result.Username}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Spring Boot returned error: " + response.StatusCode);
        }

        return null;
    }

    public async Task<DashboardData?> GetData(DateParameter model)
    {
        var client = _httpClientFactory.CreateClient("SpringBootAPI");
        var response = await client.PostAsJsonAsync("api/dashboard", model);

        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<DashboardData>(content);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Spring Boot returned error: " + response.StatusCode);
        }

        return null;
    }
    public async Task<Lead?> UpdateLeadInDashboard(Lead lead)
    {
        var client = _httpClientFactory.CreateClient("SpringBootAPI");
        
        var response = await client.PostAsJsonAsync("api/updateLead", lead);

        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<Lead>(content);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Spring Boot returned error: " + response.StatusCode);
        }

        return null;
    }
    public async Task<Ticket?> UpdateTicketInDashboard(Ticket ticket)
    {
        var client = _httpClientFactory.CreateClient("SpringBootAPI");

        // Send the updated ticket data to Spring Boot
        var response = await client.PostAsJsonAsync("api/updateTicket", ticket);

        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<Ticket>(content);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Spring Boot returned error: " + response.StatusCode);
        }

        return null;
    }
    public async Task DeleteTicketInBackend(int ticketId)
    {
        var client = _httpClientFactory.CreateClient("SpringBootAPI");

        // Send a request to the Spring Boot API to delete the ticket
        var response = await client.GetAsync($"api/deleteTicket/{ticketId}");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Ticket deleted successfully in backend.");
        }
        else
        {
            Console.WriteLine("Error deleting ticket in backend: " + response.StatusCode);
        }
    }
    public async Task DeleteLeadInBackend(int leadId)
    {
        var client = _httpClientFactory.CreateClient("SpringBootAPI");

        // Send a DELETE request to Spring Boot API to delete the lead
        var response = await client.GetAsync($"api/deleteLead/{leadId}");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Lead deleted successfully in backend.");
        }
        else
        {
            Console.WriteLine("Error deleting lead in backend: " + response.StatusCode);
        }
    }
    
    public async Task<double?> UpdateAlertInBackend(double rate)
    {
        var client = _httpClientFactory.CreateClient("SpringBootAPI");

        // Send an UPDATE request to Spring Boot API to update the alert
        var response = await client.PostAsJsonAsync($"api/alert" , rate);
        
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Alert got successfully in backend.");
            return Double.Parse(content, CultureInfo.InvariantCulture);
        }
        else
        {
            Console.WriteLine("Error getting alert in backend: " + response.StatusCode);
            return null;
        }
    }
    
    public async Task<double?> GetAlert()
    {
        var client = _httpClientFactory.CreateClient("SpringBootAPI");

        // Send a request to the Spring Boot API to delete the ticket
        var response = await client.GetAsync($"api/alertGet");

        var content = await response.Content.ReadAsStringAsync();
        
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Alert got successfully in backend.");
            return Double.Parse(content, CultureInfo.InvariantCulture);
        }
        else
        {
            Console.WriteLine("Error getting alert in backend: " + response.StatusCode);
            return null;
        }
    }
    public async Task DeleteBudgetInBackend(int budgetId)
    {
        var client = _httpClientFactory.CreateClient("SpringBootAPI");

        // Send a DELETE request to Spring Boot API to delete the lead
        var response = await client.GetAsync($"api/deleteBudget/{budgetId}");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Budget deleted successfully in backend.");
        }
        else
        {
            Console.WriteLine("Error deleting vudget in backend: " + response.StatusCode);
        }
    }
    public async Task<Budget?> UpdateBudgetInDashboard(Budget budget)
    {
        var client = _httpClientFactory.CreateClient("SpringBootAPI");

        // Send the updated ticket data to Spring Boot
        var response = await client.PostAsJsonAsync("api/updateBudget", budget);

        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<Budget>(content);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Spring Boot returned error: " + response.StatusCode);
        }

        return null;
    }


}
