﻿using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.WebApp.Models
{
    //Session Helper
    public class CurrentSession
    {
        public static EvernoteUser User
        {
            get 
            {
                if (HttpContext.Current.Session["login"]!=null)
                {
                    return HttpContext.Current.Session["login"] as EvernoteUser;
                }

                return null;
            }

        }

        public static void Set<T>(string key, T obj)
        {
            HttpContext.Current.Session[key]= obj;
        }

        public static T Get<T>(string key)
        {
            if (HttpContext.Current.Session[key]!=null)
            {
                return (T) HttpContext.Current.Session[key];
            }
            return default(T);
        }

        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}