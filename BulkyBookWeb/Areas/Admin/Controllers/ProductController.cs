using FinalBulkyBook.DataAccess.Repository.IRepository;
using FinalBulkyBook.Data;
using FinalBulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FinalBulkyBook.Models.ViewModels;
using Microsoft.Extensions.Hosting.Internal;

namespace FinalBulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnityOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnityOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            //var objCategoriesList = _db.Categories.ToList();
            //IEnumerable<Product> objProductsList = _unitOfWork.Product.GetAll();
            //return View(objProductsList);

            return View();
        }

        //Get
        public IActionResult Upsert(int? id)
        {

            //Make select dropdown use ViewData and ViewBag
            /* 
            Product product = new();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            */

            //Make select dropdown use ViewModel
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
        };


            if (id == null || id == 0)
            {
                //Create
                //Save list to viewbag to select dropdown
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;

                return View(productVM);
            } 
            else
            {
                //Update
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                return View(productVM);
            }

        }

        //Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"public\images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if(obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));

                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension ), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\public\images\products\" + fileName + extension;
                }

                if (obj.Product.Id == 0)
                {
                    //Create
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "Successfully created category";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0) return NotFound();

        //    var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(d => d.Id == id);

        //    if (categoryFromDb == null) return NotFound();

        //    return View(categoryFromDb);
        //}

        #region Api for DataTable
        [HttpGet]
        public IActionResult GetAll()
        {
            var objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = objProductList });
        }

        //Delete
        //[HttpDelete]
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(d => d.Id == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if(System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new {success = true, message = "Deleted successfully."});
        }
        #endregion
    }
}
