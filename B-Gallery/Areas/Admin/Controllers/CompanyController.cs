using B_Gallery.DataAccess.Repository.IRepository;
using B_Gallery.Models;
using Microsoft.AspNetCore.Mvc;

namespace B_Gallery.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View(new List<Company>());
        }

        public JsonResult GetAllCompanies()
        {
            var obj = unitOfWork.Company.GetAll();
            return Json(new { data = obj });
        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    byte[] p = null;
                    using (var fs = files[0].OpenReadStream())
                    {
                        using (var ms = new MemoryStream())
                        {
                            fs.CopyTo(ms);
                            p = ms.ToArray();
                        }
                    }
                    company.Image = p;
                }

                unitOfWork.Company.Add(company);
                unitOfWork.Save();
                TempData["Success"] = "Company Created Succesfully!";
                return RedirectToAction("Index");
                
            }
            return View(new Company());
        }

        public IActionResult Edit(int? id)
        {
            var obj = unitOfWork.Company.GetById(id.GetValueOrDefault());
            if(obj == null)
            {
                return View(new Company());
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    byte[] p = null;
                    using (var fs = files[0].OpenReadStream())
                    {
                        using (var ms = new MemoryStream())
                        {
                            fs.CopyTo(ms);
                            p = ms.ToArray();
                        }
                    }
                    company.Image = p;
                }
                unitOfWork.Company.Update(company);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(company);

        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var obj = unitOfWork.Company.GetById(id);
            unitOfWork.Company.Remove(obj);
            unitOfWork.Save();
            return Json(new { message = "Deleted Successfully!" });
        }
    }
}
