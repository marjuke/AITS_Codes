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
    public class AITS_CommissionController : Controller
    {
        BEEContext context = new BEEContext();
        // GET: AITS_Commission
        public ActionResult AITS_Commission()
        {
            ViewBag.Employee= LoadComboBox.LoadAllAITS_Employees();
            return View();
        }
        public ActionResult GenerateCommission(AITS_Commission model)
        {
            List<AITS_Commission> aITS = new List<AITS_Commission>();
            decimal employeeAmount=0;
            decimal RemployeeAmount=0;
            if (model.EmployeeID == 0)
            {
                return Json(new { Message = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var employee = context.AITS_SaleEntry.Where(m => m.EmployeeID == model.EmployeeID&&m.SaleDate>= model.From&&m.SaleDate<=model.To).ToList();
                employee.ForEach(x =>
                {
                    employeeAmount = employeeAmount + x.Amount;
                });
                bool isTrue= SalesComissionISTrue(employeeAmount);
                if (isTrue==true)
                {
                    var referenceEmployee = (from p in context.AITS_SaleEntry
                                             join c in context.AITS_Employee on p.EmployeeID equals c.EmpID
                                             where c.ReferenceEmpID == model.EmployeeID && p.SaleDate >= model.From && p.SaleDate <= model.To
                                             select new { RempID = p.EmployeeID, Amount = p.Amount }).ToList();
                    referenceEmployee.ForEach(x =>
                    {
                        RemployeeAmount = x.Amount;
                    });
                    decimal totalAmount = employeeAmount + RemployeeAmount;
                    decimal commissionAmountForSales = CalculationCommissionForSale(totalAmount);
                    AITS_Commission aITSs = new AITS_Commission();
                    aITSs.Code = context.AITS_Codes.FirstOrDefault(x => x.CodeID == 1).CodeName;
                    //aITS.Designation= (from p in context.AITS_EmployeeDesignation
                    //                   join c in context.AITS_Designations on p.DesignationID equals c.ID
                    //                   where p.EmpID == model.EmployeeID
                    //                   select new { p.DesignationName }).ToString();
                    var desgid = context.AITS_EmployeeDesignation.FirstOrDefault(x => x.EmpID == model.EmployeeID).DesignationID;
                    aITSs.Designation = context.AITS_Designations.FirstOrDefault(x => x.ID == desgid).DesignationName;
                    aITSs.Name = context.AITS_Employee.FirstOrDefault(x => x.EmpID == model.EmployeeID).EmployeeName;
                    aITSs.SalesCommission = commissionAmountForSales;
                    aITS.Add(aITSs);
                    return Json(new { aITS = aITS }, JsonRequestBehavior.AllowGet);
                }
                
                return Json(new { aITS = employee }, JsonRequestBehavior.AllowGet);
            }

        }

        public decimal CalculationCommissionForSale(decimal totalAmount)
        {
            if (totalAmount<10000)
            {
                return (totalAmount * 6) / 100;
            }
            else if (totalAmount>=10000 && totalAmount <30000)
            {
                return (totalAmount *(decimal)7.5) / 100;
            }
            else if (totalAmount>=30000 && totalAmount <60000)
            {
                return (totalAmount *(decimal)9.5) / 100;
            }
            else
            {
                return (totalAmount * (decimal)11) / 100;
            }
        }

        public bool SalesComissionISTrue(decimal employeeAmount)
        {
            if (employeeAmount>4000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}