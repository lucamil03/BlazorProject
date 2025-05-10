using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EquipmentManagementSystem.Components;
using EquipmentManagementSystem.Components.Account;
using EquipmentManagementSystem.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState(); //Macht Authentifizierungsstatus (AuthenticationState) in allen Komponenten verfügbar
builder.Services.AddScoped<IdentityUserAccessor>(); //Zugriff auf den aktuellen IdentityUser in Blazor-Komponenten
builder.Services.AddScoped<IdentityRedirectManager>(); //Leitet unautorisierte Benutzer automatisch um auf Login
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>(); //Blazor-Komponenten bekommen Zugriff auf Auth-Status zB @attribute [Authorize]

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter(); //Zeigt bessere Fehlermeldungen bei DB-Problemen im Entwicklungsmodus

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => //AddIdentityCore<T>	Nur die minimalen User-Funktionen z.B Login, AddIdentity - Komplettes System, inkl. RoleManager etc.
    {
        options.SignIn.RequireConfirmedAccount = false; //Sonst Mail sender integrieren
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders(); //Brauche ich eigentlich nicht weil ich keine 2FA und passwort reset usw unterstütze

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});

var app = builder.Build();

//Rollen initialisieren und festlegen welche ich habe
using (var scope = app.Services.CreateScope())
{
    await RolesInitializer.InitializeRoles(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthorization();
app.UseHttpsRedirection(); //Leitet HTTP auf HTTPS um

app.UseAntiforgery(); //Schutz gegen CSRF-Angriffe

app.MapStaticAssets();
app.MapRazorComponents<App>() //Startpunkt meiner Blazor-App App.razor
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();