using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));

builder.Services.AddIdentity<AppUser, IdentityRole>(opts =>
    {
        opts.SignIn.RequireConfirmedEmail = true;
        opts.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
    }).AddEntityFrameworkStores<AppDbContext>()
      .AddDefaultTokenProviders()
      .AddTokenProvider<CustomEmailConfirmationTokenProvider<AppUser>>("CustomEmailConfirmation");

builder.Services.Configure<DataProtectionTokenProviderOptions>(opts =>
            opts.TokenLifespan = TimeSpan.FromHours(5));

builder.Services.Configure<CustomEmailConfirmationTokenProviderOptions>(opts =>
        opts.TokenLifespan = TimeSpan.FromDays(3));

builder.Services.AddAuthentication()
    .AddGoogle(opts =>
    {
        opts.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
        opts.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
    })
    .AddFacebook(opts =>
    {
        opts.ClientId = builder.Configuration["Authentication:Facebook:ClientId"] ?? "";
        opts.ClientSecret = builder.Configuration["Authentication:Facebook:ClientSecret"] ?? "";
    });

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IMailService, MailService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization();

app.Run();
