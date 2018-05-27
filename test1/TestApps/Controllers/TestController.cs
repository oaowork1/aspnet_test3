using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;
using TestApps.Models;

namespace TestApps.Controllers
{
    public class TestController : Controller
    {
        private const string url = "http://www.mrsoft.by/data.json";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form, string override1)
        {
            //check of work environment
            bool check = false;
            int action = 0;
            int num = -1;
            string substring = "";
            if(form.AllKeys.Length==3)
            {
                check = true;
            }
            if (form.AllKeys[form.AllKeys.Length - 1] == "btn1")
            {
                action = 1;
                try
                {
                    num = Convert.ToInt32(override1);
                }
                catch (Exception e)
                {

                }
                if (num == 0)
                {
                    return View();
                }
            } else
            {
                action = 2;
                try
                {
                    substring = override1;
                } catch(Exception e)
                {

                }               
            }    
            
            //load start list
            var list = JsonConvert.DeserializeObject<Data>(new WebClient().DownloadString(url), new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            //make final list
            var strings = new List<string>();
            if (action==1)
            {
                for (int i=0; i<list.data.Count; i++)
                {
                    if (list.data[i].Length>num)
                    {
                        if (check)
                        {
                            string temp = list.data[i].ToLower();
                            if (!list.data[i].Equals(temp))
                            {
                                continue;
                            }
                        }
                        strings.Add(list.data[i]);
                    }
                }
            }
            if (action == 2)
            {
                for (int i = 0; i < list.data.Count; i++)
                {
                    if (
                        (!check && list.data[i].ToLower().Contains(substring.ToLower())) ||
                        (check && list.data[i].Contains(substring))
                       )
                    {
                        strings.Add(list.data[i]);
                    }
                }
            }
            return View(strings);
        }
        
	}
}