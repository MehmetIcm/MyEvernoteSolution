using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category 

        //Listing Category With Temdata (Carrying Model from Controller to Another Controller Action)

        //public ActionResult Select(int? id) // "?" question mark is for it can be null
        //{
        //    if (id==null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CategoryManager categoryManager = new CategoryManager();
        //    Category category=categoryManager.GetCategoryById(id.Value);
        //    if (category==null)
        //    {
        //        return HttpNotFound();
        //        //return RedirectToAction("Index", "Home");
        //    }
        //    TempData["categoryNotes"] = category.Notes; //It's for carrying data to one action to another action
        //    return RedirectToAction("Index", "Home");
        //    // return View("Index", category.Notes);
        //}
    }
}