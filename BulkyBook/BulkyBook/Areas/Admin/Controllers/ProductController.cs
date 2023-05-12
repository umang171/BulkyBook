using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM();
            productVM.Product = new Product();
            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(
                category => new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                }
                );
            productVM.CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                cover => new SelectListItem
                {
                    Text = cover.Name,
                    Value = cover.Id.ToString()
                }
                );
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            productVM.Product = _unitOfWork.Product.GetFirstOrDefault(prod => prod.Id == id);
            return View(productVM);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"images\products\" + fileName + extension;
                }
                _unitOfWork.Product.Update(obj.Product);

                _unitOfWork.Save();
                TempData["success"] = "Product Updated Successfully";

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        #region API CALLS   
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll("Category,CoverType");
            return Json(new { data = productList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(prod => prod.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
                return Json(new { success = true, message = "Successfully deleted" });
        }
        #endregion
    }
}
