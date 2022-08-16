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

    // Backend i�in haz�rlanan validationlar (e�er kullan�c� browser'�n client validation� kapat�rsa tedbiren backend k�sm� eklendi..)
    // User ile alakal� default gelen validation'lar
    opts.User.RequireUniqueEmail = true; // Email is already taken  �eklinde hata verir..
    opts.User.AllowedUserNameCharacters = "abc�defg�h�ijklmno�pqrs�tu�vwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";

    // password ile alakal� default gelen validation'lar
    opts.Password.RequiredLength = 8; // default de�eri 6
    opts.Password.RequireNonAlphanumeric = true; // default de�eri true */_ gibi de�erleri temsil eder..
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
cookieBuilder.SameSite = SameSiteMode.Lax; // SameSiteMode.Strict: Siteler aras� istek h�rs�zl��� �nler g�venlik �nemlidir...Lax kullan�m� cookie de�erini alt sayfalar�nda da kullanabilmek i�in.. g�venlik �nemsenmez
cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest; // always: browser, cookie'yi sunucuya sadece https �zerinden istek gelirse g�nderir/�al���r.. SameAsRequest:  browser, cookie'yi sunucuya sadece https �zerinden istek gelirse https olarak http ise http olarak g�nderir/�al���r..

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = new PathString("/Home/Login"); // giri� yapmadan �yelerin gezinebildi�i sayfalara girerse kullan�c� buraya y�nlendirilir..
    
    //opts.LogoutPath = new PathString("/Member/LogOut");
    
    opts.Cookie = cookieBuilder; // *** 
    
    opts.SlidingExpiration = true; // 30 g�n dolmadan 15.g�n sonras�nda kullan�c� tekrar login olursa ExpireTimeSpan 30 olarak tekrar g�ncellenir
    opts.ExpireTimeSpan = System.TimeSpan.FromDays(30); // cookie s�resi.. bitince tekrar login olmak zorunda
    
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
