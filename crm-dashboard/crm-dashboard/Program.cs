using System.Net.Http.Headers;
using crm_dashboard.Services;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Register HTTP client for Spring Boot API
builder.Services.AddHttpClient("SpringBootAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:8080/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.Configure<RequestLocalizationOptions>(options =>
    {

        options.DefaultRequestCulture = new RequestCulture("en-Us");
    }
);

// Register SpringBootService
builder.Services.AddScoped<SpringBootService>();

// CORS to allow Spring Boot requests
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpringBoot",
        policy => policy.WithOrigins("http://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();  // You had MapStaticAssets() — prefer this standard one

app.UseSession();
app.UseRouting();

app.UseCors("AllowSpringBoot");
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "dashboard-leads",
    pattern: "Dashboard/Leads/{status}",
    defaults: new { controller = "Dashboard", action = "LeadsByStatus" });

app.MapControllerRoute(
    name: "dashboard-tickets",
    pattern: "Dashboard/Tickets/{priority}",
    defaults: new { controller = "Dashboard", action = "TicketsByPriority" });

app.Run();