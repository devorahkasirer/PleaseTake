using AdsWithLogin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdsWithLogin.Web.Models
{
    public class AdDetailsViewModel
    {
        public Ad Ad { get; set; }
        public bool Posted { get; set; }
    }
}