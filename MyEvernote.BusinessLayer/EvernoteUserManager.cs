using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Common.Helpers;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyEvernote.BusinessLayer
{
    public class EvernoteUserManager:ManagerBase<EvernoteUser>
    {
        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterViewModel registerViewModel)
        {
            // Username taken before?
            // Email taken before?
            // Register processing
            // Activation e-mail sending
            EvernoteUser user = Find(x => x.Username == registerViewModel.Username || x.Email == registerViewModel.Email);
            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();

            if (user != null)
            {
                if (user.Username == registerViewModel.Username)
                {
                    layerResult.AddError(ErrorMessageCode.UserAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if (user.Email == registerViewModel.Email)
                {
                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "Email adresi ile daha önce kayıt olunmuş.");
                }
            }
            else
            {
                int dbResult = base.Insert(new EvernoteUser()
                {
                    Username = registerViewModel.Username,
                    Email = registerViewModel.Email,
                    Password = registerViewModel.Password,
                    ProfileImageFileName= "profile_picture.png",
                    ActivateGuid= Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,
                });

                if (dbResult > 0)
                {
                    layerResult.Result = Find(x => x.Email == registerViewModel.Email && x.Username == registerViewModel.Username);

                    // TODO: Actiavation mail will be send
                    //layerResult.Result.ActivateGuid

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/ActivateUser/{layerResult.Result.ActivateGuid}";
                    string body = $"Merhaba {layerResult.Result.Username};<br><br> Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank' >tıklanıyız</a>.";
                    MailHelper.SendMail(body, layerResult.Result.Email,"My Evernote Hesap Aktifleştirme");
                }
            }

            return layerResult;
        }

        public BusinessLayerResult<EvernoteUser> LoginUser(LoginViewModel loginViewModel)
        {
            // Checking login
            // Is Account Active

            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();
            layerResult.Result = Find(x => x.Username == loginViewModel.Username && x.Password == loginViewModel.Password);

            if (layerResult.Result != null)
            {
                if (!layerResult.Result.IsActive)
                {
                    layerResult.AddError(ErrorMessageCode.UserInNotActive,"Kullanıcı aktifleştirilmemiş.");
                    layerResult.AddError(ErrorMessageCode.CheckYourEmail,"Lütfen e-posta adresinizi kontrol ediniz.");
                }
            }
            else
            {
                layerResult.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı ya da şifre uyuşmuyor.");
            }

            return layerResult;
        }

        public BusinessLayerResult<EvernoteUser> GetUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.Id == id);
            if (res.Result==null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Bulunamadı.");
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.ActivateGuid == activateId);

            if (res.Result!=null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }

                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Aktifleştirilecek kullanıcı bulunamadı.");
            }

            return res;

        }

        public BusinessLayerResult<EvernoteUser> UpdateProfile(EvernoteUser data)
        {
            EvernoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();

            if (user != null && user.Id != data.Id)
            {
                if (user.Username == data.Username)
                {
                    layerResult.AddError(ErrorMessageCode.UserAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if (user.Email == data.Email)
                {
                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "Email adresi ile daha önce kayıt olunmuş.");
                }

                return layerResult;
            }
            else
            {
                layerResult.Result = Find(x => x.Id == data.Id);
                layerResult.Result.Email = data.Email;
                layerResult.Result.Name = data.Name;
                layerResult.Result.Surname = data.Surname;
                layerResult.Result.Password = data.Password;
                layerResult.Result.Username = data.Username;

                if (string.IsNullOrEmpty(data.ProfileImageFileName)==false)
                {
                    layerResult.Result.ProfileImageFileName = data.ProfileImageFileName;
                }
                if (base.Update(layerResult.Result)==0)
                {
                    layerResult.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profiliniz güncellenemedi.");
                }
            return layerResult;
            }

        }

        public BusinessLayerResult<EvernoteUser> RemoveUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> businessLayerResult = new BusinessLayerResult<EvernoteUser>();
            EvernoteUser user = Find(x => x.Id == id);
            if (user!=null)
            {
                if (Delete(user)==0)
                {
                    businessLayerResult.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi");
                    return businessLayerResult;
                }
            }
            else
            {
                businessLayerResult.AddError(ErrorMessageCode.UserCouldNotFound, "Kullanıcı bulunamadı");
            }
            return businessLayerResult;
        }

        //Method Hiding..
        public new BusinessLayerResult<EvernoteUser> Insert(EvernoteUser data)   //new : method hiding. it's using for like override but aslo we can change return type with new
        {           
            EvernoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();

            layerResult.Result = data;

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    layerResult.AddError(ErrorMessageCode.UserAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if (user.Email == data.Email)
                {
                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "Email adresi ile daha önce kayıt olunmuş.");
                }
            }
            else
            {
                layerResult.Result.ProfileImageFileName = "profile_picture.png";
                layerResult.Result.ActivateGuid = Guid.NewGuid();


                if (base.Insert(layerResult.Result) == 0) 
                {
                    layerResult.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı eklenemedi.");
                }           
            }

            return layerResult;

        }

        public new BusinessLayerResult<EvernoteUser> Update(EvernoteUser data)
        {
            EvernoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();
            layerResult.Result = data;

            if (user != null && user.Id != data.Id)
            {
                if (user.Username == data.Username)
                {
                    layerResult.AddError(ErrorMessageCode.UserAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if (user.Email == data.Email)
                {
                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "Email adresi ile daha önce kayıt olunmuş.");
                }

                return layerResult;
            }
            else
            {
                layerResult.Result = Find(x => x.Id == data.Id);
                layerResult.Result.Email = data.Email;
                layerResult.Result.Name = data.Name;
                layerResult.Result.Surname = data.Surname;
                layerResult.Result.Password = data.Password;
                layerResult.Result.Username = data.Username;
                layerResult.Result.IsActive = data.IsActive;
                layerResult.Result.IsAdmin = data.IsAdmin;

                if (base.Update(layerResult.Result) == 0)
                {
                    layerResult.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
                }
                return layerResult;
            }
        }
    }
}
