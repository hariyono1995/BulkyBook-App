using FinalBulkyBook.DataAccess.Repository.IRepository;
using FinalBulkyBook.Models;
using FinalBulkyBook.Data;
using FinalBulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalBulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnityOfWork _unitOfWork;

        public CoverTypeController(IUnityOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //var objCategoriesList = _db.Categories.ToList();
            IEnumerable<CoverType> objCoverTypesList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypesList);
        }

        //Get
        public IActionResult Create()
        {
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Successfully create cover type.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        //Get
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(d => d.Id == id);

            if (coverTypeFromDb == null) return NotFound();

            return View(coverTypeFromDb);
        }

        //Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
           if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Successfully update cover type.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(d => d.Id == id);

            if (coverTypeFromDb == null) return NotFound();

            return View(coverTypeFromDb);
        }

        //Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(d => d.Id == id);

            if (obj != null)
            {
                _unitOfWork.CoverType.Remove(obj);
                _unitOfWork.Save();
                TempData["success"] = "Successfully delete cover type.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}
