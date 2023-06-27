﻿using Microsoft.AspNetCore.Mvc;
using P010Store.Entities;
using P010Store.Service.Abstract;
using P010Store.WebUI.Models;
using P010Store.WebUI.Utils;
using System.Diagnostics;

namespace P010Store.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService<Product> _service;
        private readonly IService<Carousel> _serviceCarousel;
        private readonly IService<Brand> _serviceBrand;
        private readonly IService<Contact> _serviceContact;

        public HomeController(IService<Product> service, IService<Carousel> serviceCarousel, IService<Brand> serviceBrand, IService<Contact> serviceContact)
        {
            _service = service;
            _serviceCarousel = serviceCarousel;
            _serviceBrand = serviceBrand;
            _serviceContact = serviceContact;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomePageViewModel()
            {
                Products = await _service.GetAllAsync(p => p.IsHome),
                Carousels = await _serviceCarousel.GetAllAsync(),
                Brands = await _serviceBrand.GetAllAsync()
             }; 

            return View(model);
        }

        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Route("iletisim")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [Route("iletisim"), HttpPost]
        public async Task<IActionResult> ContactUs(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _serviceContact.Add(contact);
                    _serviceContact.SaveChanges();
                    TempData["Mesaj"] = "<div class='alert alert-success'>Mesajınız Gönderildi. Teşekkürler..</div>";
                    //await MailHelper.SendMailAsync(contact);// ekrandan gönderilen mesajı mail ile gönderme
                    return RedirectToAction("ContactUs");
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Hata Oluştu! Mesajınız Gönderilemedi!");
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}