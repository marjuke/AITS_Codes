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
    public class AITS_AssociateSearchController : Controller
    {
        BEEContext context = new BEEContext();
        // GET: AITS_AssociateSearch
        public ActionResult AITS_AssociateSearch()
        {
            ViewBag.Employee = LoadComboBox.LoadAllAITS_Employees();
            return View();
        }
        public ActionResult SearchAssociate(int EmployeeID)
        {
            var employee = context.AITS_Employee.FirstOrDefault(x => x.EmpID == EmployeeID);
            if (employee!=null)
            {
                var b = context.AITS_EmployeeDesignation.FirstOrDefault(p => p.EmpID == EmployeeID);

                employee.Designation = context.AITS_Designations.FirstOrDefault(p => p.ID == b.DesignationID).DesignationName;
                var associateList = context.AITS_Employee.Where(x => x.ReferenceEmpID == EmployeeID).ToList();
                associateList.ForEach(x =>
                {
                    var a = context.AITS_EmployeeDesignation.FirstOrDefault(p => p.EmpID == x.EmpID);
                    x.code = a.ID.ToString();
                    x.FirstCode = context.AITS_Codes.FirstOrDefault(p => p.CodeID == a.FirstCode).CodeName;
                    x.SecondCode = context.AITS_Codes.FirstOrDefault(p => p.CodeID == a.SecondCode).CodeName;
                    x.Designation = context.AITS_Designations.FirstOrDefault(p => p.ID == a.DesignationID).DesignationName;
                });
                return Json(new { employee = employee, associateList = associateList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}