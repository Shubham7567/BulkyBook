using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            var objCoverType = new CoverType();
            if (id == null)
            {
                return View(objCoverType);
            }
            objCoverType = _unitOfWork.CoverType.Get(id.GetValueOrDefault());
            if (objCoverType == null)
            {
                return NotFound();
            }
            return View(objCoverType);
        }
        [HttpPost]
        public IActionResult Upsert(CoverType objCoverType)
        {
            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Name",objCoverType.Name);
                if (objCoverType.Id == 0)
                {
                    //_unitOfWork.CoverType.Add(objCoverType);
                    _unitOfWork.SP_Call.Execute(SD.Proc_Cover_Type_Create,parameter);
                }
                else {
                    parameter.Add("@Id", objCoverType.Id);
                    //_unitOfWork.CoverType.Update(objCoverType);
                    _unitOfWork.SP_Call.Execute(SD.Proc_Cover_Type_Update, parameter);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        #region API CALL
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SP_Call.List<CoverType>(SD.Proc_Cover_Type_GetAll,null);//.CoverType.GetAll();
            return Json(new { data = allObj});
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id",id);
            var objFromDb = _unitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_Cover_Type_Get, parameter);//.CoverType.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Cover Type not found!" });
            }
            //_unitOfWork.CoverType.Remove(objFromDb);
            _unitOfWork.SP_Call.Execute(SD.Proc_Cover_Type_Delete, parameter);
            _unitOfWork.Save();
            return Json(new { success = true, message="Cover type deleted successfully!"});
        }
        #endregion
    }
}
