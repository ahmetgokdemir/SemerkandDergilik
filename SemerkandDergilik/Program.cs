using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.BLL.ServiceExtensions;
using Project.DAL.Context;
using Project.ENTITIES.Models;
using Semerkand_Dergilik.CustomValidation;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// builder.Services.AddDbContextService();

/*builder.Services.AddDbContext<SemerkandDergilikContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MyConnection");
    options.UseSqlServer(connectionString);
});*/

/*
builder.Services.AddDbContext<SemerkandDergilikContext>(opts =>
        opts.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"),
            options => options.MigrationsAssembly("EFCoreApp")));
*/

builder.Services.AddDbContext<SemerkandDergilikContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
    // opts.UseSqlServer(configuration["ConnectionStrings:DefaultAzureConnectionString"]);
});


builder.Services.AddIdentity<AppUser, AppRole>(opts =>
{
    //https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.useroptions.allowedusernamecharacters?view=aspnetcore-2.2

    // Backend için hazýrlanan validationlar (eðer kullanýcý browser'ýn client validationý kapatýrsa tedbiren backend kýsmý eklendi..)
    // User ile alakalý default gelen validation'lar
    opts.User.RequireUniqueEmail = true; // Email is already taken  þeklinde hata verir..
    opts.User.AllowedUserNameCharacters = "abcçdefgðhýijklmnoöpqrsþtuüvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";

    // password ile alakalý default gelen validation'lar
    opts.Password.RequiredLength = 8; // default deðeri 6
    opts.Password.RequireNonAlphanumeric = true; // default deðeri true */_ gibi deðerleri temsil eder..
    opts.Password.RequireLowercase = true;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = true;
}).AddPasswordValidator<CustomPasswordValidator>().
AddUserValidator<CustomUserValidator>().
AddErrorDescriber<CustomIdentityErrorDescriber>().
AddEntityFrameworkStores<SemerkandDergilikContext>().
AddDefaultTokenProviders();


/* Cookie */
CookieBuilder cookieBuilder = new CookieBuilder();

cookieBuilder.Name = "SemerkandBlog";
cookieBuilder.HttpOnly = false;
cookieBuilder.SameSite = SameSiteMode.Lax; // SameSiteMode.Strict: Siteler arasý istek hýrsýzlýðý önler güvenlik önemlidir...Lax kullanýmý cookie deðerini alt sayfalarýnda da kullanabilmek için.. güvenlik önemsenmez
cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest; // always: browser, cookie'yi sunucuya sadece https üzerinden istek gelirse gönderir/çalýþýr.. SameAsRequest:  browser, cookie'yi sunucuya sadece https üzerinden istek gelirse https olarak http ise http olarak gönderir/çalýþýr..

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = new PathString("/Home/Login"); // giriþ yapmadan üyelerin gezinebildiði sayfalara girerse kullanýcý buraya yönlendirilir..
    
    //opts.LogoutPath = new PathString("/Member/LogOut");
    
    opts.Cookie = cookieBuilder; // *** 
    
    opts.SlidingExpiration = true; // 30 gün dolmadan 15.gün sonrasýnda kullanýcý tekrar login olursa ExpireTimeSpan 30 olarak tekrar güncellenir
    opts.ExpireTimeSpan = System.TimeSpan.FromDays(30); // cookie süresi.. bitince tekrar login olmak zorunda
    
    //opts.AccessDeniedPath = new PathString("/Member/AccessDenied");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStatusCodePages();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
            name: "MyAreaAdmin",
            areaName: "Admin",
            pattern: "Admin/{controller=Admin}/{action=Index}/{id?}");

app.Run();
