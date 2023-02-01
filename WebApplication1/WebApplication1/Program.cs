using ClassRoomMVC.Data;
using ClassRoomMVC.Models;
using ClassRoomMVC.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<TaskModel>();
builder.Services.AddScoped<GroupsRepository>();
builder.Services.AddScoped<Group>();

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<ClassRoomDbContext>();


builder.Services.AddLocalization(option =>
{
    option.ResourcesPath = "Resources";
}).AddControllersWithViews()
.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Home/Privacy";
});

builder.Services.AddDbContext<ClassRoomDbContext>(options =>
    options.UseLazyLoadingProxies()
    .UseSqlite(builder.Configuration
    .GetConnectionString("DefaultConnection")));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AddPolicy", policy => policy.RequireRole("Teacher", "Director", "Tutor"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("ForAll", cors =>
    {
        cors.AllowAnyHeader();
        cors.AllowAnyMethod();
        cors.AllowAnyOrigin();
    });
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseCors("ForAll");

app.UseAuthorization();

app.UseRequestLocalization(option =>
{
    option.AddSupportedUICultures("En", "Ru", "Uz");
    option.FallBackToParentUICultures = true;
    option.RequestCultureProviders = new List<IRequestCultureProvider>()
    {
        new CookieRequestCultureProvider(),
        new QueryStringRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    };
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
