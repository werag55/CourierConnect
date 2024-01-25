using CourierConnect.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.DataAccess.Repository;
using CourierConnectWeb.Services.IServices;
using CourierConnectWeb.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Services;
using CourierConnect.Utility;
using CourierConnect;
using CourierConnectWeb.Services.Factory;
//using CourierConnectWeb.Email;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddTransient<IEmailSender, EmailSender>()

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}).AddCookie().AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
});
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(/*options => options.SignIn.RequireConfirmedAccount = true*/).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddRazorPages();

////////////////////////////////////////////////
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddSingleton<OurServiceFactory>();
builder.Services.AddHttpClient<IOfferService, OfferService>();
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddHttpClient<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddHttpClient<IRequestService, RequestService>();
builder.Services.AddScoped<IRequestService, RequestService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession(); //////////////////////

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
