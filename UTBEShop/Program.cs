using Microsoft.AspNetCore.Builder;
using UTBEShop.Models.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
using UTBEShop.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using UTBEShop.Models.ApplicationServices.Abstraction;
using UTBEShop.Models.ApplicationServices.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Adding DbContext to our Database and Connection String

builder.Services.AddDbContext<EShopDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MySQL_LOCAL_CONNECTION")));

#endregion

#region Adding Configuration for Idenetity and Rules 
builder.Services.AddIdentity<User,Role>()
    .AddEntityFrameworkStores<EShopDbContext>()
    .AddDefaultTokenProviders();

//Modifciation of Idenetity password and requirements 
builder.Services.Configure<IdentityOptions>(Options =>
{
    Options.Password.RequireDigit = true;
    Options.Password.RequiredLength = 2 ;
    Options.Password.RequireNonAlphanumeric = false;
    Options.Password.RequireUppercase = true;
    Options.Password.RequireLowercase = true;
    Options.Password.RequiredUniqueChars = 1;

    Options.Lockout.AllowedForNewUsers = true;
    Options.Lockout.MaxFailedAccessAttempts = 10;
    Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);

    //unique Email Each User 
    Options.User.RequireUniqueEmail = true;


});
#endregion

#region Configure Application Cookies 
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/Security/Account/Login";
    options.LogoutPath = "/Security/Account/Logout";
  
    options.SlidingExpiration = true;
});
#endregion

#region Add Session and memory configuration 
builder.Services.AddDistributedMemoryCache(); // adds a default in-memory implementation of IDistrubtion 

builder.Services.AddSession(options =>
{
    //Set a short timeout for easy testing 
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly= true;
    //Make the session cokkie essential 
    options.Cookie.IsEssential = true;
});

#endregion

#region Connect the interface with the implementation 

builder.Services.AddScoped<ISecurityApplicationService, SecurityIdentityApplicationService>();
builder.Services.AddControllersWithViews();
#endregion




var app = builder.Build();

#region Adding the admin and manager to database when the app start building between phase of Build and run 
//we used (using) because after what inside using will be used will be delteded or dispacthed
using (var scope = app.Services.CreateScope())
{
    UserManager<User> userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    DatabaseInit databaseInit = new DatabaseInit();
    await databaseInit.EnsureAdminCreated(userManager);
    await databaseInit.EnsureManagerCreated(userManager);
}
#endregion



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();
#region Adding useSession 
app.UseSession();
#endregion
app.UseRouting();




#region We should authenticate user before authorize - Authentication and authorization
app.UseAuthentication();
app.UseAuthorization();
#endregion


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
