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
    public class EmployeeDesignationController : Controller
    {
        
        BEEContext context = new BEEContext();
        // GET: EmployeeDesignation
        public ActionResult EmployeeDesignation()
        {
            ViewBag.FirstCode = LoadComboBox.LoadAllAITS_Codes(1);
            ViewBag.SecondCode = LoadComboBox.LoadAllAITS_Codes(2);
            ViewBag.Designation =LoadComboBox.LoadAllAITS_Designation();
            var allEmployeeDesg = context.AITS_EmployeeDesignation.ToList();
            if (allEmployeeDesg != null)
            { 
                allEmployeeDesg.ForEach(x =>
                {
                    x.DesignationName = context.AITS_Designations.FirstOrDefault(p => p.ID == x.DesignationID).DesignationName;
                    x.EmployeeName = context.AITS_Employee.FirstOrDefault(p => p.EmpID == x.EmpID).EmployeeName;
                    x.FirstCodeName = context.AITS_Codes.FirstOrDefault(p => p.CodeID == x.FirstCode).CodeName;
                    x.SecondCodeName = context.AITS_Codes.FirstOrDefault(p => p.CodeID == x.SecondCode).CodeName;
                });
            }
            ViewBag.EmployeeDesignation = allEmployeeDesg;
            ViewBag.employees = LoadComboBox.LoadAllAITS_Employees();
            var id = context.AITS_EmployeeDesignation.ToList().OrderBy(x => x.ID).LastOrDefault();
            if (id != null)
            {
                ViewBag.ID = id.ID + 1;
            }
            else
            {
                ViewBag.ID = 1;
            }

            return View();
        }
        [HttpPost]
        public ActionResult SaveEmployeeDesignation(AITS_EmployeeDesignation employeedesignation)
        {

            if (ModelState.IsValid)
            {
                if (employeedesignation.ID == 0 || employeedesignation == null)
                {
                    return RedirectToAction("EmployeeDesignation");
                }
                else
                {
                    var prevZone = context.AITS_EmployeeDesignation.AsNoTracking().FirstOrDefault(m => m.ID == employeedesignation.ID);
                    if (prevZone == null)
                    {
                        employeedesignation.FirstCode = 1;
                        context.AITS_EmployeeDesignation.Add(employeedesignation);
                        context.SaveChanges();
                        return RedirectToAction("EmployeeDesignation");
                    }
                    else
                    {
                        employeedesignation.FirstCode = 1;
                        context.Entry<AITS_EmployeeDesignation>(employeedesignation).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        return RedirectToAction("EmployeeDesignation");
                    }
                }


            }
            else
            {
                ViewBag.Designation = context.AITS_EmployeeDesignation.ToList();
                var id = context.AITS_EmployeeDesignation.ToList().OrderBy(x => x.ID).LastOrDefault();
                if (id != null)
                {
                    ViewBag.ID = id.ID + 1;
                }
                else
                {
                    ViewBag.ID = 1;
                }
                return View("EmployeeDesignation");
            }
        }

        public ActionResult GetEmployeeDesignationforSelect(int id)
        {
            var DesignationID = 0;
            if (id == 0)
            {
                return Json(new { Message = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var associateEmp = context.AITS_Employee.Where(x => x.ReferenceEmpID == id).ToList();
                associateEmp.ForEach(x =>
                {
                    x.DesignationID = context.AITS_EmployeeDesignation.FirstOrDefault(p => p.EmpID == x.EmpID).DesignationID;
                });
                
                DesignationID = GenerateDesignationOnCondition(associateEmp);
                
                return Json(new { DesignationID = DesignationID }, JsonRequestBehavior.AllowGet);
            }

        }

        public int GenerateDesignationOnCondition(List<AITS_Employee> associateEmp)
        {
            
            var q = from x in associateEmp
                    group x by x.DesignationID into g
                    let count = g.Count()
                    orderby count descending
                    select new { Value = g.Key, Count = count };
            var conditionB = q.FirstOrDefault(x => x.Count == 2 && x.Value == 1);
            var conditionC = q.FirstOrDefault(x => x.Count == 3 && x.Value == 2);
            var conditionD = q.FirstOrDefault(x => (x.Count == 1 && x.Value == 3) && (x.Count == 2 && x.Value == 2));
            var conditionE = q.FirstOrDefault(x => (x.Count == 3 && x.Value == 3) && (x.Count == 3 && x.Value == 2));
            var conditionF = q.FirstOrDefault(x => (x.Count == 2 && x.Value == 5) && (x.Count == 1 && x.Value == 3) && (x.Count == 2 && x.Value == 2));
            var conditionG = q.FirstOrDefault(x => x.Count == 4 && x.Value == 5);
            var conditionH = q.FirstOrDefault(x => x.Count == 6 && x.Value == 5);
            var conditionI = q.FirstOrDefault(x => x.Count == 6 && x.Value == 8);
            var conditionJ = q.FirstOrDefault(x => x.Count == 3 && x.Value == 9);
            var conditionK = q.FirstOrDefault(x => x.Count == 2 && x.Value == 10);

            var desg=conditionB != null ?  2 :
                conditionC != null ?  3 :
                conditionD != null ?  4 :
                conditionE != null ?  5 :
                conditionF != null ?  6 :
                conditionG != null ?  7 :
                conditionH != null ?  8 :
                conditionI != null ?  9 :
                conditionJ != null ?  10 :
                conditionK != null ?  11 :1;
            return desg;
        }


        public ActionResult GetEmployeeDesignation(int ID)
        {
            if (ID == 0)
            {
                return Json(new { Message = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var designation = context.AITS_EmployeeDesignation.FirstOrDefault(m => m.ID == ID);
                designation.EmployeeName = context.AITS_Employee.FirstOrDefault(x => x.EmpID == designation.EmpID).EmployeeName;
                designation.DesignationName = context.AITS_Designations.FirstOrDefault(x => x.ID == designation.DesignationID).DesignationName;
                return Json(new { designation = designation }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult DeleteEmployeeDesignationByid(int id)
        {
            var str = context.AITS_EmployeeDesignation.ToList().Find(x => x.ID == id);

            context.AITS_EmployeeDesignation.Remove(str);
            context.SaveChanges();
            return Json(id, JsonRequestBehavior.AllowGet);
        }
    }
}