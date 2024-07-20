using Infra;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IAddressRepo, AddressRepo>();

builder.Services.AddScoped<IEncryptService, EncryptService>(p => new EncryptService(new Domain.EncryptKeys(builder.Configuration["EncryptionKeys:PASSWORDHASH"], builder.Configuration["EncryptionKeys:SALTKEY"], builder.Configuration["EncryptionKeys:VIKEY"])));
builder.Services.AddScoped<IJwtFunctions, JwtFunctions>(p => new JwtFunctions(builder.Configuration["JwtKey"]));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAddressService, AddressService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(300000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

string? conn = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddMySql<AppDbContext>(conn, ServerVersion.AutoDetect(conn));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]))
    };
    options.SaveToken = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/SignIN";
    options.AccessDeniedPath = "/User/SignIN";
});

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
