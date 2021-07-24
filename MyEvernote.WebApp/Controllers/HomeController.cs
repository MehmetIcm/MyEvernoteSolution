using MyEvernote.BusinessLayer;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using MyEvernote.WebApp.Models;
using MyEvernote.WebApp.ViewModels;
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
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private EvernoteUserManager evernoteUserManager = new EvernoteUserManager();

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

            return View(noteManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());      //Sorting on csharp
            //return View(noteManager.GetAllNotesQueryable().OrderByDescending(x=>x.ModifiedOn).ToList());    
            //Sorting on Sql Serverside (it's sending sorting queryable to sql)
        }
        public ActionResult ByCategory(int? id) // "?" question mark is for it can be null
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x=>x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index", "Home");
            }
            return View("Index", category.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }
        public ActionResult MostLiked()
        {
            return View("Index", noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
            //With "Index" statement we say to code, "Go to Index and carry to this model to view"
        }  //We don't have to creating a new view for same kind of pages just we choose the view and send the model which order by another component
        public ActionResult About()
        {
            return View();
        }
        public ActionResult ShowProfile()
        {

            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                TempData["errors"] = res.Errors;
                return View("Error", errorNotifyObj);
            }

            return View(res.Result);

        }
        public ActionResult EditProfile()
        {

            EvernoteUserManager evernoteUserManager = new EvernoteUserManager();
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                TempData["errors"] = res.Errors;
                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditProfile(EvernoteUser user, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");  //Don't check ModifiedUsername because we're sending data default

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                (ProfileImage.ContentType == "image/jpeg" ||
                ProfileImage.ContentType == "image/jpg" ||
                ProfileImage.ContentType == "image/png")
                )
                {
                    string filename = $"user_{user.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    user.ProfileImageFileName = filename;
                }

                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.UpdateProfile(user);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }

                CurrentSession.Set<EvernoteUser>("login", res.Result); //The session has been updated as the profile has been updated
                return RedirectToAction("ShowProfile");
            }

            return View(user);

        }
        public ActionResult RemoveProfile()
        {

            BusinessLayerResult<EvernoteUser> businessLayerResult = evernoteUserManager.RemoveUserById(CurrentSession.User.Id);
            if (businessLayerResult.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = businessLayerResult.Errors,
                    Title = "Profil Silinemedi",
                    RedirectingUrl = "/Home/ShowProfile"
                };
                return View("Error", errorNotifyObj);
            }
            Session.Clear();

            return RedirectToAction("Index");
        }

        //public ActionResult TestNotify()
        //{
        //    ErrorViewModel model = new ErrorViewModel()
        //    {
        //        Header = "Yönlendirme...",
        //        Title = "Ok Test",
        //        RedirectingTimeOut = 3000,
        //        Items=new List<ErrorMessageObj>() { 
        //            new ErrorMessageObj() { Message="Test Başarılı 1"},
        //            new ErrorMessageObj() { Message="Test Başarılı 2"}, 
        //        }
        //    };
        //    return View("Error", model);
        //}
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<EvernoteUser> businessLayerResult = evernoteUserManager.LoginUser(loginViewModel);
                if (businessLayerResult.Errors.Count > 0)
                {
                    if (businessLayerResult.Errors.Find(x => x.Code == ErrorMessageCode.UserInNotActive) != null)
                    {
                        //ViewBag.SetLink = "E-Posta Gönder";
                        //ViewBag.SetLink = "http://Home/Activate/1234-4567-78980";
                    }

                    businessLayerResult.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(loginViewModel);
                }

                // Login control and redirecting
                // Add user datas to session and keep data on session
                CurrentSession.Set<EvernoteUser>("login", businessLayerResult.Result);
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
                BusinessLayerResult<EvernoteUser> businessLayerResult = evernoteUserManager.RegisterUser(registerViewModel);
                if (businessLayerResult.Errors.Count > 0)
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
                OkViewModel notfiyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"
                };
                notfiyObj.Items.Add("Lütfen e-posta adresinize gönderilen aktivasyon linkine tıklayarak hesabınızı aktive ediniz.                       Hesabınızı aktive etmeden not ekleyemez ve beğenme yapamazsınız.");
                return View("Ok", notfiyObj);
            }

            return View(registerViewModel); //If model isn't correct, return model to view and show data anotations
        }
        public ActionResult ActivateUser(Guid id)
        {
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
                TempData["errors"] = res.Errors;
                return View("Error", errorNotifyObj);
            }

            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştirilme Başarılı",
                RedirectingUrl = "/Home/Login"
            };
            okNotifyObj.Items.Add("Hesap Aktifleştirildi. Artık not paylaşabilir ve beğenme yapabilirsiniz.");
            return View("Ok", okNotifyObj);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}