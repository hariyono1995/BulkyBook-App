using FinalBulkyBook.DataAccess.Repository.IRepository;
using FinalBulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FinalBulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnityOfWork _unityOfWork;

        public HomeController(ILogger<HomeController> logger, IUnityOfWork unityOfWork)
        {
            _logger = logger;
            _unityOfWork = unityOfWork;
        }

        public IActionResult Index()
        {
            var ProductList = _unityOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return View(ProductList);
        }

        public IActionResult Details(int id)
        {
            ShoopingCart shoopingCart = new()
            {
                Product = _unityOfWork.Product.GetFirstOrDefault(u => u.Id == id, includeProperties: "Category,CoverType"),
                Count = 1
            };
            return View(shoopingCart);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}