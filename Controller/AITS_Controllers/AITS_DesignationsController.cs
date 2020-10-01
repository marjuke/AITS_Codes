using BEEERP.Models.AITS_Models;
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
    public class AITS_DesignationsController : Controller
    {
        BEEContext context = new BEEContext();
        // GET: AITS_Designations
        public ActionResult AITS_Designations()
        {
            ViewBag.Designation = context.AITS_Designations.ToList();
            var id = context.AITS_Designations.ToList().OrderBy(x => x.ID).LastOrDefault();
            if (id != null)
            {
                ViewBag.ID= id.ID + 1;
            }
            else
            {
                ViewBag.ID = 1;
            }


            return View();
        }
        [HttpPost]
        public ActionResult SaveDesignation(AITS_Designations designation)
        {

            if (ModelState.IsValid)
            {
                if (designation.ID == 0 || designation == null)
                {
                    return RedirectToAction("AITS_Designations");
                }
                else
                {
                    var prevZone = context.AITS_Designations.AsNoTracking().FirstOrDefault(m => m.ID == designation.ID);
                    if (prevZone == null)
                    {
                        context.AITS_Designations.Add(designation);
                        context.SaveChanges();
                        return RedirectToAction("AITS_Designations");
                    }
                    else
                    {
                        context.Entry<AITS_Designations>(designation).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        return RedirectToAction("AITS_Designations");
                    }
                }


            }
            else
            {
                ViewBag.Designation = context.AITS_Designations.ToList();
                var id = context.AITS_Designations.ToList().OrderBy(x => x.ID).LastOrDefault();
                if (id != null)
                {
                    ViewBag.ID = id.ID + 1;
                }
                else
                {
                    ViewBag.ID = 1;
                }
                return View("AITS_Designations");
            }
        }
        public ActionResult GetDesignation(int ID)
        {
            if (ID == 0)
            {
                return Json(new { Message = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var designation = context.AITS_Designations.FirstOrDefault(m => m.ID == ID);
                return Json(new { designation = designation }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult DeleteDesignationByid(int id)
        {
            var str = context.AITS_Designations.ToList().Find(x => x.ID == id);

            context.AITS_Designations.Remove(str);
            context.SaveChanges();
            return Json(id, JsonRequestBehavior.AllowGet);
        }
    }
}