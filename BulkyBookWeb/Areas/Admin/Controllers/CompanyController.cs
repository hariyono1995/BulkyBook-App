using FinalBulkyBook.DataAccess.Repository.IRepository;
using FinalBulkyBook.Data;
using FinalBulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FinalBulkyBook.Models.ViewModels;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Authorization;
using FinalBulkyBook.Utility;

namespace FinalBulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnityOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CompanyController(IUnityOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get
        public IActionResult Upsert(int? id)
        {


            Company company = new();

            if (id == null || id == 0)
            {
                return View(company);
            } 
            else
            {
                //Update
               company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
                return View(company);
            }

        }

        //Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {

            if (ModelState.IsValid)
            {
                string msgAction = string.Empty;
                if (obj.Id == 0)
                {
                    //Create
                    _unitOfWork.Company.Add(obj);
                    msgAction = "Successfully adding data.";
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                    msgAction = "Successfully update data";
                }

                _unitOfWork.Save();
                TempData["success"] = msgAction;
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        #region Api for DataTable
        [HttpGet]
        public IActionResult GetAll()
        {
            var objCompaniesList = _unitOfWork.Company.GetAll();
            return Json(new { data = objCompaniesList });
        }

        //Delete
        //[HttpDelete]
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(d => d.Id == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new {success = true, message = "Deleted successfully."});
        }
        #endregion
    }
}
