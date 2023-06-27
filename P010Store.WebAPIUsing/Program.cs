using Microsoft.AspNetCore.Authentication.Cookies;
using P010Store.Data;
using P010Store.Service.Abstract;
using P010Store.Service.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddTransient(typeof(IService<>), typeof(Service<>));
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddSession();// Uygulamamýzda session kullanmamýz gerekirse
builder.Services.AddHttpClient();// Web API yi kullanabilmemiz için gerekli servisimiz
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


//Authentication : Outurum açma servisi
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Login";//Giriþ yapma sayfasý
    x.AccessDeniedPath = "/AccessDenied";// Giriþ yapan kullanýcýnýn admin yetkisi yok ise AccessDenied sayfasýna yönlendirir
    x.LogoutPath = "/Admin/Login/Logout";//Çýkýþ sayfasý
    x.Cookie.Name = "Administrator";//oluþacak cookinin adý
    x.Cookie.MaxAge = TimeSpan.FromDays(1);// oluþacak cookie nin yaþam süresi
});


builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", policy => policy.RequireClaim("Role", "Admin"));
    x.AddPolicy("UserPolicy", policy => policy.RequireClaim("Role", "User"));
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
app.UseStaticFiles();

app.UseRouting();
app.UseSession();// Uygulamamýzda session kullanmamýz gerekirse

app.UseAuthentication();//önce oturum açma
app.UseAuthorization();//sonra yetkilendirme

app.MapControllerRoute(
            name: "admin",
            pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "custom",
    pattern: "{customurl?}/{controller=Home}/{action=Index}/{id?}");

app.Run();
