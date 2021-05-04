using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
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
        }  //We don't have to creating a new view for same kind of pages just we choose the view and send the model which order by another component

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login()
        {            
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                EvernoteUserManager evernoteUserManager = new EvernoteUserManager();
                BusinessLayerResult<EvernoteUser> businessLayerResult = evernoteUserManager.LoginUser(loginViewModel);
                if (businessLayerResult.Errors.Count > 0)
                {
                    if (businessLayerResult.Errors.Find(x => x.Code == ErrorMessageCode.UserInNotActive)!=null)
                    {
                        //ViewBag.SetLink = "E-Posta Gönder";
                        //ViewBag.SetLink = "http://Home/Activate/1234-4567-78980";
                    }
                        
                    businessLayerResult.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(loginViewModel);
                }

                // Login control and redirecting
                // Add user datas to session and keep data on session
                Session["login"] = businessLayerResult.Result;
                return RedirectToAction("Index");
            }

            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {

            if (ModelState.IsValid)
            {
                EvernoteUserManager evernoteUserManager = new EvernoteUserManager();
                BusinessLayerResult<EvernoteUser> businessLayerResult= evernoteUserManager.RegisterUser(registerViewModel);
                if (businessLayerResult.Errors.Count>0)
                {
                    businessLayerResult.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(registerViewModel);
                }

                //EvernoteUser evernoteUser = null;
                //try
                //{
                //    evernoteUser= evernoteUserManager.RegisterUser(registerViewModel);
                //}
                //catch (Exception ex)
                //{
                //    ModelState.AddModelError("", ex.Message);
                //}


                // Testing register processing (These should not be in the ui layer because tomorrow if we want to create a mobile ui, we have to rewrite it there. So we do it in the BLL layer)

                //if (registerViewModel.Username == "aaa")
                //{
                //    ModelState.AddModelError("", "Kullanıcı adı daha önce alınmış.");
                //}
                //if (registerViewModel.Email == "aaa@aa.com")
                //{
                //    ModelState.AddModelError("", "Bu E-Posta adresiyle daha önce kayıt olunmuş.");
                //}

                //foreach (var item in ModelState)
                //{
                //    if (item.Value.Errors.Count > 0)
                //    {
                //        return View(registerViewModel);
                //    }
                //}

                //if ( evernoteUser ==null)
                //{
                //    return View(registerViewModel);
                //}
                return RedirectToAction("RegisterOk");
            }

            return View(registerViewModel); //If model isn't correct, return model to view and show data anotations
        }

        public ActionResult RegisterOk()
        {
            return View();
        }
        public ActionResult ActivateUser(Guid id)
        {
            EvernoteUserManager evernoteUserManager = new EvernoteUserManager();
            BusinessLayerResult<EvernoteUser> res =  evernoteUserManager.ActivateUser(id);

            if (res.Errors.Count>0)
            {
                TempData["errors"] = res.Errors;
                return RedirectToAction("ActivateUserCancel");
            }
            return RedirectToAction("ActivateUserOk");
        } 
        
        public ActionResult ActivateUserOk()
        {
            // User acvtivation will be done
            return View();
        }
        public ActionResult ActivateUserCancel()
        {
            List<ErrorMessageObj> errors = null;

            if (TempData["errors"]!=null)
            {
               errors = TempData["errors"] as List<ErrorMessageObj>;
            }
            return View(errors);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }


    }
}