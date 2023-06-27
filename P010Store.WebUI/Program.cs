using P010Store.Data;
using P010Store.Data.Abstract;
using P010Store.Data.Concrete;
using P010Store.Entities;
using P010Store.Service.Abstract;
using P010Store.Service.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies; // oturum iþlemi için gerekli kütüphane
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseContext>(); // Entityframework iþlemlerini yapabilmek için bu satýrý ekliyoruz

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient(typeof(IService<>), typeof(Service<>));// Veritabaný iþlemleri yapacaðýmýz servisleri ekledik. Burada .net core a eðer sana
//  IService interface i kullanma isteði gelirse Service sýnýfýndan bir nesne oluþtur demiþ olduk.
//  .net core de 3 farklý yöntemle servisleri ekleyebiliyoruz:
//  builder.Services.AddSingleton(); : AddSingleton kullanarak oluþturduðumuz nesneden bir tane örnek oluþur ve her seferinde bu örnek kullanýlýr
//  builder.Services.AddTransient(); : yönteminde ise önceden oluþmuþ nesne varsa o kullanýlýr yoksa yenisi oluþturulur.
//  builder.Services.AddScoped(); : yönteminde ise yapýlan her istek için yeni bir nesne oluþturulur.

builder.Services.AddTransient<IProductService, ProductService>();// producta özel yazdýðýmýz servis 
builder.Services.AddTransient<ICategoryService, CategoryService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();//IHttpContextAccessor ile uygulama içerisindeki giriþ yapan kullanýcý, session verileri, cookie ler gibi
                                                                           //gibi içeriklere view lardan veya controller dan ulaþabilmemizi saðlar

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

//Authenticaton : oturum açma-giriþ yapma
app.UseAuthentication(); // Admin login için 
//Authorization : yetkinlendirme (oturum açan kullanýcýnýn admine giriþ yetkisi var mý)
app.UseAuthorization();

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


//Katmanlý Mimari Ýliþki Hiyerarþisi

/*
 Web UI üzerinde veritabaný iþlemleri yapabilmek için WebUI ýn dependencies(referansalrýna) Service katmanýný dependencies e sað týklayýp add project references diyerek açýlan 
 pencereden Service katmanýna tik koyup ok butonuyla pencereyi kapatýp baðlantý kurduk

 Service katmaný da veritabaný iþlemlerini yapabilmek için Data katmanýna eriþmesi gerekiyor, yine dependencies e sað týklayýp add project references diyerek açýlan pencereden
 Data kýsmýna iþaret koyup ekliyoruz

 Data katmanýnýn da entity lere eriþmesi gerekiyor ki class larý kullanarak veritabaný iþlemleri yapabilsin. Yine ayný yolu izleyerek veya class larýn üzerine gelip ampul e týklayýp 
 add project references diyerek data dan entities e eriþim vermemiz gerekir.
 */ 