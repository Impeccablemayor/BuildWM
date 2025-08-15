using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using BuildWM.Data;
using Microsoft.AspNetCore.Identity;
using BuildWM.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddHttpClient<ProductService>();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Auth0 Authentication setup
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect("Auth0", options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        options.ClientId = builder.Configuration["Auth0:ClientId"];
        options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
        options.ResponseType = OpenIdConnectResponseType.Code;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");

        options.CallbackPath = new PathString("/callback");

        options.SaveTokens = true;

        options.Events = new OpenIdConnectEvents
        {
            OnTokenValidated = context =>
            {
                // Optional: Do something after token validation
                return Task.CompletedTask;
            },

            OnRedirectToIdentityProvider = context =>
            {
                // Optional: Customize redirection to Auth0
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// builder.Services.AddHttpsRedirection(options =>
// {
//     options.HttpsPort = 7167; // Match your launchSettings.json port
// });

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
