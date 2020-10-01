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
    public class AITS_EmployeeController : Controller
    {
        BEEContext context = new BEEContext();
        // GET: AITS_Employee
        public ActionResult AITS_Employee()
        {
            ViewBag.employees = LoadComboBox.LoadAllAITS_Employees();
            var Employee = context.AITS_Employee.ToList();
            Employee.ForEach(x =>
            {
                if (x.ReferenceEmpID!=0)
                {
                    x.RefeneceName = context.AITS_Employee.FirstOrDefault(m => m.EmpID == x.ReferenceEmpID).EmployeeName;
                }
               
            });
            ViewBag.Employee = Employee;

            var id = context.AITS_Employee.ToList().OrderBy(x => x.EmpID).LastOrDefault();
            if (id != null)
            {
                ViewBag.EmpID = id.EmpID + 1;
            }
            else
            {
                ViewBag.EmpID = 1;
            }
            return View();
        }
        [HttpPost]
        public ActionResult SaveEmployee(AITS_Employee employee)
        {

            //if (ModelState.IsValid)
            //{
                if (employee.EmpID == 0 || employee == null)
                {
                    return RedirectToAction("AITS_Employee");
                }
                else
                {
                    var prevZone = context.AITS_Employee.AsNoTracking().FirstOrDefault(m => m.EmpID == employee.EmpID);
                    if (prevZone == null)
                    {
                        context.AITS_Employee.Add(employee);
                        context.SaveChanges();
                        return RedirectToAction("AITS_Employee");
                    }
                    else
                    {
                        context.Entry<AITS_Employee>(employee).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        return RedirectToAction("AITS_Employee");
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
                var employee = context.AITS_Employee.FirstOrDefault(m => m.EmpID == ID);
                if (employee.ReferenceEmpID!=0)
                {
                    employee.RefeneceName = context.AITS_Employee.FirstOrDefault(m => m.EmpID == employee.ReferenceEmpID).EmployeeName;
                }
                
                return Json(new { employee = employee }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult DeleteEmployeeByid(int id)
        {
            var str = context.AITS_Employee.ToList().Find(x => x.EmpID == id);

            context.AITS_Employee.Remove(str);
            context.SaveChanges();
            return Json(id, JsonRequestBehavior.AllowGet);
        }
    }
}