using P010Store.Data;
using P010Store.Data.Abstract;
using P010Store.Data.Concrete;
using P010Store.Entities;
using P010Store.Service.Abstract;
using P010Store.Service.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies; // oturum i�lemi i�in gerekli k�t�phane
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseContext>(); // Entityframework i�lemlerini yapabilmek i�in bu sat�r� ekliyoruz

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient(typeof(IService<>), typeof(Service<>));// Veritaban� i�lemleri yapaca��m�z servisleri ekledik. Burada .net core a e�er sana
//  IService interface i kullanma iste�i gelirse Service s�n�f�ndan bir nesne olu�tur demi� olduk.
//  .net core de 3 farkl� y�ntemle servisleri ekleyebiliyoruz:
//  builder.Services.AddSingleton(); : AddSingleton kullanarak olu�turdu�umuz nesneden bir tane �rnek olu�ur ve her seferinde bu �rnek kullan�l�r
//  builder.Services.AddTransient(); : y�nteminde ise �nceden olu�mu� nesne varsa o kullan�l�r yoksa yenisi olu�turulur.
//  builder.Services.AddScoped(); : y�nteminde ise yap�lan her istek i�in yeni bir nesne olu�turulur.

builder.Services.AddTransient<IProductService, ProductService>();// producta �zel yazd���m�z servis 
builder.Services.AddTransient<ICategoryService, CategoryService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();//IHttpContextAccessor ile uygulama i�erisindeki giri� yapan kullan�c�, session verileri, cookie ler gibi
                                                                           //gibi i�eriklere view lardan veya controller dan ula�abilmemizi sa�lar

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

//Authenticaton : oturum a�ma-giri� yapma
app.UseAuthentication(); // Admin login i�in 
//Authorization : yetkinlendirme (oturum a�an kullan�c�n�n admine giri� yetkisi var m�)
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


//Katmanl� Mimari �li�ki Hiyerar�isi

/*
 Web UI �zerinde veritaban� i�lemleri yapabilmek i�in WebUI �n dependencies(referansalr�na) Service katman�n� dependencies e sa� t�klay�p add project references diyerek a��lan 
 pencereden Service katman�na tik koyup ok butonuyla pencereyi kapat�p ba�lant� kurduk

 Service katman� da veritaban� i�lemlerini yapabilmek i�in Data katman�na eri�mesi gerekiyor, yine dependencies e sa� t�klay�p add project references diyerek a��lan pencereden
 Data k�sm�na i�aret koyup ekliyoruz

 Data katman�n�n da entity lere eri�mesi gerekiyor ki class lar� kullanarak veritaban� i�lemleri yapabilsin. Yine ayn� yolu izleyerek veya class lar�n �zerine gelip ampul e t�klay�p 
 add project references diyerek data dan entities e eri�im vermemiz gerekir.
 */ 