using BEEERP.Models.AITS_Models;
using BEEERP.Models.CommonInformation;
using BEEERP.Models.CustomAttribute;
using BEEERP.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BEEERP.Controllers.AITS_Controllers
{
    [ShowNotification]
    public class AITS_SaleEntryController : Controller
    {
        
        BEEContext context = new BEEContext();
        // GET: AITS_SaleEntry
        public ActionResult AITS_SaleEntry()
        {
            ViewBag.employees = LoadComboBox.LoadAllAITS_Employees();
            ViewBag.codes = LoadComboBox.LoadAllAITS_Codes(3);
            var items = context.AITS_SaleEntry.ToList();
            if (items.Count() != 0)
            {
                items.ForEach(x =>
                {
                    x.CodeName = context.AITS_Codes.FirstOrDefault(p => p.CodeID == x.CodeID).CodeName;
                    x.EmployeeName = context.AITS_Employee.FirstOrDefault(p=>p.EmpID==x.EmployeeID).EmployeeName;
                });
            }
            ViewBag.SalesEntry = items;

            var id = context.AITS_SaleEntry.ToList().OrderBy(x => x.ID).LastOrDefault();
            if (id != null)
            {
                ViewBag.EmpID = id.ID + 1;
            }
            else
            {
                ViewBag.EmpID = 1;
            }
            return View();
        }
        [HttpPost]
        public ActionResult SaveEmployee(AITS_SaleEntry employee)
        {

            //if (ModelState.IsValid)
            //{
            if (employee.ID == 0 || employee == null)
            {
                return RedirectToAction("AITS_SaleEntry");
            }
            else
            {
                var prevZone = context.AITS_SaleEntry.AsNoTracking().FirstOrDefault(m => m.ID == employee.ID);
                if (prevZone == null)
                {
                    context.AITS_SaleEntry.Add(employee);
                    context.SaveChanges();
                    return RedirectToAction("AITS_SaleEntry");
                }
                else
                {
                    context.Entry<AITS_SaleEntry>(employee).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("AITS_SaleEntry");
                }
            }


            //}
            //else
            //{
            //    ViewBag.employees = LoadComboBox.LoadAllAITS_Employees();
            //    var Employee = context.AITS_Employee.ToList();
            //    Employee.ForEach(x =>
            //    {
            //        x.RefeneceName = context.AITS_Employee.FirstOrDefault(m => m.EmpID == x.ReferenceEmpID).EmployeeName;
            //    });
            //    ViewBag.Employee = Employee;

            //    var id = context.AITS_Employee.ToList().OrderBy(x => x.EmpID).LastOrDefault();
            //    if (id != null)
            //    {
            //        ViewBag.EmpID = id.EmpID + 1;
            //    }
            //    else
            //    {
            //        ViewBag.EmpID = 1;
            //    }
            //    return View("AITS_Employee");
            //}
        }
        public ActionResult GetEmployee(int ID)
        {
            if (ID == 0)
            {
                return Json(new { Message = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var employee = context.AITS_SaleEntry.FirstOrDefault(m => m.ID == ID);
                employee.Designation= (from p in context.AITS_EmployeeDesignation
                                       join c in context.AITS_Designations on p.DesignationID equals c.ID
                                       where p.EmpID == ID
                                       select c.DesignationName).FirstOrDefault();
                return Json(new { employee = employee }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult GetEmployeeInfo(int ID)
        {
            if (ID == 0)
            {
                return Json(new { Message = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var employee = context.AITS_EmployeeDesignation.FirstOrDefault(m => m.EmpID == ID);
                var codes = LoadComboBox.LoadAllAITS_codesbyemp(ID);
                employee.DesignationName = (from p in context.AITS_EmployeeDesignation
                                        join c in context.AITS_Designations on p.DesignationID equals c.ID
                                        where p.EmpID == ID
                                        select c.DesignationName).FirstOrDefault();
                return Json(new { employee = employee,codes=codes }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DeleteEmployeeByid(int id)
        {
            var str = context.AITS_SaleEntry.ToList().Find(x => x.ID == id);

            context.AITS_SaleEntry.Remove(str);
            context.SaveChanges();
            return Json(id, JsonRequestBehavior.AllowGet);
        }
    }
}