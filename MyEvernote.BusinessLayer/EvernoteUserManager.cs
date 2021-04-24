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
                    IsActive = false,
                    IsAdmin = false,
                    ModifiedUsername = "system"
                });

                if (dbResult > 1)
                {
                    layerResult.Result = repo_user.Find(x => x.Email == registerViewModel.Email && x.Username == registerViewModel.Username);

                    // TODO: Actiavation mail will be send
                    //layerResult.Result.ActivateGuid
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
    }
}
