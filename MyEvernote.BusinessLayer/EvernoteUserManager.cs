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
    public class EvernoteUserManager
    {
        private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterViewModel registerViewModel)
        {
            // Username taken before?
            // Email taken before?
            // Register processing
            // Activation e-mail sending
            EvernoteUser user = repo_user.Find(x => x.Username == registerViewModel.Username || x.Email == registerViewModel.Email);
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
                int dbResult = repo_user.Insert(new EvernoteUser()
                {
                    Username = registerViewModel.Username,
                    Email = registerViewModel.Email,
                    Password = registerViewModel.Password,
                    ActivateGuid= Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,
                });

                if (dbResult > 0)
                {
                    layerResult.Result = repo_user.Find(x => x.Email == registerViewModel.Email && x.Username == registerViewModel.Username);

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
            layerResult.Result = repo_user.Find(x => x.Username == loginViewModel.Username && x.Password == loginViewModel.Password);

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

        public BusinessLayerResult<EvernoteUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = repo_user.Find(x => x.ActivateGuid == activateId);

            if (res.Result!=null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }

                res.Result.IsActive = true;
                repo_user.Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Aktifleştirilecek kullanıcı bulunamadı.");
            }

            return res;

        }
    }
}
