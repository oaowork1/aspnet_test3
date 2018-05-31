using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Client
{
    public class Categories
    {
        public string id { get; set; }
        public string name { get; set; }

        public bool check = false;

        public static List<Categories> parser(string xmlString)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlString);
            List<Categories> result = new List<Categories>();
            XmlNode xmlNode = xml.ChildNodes[0];
            foreach (XmlNode categoryNode in xmlNode) //sections
            {
                if (categoryNode.Name == "Categories")
                {
                    result.Add(new Categories());
                    foreach (XmlNode itemOne in categoryNode)
                    {
                        if (itemOne.Name == "name")
                        {
                            result[result.Count - 1].name = itemOne.InnerText;
                        }
                        if (itemOne.Name == "id")
                        {
                            result[result.Count - 1].id = itemOne.InnerText;
                        }
                    }
                }
            }
            return result;
        }
    }
}
