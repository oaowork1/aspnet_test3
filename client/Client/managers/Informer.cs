using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Client
{
    public class Informer
    {
        private Informer()
        {

        }
        private static Informer instance;
        public static Informer getInstance()
        {
            if (instance == null)
            {
                instance = new Informer();
            }
            return instance;
        }

        public int xx=10;
        public int yy=10;

        public bool GET(string Url, string Data="")
        {
            string url = Url;
            if (Data!="")
            {
                url += "?" + Data;
            }
            xmlString = "";
           
            try
            {

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "application/xml";
                request.Accept = "application/xml";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    xmlString = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                error = "Error with connection. " + e;
                return false;
            }
            catch
            {
                return false;
            }
            if (xmlString == null || xmlString.Length == 0)
            {
                error = "Error with api request";
                return false;
            }
            if (xmlString.Length>=5 && xmlString.Substring(0, 5).Contains("Error"))
            {
                error = xmlString;
                return false;
            }                
           
            return true;
        }

        public string error = "";
        public string xmlString = "";
        public List<Categories>  categoryList= new List<Categories>();
        public List<Product> productList = new List<Product>(); 
    }
}
