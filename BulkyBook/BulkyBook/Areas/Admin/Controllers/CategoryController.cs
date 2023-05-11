using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if ((obj.Name != null) && (obj.DisplayOrder.ToString() != null))
            {
                if (obj.Name.Equals(obj.DisplayOrder.ToString()))
                {
                    ModelState.AddModelError("SameNameError", "Name can't be same as Display Order");
                }
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb= _unitOfWork.Category.GetFirstOrDefault(cat=>cat.Id==id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if ((obj.Name != null) && (obj.DisplayOrder.ToString() != null))
            {
                if (obj.Name.Equals(obj.DisplayOrder.ToString()))
                {
                    ModelState.AddModelError("SameNameError", "Name can't be same as Display Order");
                }
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully";

                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb= _unitOfWork.Category.GetFirstOrDefault(cat=>cat.Id==id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        //POST
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(cat => cat.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["delete"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
