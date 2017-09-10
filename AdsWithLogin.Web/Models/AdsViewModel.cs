using AdsWithLogin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdsWithLogin.Web.Models
{
    public class AdsViewModel
    {
        public IEnumerable<Ad> Ads { get; set; }
        public User User { get; set; }
    }
}