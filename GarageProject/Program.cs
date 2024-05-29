using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MudBlazor.Services;
using GarageProject.Auth;
using GarageProject.Services;
using GarageProject.Services.Interfaces;
using BlazorPro.BlazorSize;
using DatabaseLibrary.Models;
using System.Reflection;
using DatabaseLibrary.Database;
using DatabaseLibrary.Database.Appointments;
using DatabaseLibrary.Database.Invoices;
using DatabaseLibrary.Database.Users;
using DatabaseLibrary.Database.Materials;
using DatabaseLibrary.Database.ServiceActions;
using DatabaseLibrary.Database.Reviews;
using DatabaseLibrary.Database.InvoiceCouples;

var builder = WebApplication.CreateBuilder(args);


// Load salt from appsettings.json
var configuration = builder.Configuration;
var salt = configuration["Salt"];
var conn = configuration["ConnectionStrings:default"];

UserAccount.Salt=salt;
DatabaseContext._defaultCconnectionString = conn;
//FieldInfo fieldInfo = typeof(UserAccount).GetField("Salt", BindingFlags.Public | BindingFlags.Static);
//if (fieldInfo != null)
//{
//    fieldInfo.SetValue(null, salt);
//}

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddAuthenticationCore();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<ProtectedSessionStorage>();

//save dark mode prefference
builder.Services.AddScoped<IUserPreferencesService,UserPreferencesService>();

//Refresh service
builder.Services.AddScoped<RemoteService>();
//builder.Services.AddScoped<ThemeCallbackService>();

builder.Services.AddSingleton<CommunicationService>();

builder.Services.AddMediaQueryService();


//database
builder.Services.AddHttpClient();
//builder.Services.AddDbContext<DatabaseContext>(
//               o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

//auth
//i changed this from singleton to scoped (i hope it wont crash in the future because of conflicts or whatever)


//builder.Services.AddScoped<DatabaseContext>();

builder.Services.AddDbContextFactory<DatabaseContext>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IServiceActionRepository, ServiceActionRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceCoupleRepository, InvoiceCoupleRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();


builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

// Initialize the database
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var AccountDB = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    if (AccountDB.Database.EnsureCreated())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        // Ensure that the database and its tables are created
        dbContext.Database.EnsureCreated();

        DefaultData.Initialize(AccountDB);
    }
}

app.Run();