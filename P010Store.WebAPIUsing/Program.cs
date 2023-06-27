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
builder.Services.AddSession();// Uygulamam�zda session kullanmam�z gerekirse
builder.Services.AddHttpClient();// Web API yi kullanabilmemiz i�in gerekli servisimiz
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


//Authentication : Outurum a�ma servisi
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Login";//Giri� yapma sayfas�
    x.AccessDeniedPath = "/AccessDenied";// Giri� yapan kullan�c�n�n admin yetkisi yok ise AccessDenied sayfas�na y�nlendirir
    x.LogoutPath = "/Admin/Login/Logout";//��k�� sayfas�
    x.Cookie.Name = "Administrator";//olu�acak cookinin ad�
    x.Cookie.MaxAge = TimeSpan.FromDays(1);// olu�acak cookie nin ya�am s�resi
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
app.UseSession();// Uygulamam�zda session kullanmam�z gerekirse

app.UseAuthentication();//�nce oturum a�ma
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
