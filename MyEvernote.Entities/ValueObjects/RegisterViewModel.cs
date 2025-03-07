﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEvernote.Entities.ValueObjects
{
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı adı"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(25, ErrorMessage = "{0} maks. {1} karakter olmalı")]
        public string Username { get; set; }

        [DisplayName("E-posta"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(70, ErrorMessage = "{0} maks. {1} karakter olmalı"),
            EmailAddress(ErrorMessage = "Lütfen {0} alanı için geçerli bir e-posta adresi giriniz.")]

        public string Email { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş geçilemez."),
            DataType(DataType.Password),
            StringLength(25, ErrorMessage = "{0} maks. {1} karakter olmalı")]

        public string Password { get; set; }
        [DisplayName("Şifre tekrar"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            DataType(DataType.Password), StringLength(25, ErrorMessage = "{0} maks. {1} karakter olmalı"),
            Compare("Password", ErrorMessage = "{0} ile {1} uyuşmuyor.")] //Compare for checking password and repassword match
        public string RePassword { get; set; }
    }
}