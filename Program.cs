using BeSkSalon.Data;
using BeSkSalon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData(services);
}
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
async Task SeedData(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    string[] roleNames = { "Admin", "Klijent" };
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
    var adminEmail = "admin@besk.ba";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var newAdminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(newAdminUser, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdminUser, "Admin");
        }
    }
    if (!context.Frizeri.Any())
    {
        context.Frizeri.AddRange(
            new Frizer { Ime = "Marko Marković", Tip = "Muški" },
            new Frizer { Ime = "Ana Anić", Tip = "Ženski" }
        );
        await context.SaveChangesAsync();
    }
    if (!context.Usluge.Any() && context.Frizeri.Any())
    {
        var muskiFrizer = context.Frizeri.First(f => f.Tip == "Muški");
        var zenskiFrizer = context.Frizeri.First(f => f.Tip == "Ženski");
        context.Usluge.AddRange(
            new Usluga { Naziv = "Muško šišanje", Trajanje = 30, Cijena = 10.00m, FrizerId = muskiFrizer.Id },
            new Usluga { Naziv = "Šišanje brade", Trajanje = 20, Cijena = 7.00m, FrizerId = muskiFrizer.Id },
            new Usluga { Naziv = "Šišanje + brada", Trajanje = 45, Cijena = 15.00m, FrizerId = muskiFrizer.Id },
            new Usluga { Naziv = "Žensko šišanje", Trajanje = 45, Cijena = 20.00m, FrizerId = zenskiFrizer.Id },
            new Usluga { Naziv = "Feniranje", Trajanje = 30, Cijena = 15.00m, FrizerId = zenskiFrizer.Id },
            new Usluga { Naziv = "Farbanje kose", Trajanje = 90, Cijena = 40.00m, FrizerId = zenskiFrizer.Id },
            new Usluga { Naziv = "Prelijevanje", Trajanje = 120, Cijena = 60.00m, FrizerId = zenskiFrizer.Id },
            new Usluga { Naziv = "Tretman kose", Trajanje = 60, Cijena = 25.00m, FrizerId = zenskiFrizer.Id }
        );
        await context.SaveChangesAsync();
    }
}