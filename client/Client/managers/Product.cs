using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Client
{
    public class Product
    {
        public string id { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string count { get; set; }
        public string categoryId { get; set; }
        public string description { get; set; }

        public bool check;

        public static List<Product> parser(string xmlString)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlString);
            List<Product> result = new List<Product>();
            XmlNode xmlNode = xml.ChildNodes[0];
            foreach (XmlNode productyNode in xmlNode) //sections
            {
                if (productyNode.Name == "Product")
                {
                    result.Add(new Product());
                    foreach (XmlNode itemOne in productyNode)
                    {
                        if (itemOne.Name == "name")
                        {
                            result[result.Count - 1].name = itemOne.InnerText;
                        }
                        if (itemOne.Name == "id")
                        {
                            result[result.Count - 1].id = itemOne.InnerText;
                        }
                        if (itemOne.Name == "price")
                        {
                            result[result.Count - 1].price = itemOne.InnerText;
                        }
                        if (itemOne.Name == "count")
                        {
                            result[result.Count - 1].count = itemOne.InnerText;
                        }
                        if (itemOne.Name == "categoryId")
                        {
                            result[result.Count - 1].categoryId = itemOne.InnerText;
                        }
                        if (itemOne.Name == "description")
                        {
                            result[result.Count - 1].description = itemOne.InnerText;
                        }
                    }
                }
            }
            return result;
        }
    }
}