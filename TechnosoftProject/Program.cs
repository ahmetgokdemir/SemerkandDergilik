using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Project.BLL.ManagerServices.Abstracts;
using Project.BLL.ManagerServices.Concretes;
using Project.BLL.ServiceExtensions;
using Project.DAL.Context;
using Project.DAL.Repositories.Abstracts;
using Project.DAL.Repositories.Concretes;
using Project.DAL.Strategy;
using Project.ENTITIES.Identity_Models;
using Technosoft_Project.ClaimProvider;
using Technosoft_Project.CustomValidation;
using Technosoft_Project.Helper;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// builder.Services.AddDbContextService();

/*builder.Services.AddDbContext<TechnosoftProjectContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MyConnection");
    options.UseSqlServer(connectionString);
});*/

/*
builder.Services.AddDbContext<TechnosoftProjectContext>(opts =>
        opts.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"),
            options => options.MigrationsAssembly("EFCoreApp")));
*/

//////////////////////////////////////////////////////////////////////////
// Program.cs'in 1.par�as� Configuration
//////////////////////////////////////////////////////////////////////////
///


builder.Services.AddDbContext<TechnosoftProjectContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
    // opts.UseSqlServer(configuration["ConnectionStrings:DefaultAzureConnectionString"]);
});

// AddScoped veya AddTransient eklendi.. 
// policy of customized claims that used in MemberController (IActionResult IstanbulPage())
builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("IstanbulPolicy", policy =>
    {
        policy.RequireClaim("city", "istanbul"); // claimprovider.cs
        //policy.RequireClaim("city");
        //policy.RequireRole("Admin"); 
        // policy.RequireRole("admin"); hata verir case sensitive

        // ClaimProvider.cs'de Claim CityClaim = new Claim("city", user.City, ClaimValueTypes.String, "Internal");
    });

    opts.AddPolicy("ViolencePolicy", policy =>
    {
        policy.RequireClaim("violence"); // claimprovider.cs

        // Claim ViolenceClaim = new Claim("violence", true.ToString(), ClaimValueTypes.String, "Internal");
    });

    opts.AddPolicy("ExchangePolicy", policy =>
    {
        policy.AddRequirements(new Technosoft_Project.ClaimProvider.ExpireDateExchangeRequirement()); // requirement.cs
    });

});


// install Microsoft.AspNetCore.Authentication.Facebook from NugET
// install Microsoft.AspNetCore.Authentication.Google from NugET
// kaynak: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/social-without-identity?view=aspnetcore-6.0
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = MicrosoftAccountDefaults.AuthenticationScheme;


    })
    .AddCookie()
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    }).AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientID"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    }).AddMicrosoftAccount(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"]; 
    });

builder.Services.AddRazorPages();


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
AddEntityFrameworkStores<TechnosoftProjectContext>().
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

    //  signInManager.SignOutAsync(); bu kod a�a��daki kodu, bu kod da asp-route-returnUrl="/Ho... tetikler..
    opts.LogoutPath = new PathString("/Member/LogOut"); //  asp-route-returnUrl="/Home/Index" tetiklenir.. MemberLayout.cs'de

    opts.Cookie = cookieBuilder; // *** 
    
    opts.SlidingExpiration = true; // 30 g�n dolmadan 15.g�n sonras�nda kullan�c� tekrar login olursa ExpireTimeSpan 30 olarak tekrar g�ncellenir
    opts.ExpireTimeSpan = System.TimeSpan.FromDays(30); // cookie s�resi.. bitince tekrar login olmak zorunda
    
    opts.AccessDeniedPath = new PathString("/Member/AccessDenied"); // bu path (Access Denied) eri�im yetkisi olmayan kullan�c�y� sayfadan Access Denied etme
});


//  Technosoft_Project.ClaimProvider kullan�ld���ndan using gerek kalmad�
builder.Services.AddScoped<IClaimsTransformation, Technosoft_Project.ClaimProvider.ClaimProvider>();

builder.Services.AddTransient<IAuthorizationHandler, ExpireDateExchangeHandler>();
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("TwoFactorOptions"));
// wski versiyon =>  services.Configure<TwoFactorOptions>(configuration.GetSection("TwoFactorOptions"));
builder.Services.AddScoped<EmailConfirmation>();
builder.Services.AddScoped<PasswordReset>();


//Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ICategory_of_FoodRepository, Category_of_FoodRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

//Managers
builder.Services.AddScoped(typeof(IManager<>), typeof(BaseManager<>));
builder.Services.AddScoped<ICategory_of_FoodManager, Category_of_FoodManager>();
builder.Services.AddScoped<IProductManager, ProductManager>();
builder.Services.AddScoped<IBlogManager, BlogManager>();



builder.Services.AddSession();

// builder.Services.AddAutoMapper();

/*
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.Name = "MainSession";
});
*/

// create database if not exist
var app = builder.Build();

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<TechnosoftProjectContext>();
        //context.Database.EnsureCreated();
    }



// create some default data if not exist in database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        await SeedRoles.Seed(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

//////////////////////////////////////////////////////////////////////////
// Program.cs'in 2.par�as� Middleware
//////////////////////////////////////////////////////////////////////////
///

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
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
            name: "MyAreaAdmin",
            areaName: "Admin",
            pattern: "Admin/{controller=Admin}/{action=Index}/{id?}");

app.Run();
