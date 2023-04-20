using Business.Interfaces;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjectSpiritualArtCore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace ProjectSpiritualArtCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServices<Planes> _Planes;
        private readonly IServices<Obras> _Obras;
        private readonly UserManager<Users> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            IServices<Planes> Planes,
            IServices<Obras> Obras,
            UserManager<Users> userManager)

        {
            _logger = logger;
            _Planes = Planes;
            _Obras = Obras;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> MyArtworks()
        {
            var user = await _userManager.GetUserAsync(User);

            var Galerys = await _Obras.Filters(o => o.IdUser == user.Id);

            var myGalery =
                (
                     from lstGalerys in Galerys
                     select new ObrasViewModel
                     {
                         IdObra = lstGalerys.IdObra,
                         Nombre = lstGalerys.Nombre,
                         Descripcion = lstGalerys.Descripcion,
                         Cantidad = lstGalerys.Cantidad,
                         Estado = lstGalerys.Estado,
                         RutaImagen = lstGalerys.RutaImagen,
                         IdUser = lstGalerys.IdUser,
                         IdCategoria = lstGalerys.IdCategoria

                     }
                ).ToList();

            return Json(new { data = myGalery});
        }
        public IActionResult MyGalery()
        {
            return View();
        }



        public async Task<IActionResult> OfferView()
        {
            var offers = await _Planes.GetAll();

            return View(offers);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> PagoPlan(PagoViewModel viewModel)
        {
            bool status = false;
            string respuesta = string.Empty;

            using (var client = new HttpClient())
            {
                var userName = "Aedtf_Rt5Y4MwJCsIph_C3re97TvnicO-cyxmTM_VRPsG1y-eUXREfVXpzPrhH1zZZkcNuRnb1QTYbmC";
                var password = "EFfc3lvPYwDO7FJBZuFg2BhZWb3EfUz-KLt_KfKKU_HzT1pHJPnG87BtMtlpopXbwJlSK-GGA5RaEqb7";

                client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");

                var authToken = Encoding.ASCII.GetBytes($"{userName}:{password}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var orden = new OrdenPlanesViewModel()
                {
                    intent = "CAPTURE",
                    purchase_units = new List<PurchaseUnit>()
                      {
                          new PurchaseUnit()
                          {
                              amount = new Amount()
                              {
                                  currency_code = "USD",
                                  value = viewModel.Precio
                              },
                              description = viewModel.Plan
                          }
                      },
                    application_context = new ApplicationContext()
                    {
                        brand_name = "SpiritualArtCore.com",
                        landing_page = "NO_PREFERENCE",
                        user_action = "PAY_NOW",
                        return_url = "https://localhost:44329/Principal/IniciarSesionPago",
                        cancel_url = "https://localhost:44329/Principal/Index"

                    }
                };

                var json = JsonConvert.SerializeObject(orden);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);
                status = response.IsSuccessStatusCode;

                if (status)
                {
                    respuesta = response.Content.ReadAsStringAsync().Result;
                }

            }

            return Json(new { status = status, respuesta = respuesta });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
