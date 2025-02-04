using EcommerceBookApp.DataAccess.Repository.IRepository;
using EcommerceBookApp.Utility;
using EcommerceBookApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBookApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.ROLE_ADMIN)] //Only allows Admin users to access it.
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> dbCompanyList = _unitOfWork.CompanyRepository.GetAll().ToList();
			return View(dbCompanyList);
        }

		public IActionResult Upsert(int? id)
		{
			if(id==null || id==0)
			{
				//For create
				return View(new Company());
			}
			else
			{
				Company companyObj = _unitOfWork.CompanyRepository.GetFirstOrDefault(u => u.Id == id);
				return View(companyObj);
			}	
			
		}

		[HttpPost]
		public IActionResult Upsert(Company companyObj)
		{
			if (ModelState.IsValid)
			{ 
				if(companyObj.Id ==0)
				{
					_unitOfWork.CompanyRepository.Add(companyObj); //Used to persist the Company dataobject in to the table
				}
				else
				{
					_unitOfWork.CompanyRepository.Update(companyObj);
				}
				_unitOfWork.Save();
				TempData["success"] = "Company created Sucessfully!!";
				return RedirectToAction("Index", "Company"); // Redirects to the Index in Company
			}
			else
			{
				return View(companyObj);
			}
			
		}
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Company? CompanyFromDb = _unitOfWork.CompanyRepository.GetFirstOrDefault(u => u.Id == id); //Find only works on the primary key 
																										
			if (CompanyFromDb == null)
			{
				return NotFound();
			}
			return View(CompanyFromDb);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{
			Company? CompanyObj = _unitOfWork.CompanyRepository.GetFirstOrDefault(u => u.Id == id);
			if (CompanyObj == null)
			{
				return NotFound();
			}
			_unitOfWork.CompanyRepository.Remove(CompanyObj);
			_unitOfWork.Save();
			TempData["success"] = "Company deleted Sucessfully!!";
			return RedirectToAction("Index", "Company");
		}

		#region API CALLS
		[HttpGet]
		public IActionResult GetaLL()
		{
            List<Company> dbCompanyList = _unitOfWork.CompanyRepository.GetAll().ToList();
            return Json(new {data=dbCompanyList});
        }
        #endregion
    }
}
