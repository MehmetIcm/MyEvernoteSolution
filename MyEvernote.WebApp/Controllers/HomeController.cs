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
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //Test test = new Test();
            //test.InsertTest();
            //test.UpdateTest();
            //test.DeteleTest();
            //test.CommentTest();

            // 
            //if (TempData["categoryNotes"] != null)
            //{
            //    return View(TempData["categoryNotes"] as List<Note>);
            //}

            NoteManager noteManager = new NoteManager();
            return View(noteManager.GetAllNotes().OrderByDescending(x => x.ModifiedOn).ToList());      //Sorting on csharp
            //return View(noteManager.GetAllNotesQueryable().OrderByDescending(x=>x.ModifiedOn).ToList());    
            //Sorting on Sql Serverside (it's sending sorting queryable to sql)
        }

        public ActionResult ByCategory(int? id) // "?" question mark is for it can be null
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryManager categoryManager = new CategoryManager();
            Category category = categoryManager.GetCategoryById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index", "Home");
            }
            return View("Index", category.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            NoteManager noteManager = new NoteManager();
            return View("Index", noteManager.GetAllNotes().OrderByDescending(x => x.LikeCount).ToList());
            //With "Index" statement we say to code, "Go to Index and carry to this model to view"
        }

        public ActionResult About()
        {
            return View();
        }
    }
}