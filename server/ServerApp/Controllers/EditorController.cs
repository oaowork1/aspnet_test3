using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServerApp.Models;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http.Description;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Xml;
using System.Web;

namespace ServerApp.Controllers
{
    public class EditorController : ApiController
    {
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> getCategories()
        {
            DataSet dataSet = DataBase.get("SELECT * FROM Category;");
            List<Categories> categories = new List<Categories>();

            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                try
                {
                    categories.Add(new Categories
                    {
                        id = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[0]),
                        name = Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[1])
                    });
                }
                catch (Exception e)
                {
                    return NotFound();
                }
            }
            if (categories == null || categories.Count == 0)
            {
                return NotFound();
            }

            return Ok(categories);
        }

        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> getProducts()
        {
            DataSet dataSet = DataBase.get("SELECT * FROM Product;");
            List<Product> products = new List<Product>();

            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                try
                {
                    products.Add(new Product
                    {
                        id = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[0]),
                        name = Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[1]),
                        price = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[2]),
                        count = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[3]),
                        description = Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[4]),
                        categoryId = Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[5])
                    });
                }
                catch (Exception e)
                {
                    return NotFound();
                }
            }
            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage addCategories(string name)
        {
            string temp = name;
            if (temp.Length>100)
            {
                temp = temp.Substring(0, 100);
            }
            if (!DataBase.control(temp))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Looks like injection.")
                };
            }
            temp = "'" + temp + "'";
            if (!DataBase.add("Category", "Name", temp))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. DataBase problem.")
                };
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent("ok")
            };
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage addProduct(string name, string price, string count, string categoryid, string description="")             
        {
            //check on number;
            try{
                int test = Convert.ToInt32(price);
                test = Convert.ToInt32(count);
                test = Convert.ToInt32(categoryid);
            } catch (Exception e)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Non-integer parameters.")
                };
            }
            
            string description1000 = description;
            if (description1000 == null)
            {
                description1000 = "";
            }
            if (description1000.Length > 100)
            {
                description1000 = description1000.Substring(0, 100);
            }

            if (!DataBase.control(name) || !DataBase.control(price) ||!DataBase.control(count) ||
                !DataBase.control(categoryid) || !DataBase.control(description1000))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Looks like injection.")
                };
            }

            string temp = "'" + name + "'," +
                price + "," +
                count + "," +
                "'" + description1000 + "'," +
                categoryid;           
            
            //INSERT INTO Product ( Name, Price, Count, Description, CategoryId ) SELECT 'NewOne', 100, 5, 'ho-ho-ho', 1;
            if (!DataBase.add("Product", "Name, Price, [Count], Description, CategoryId", temp))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. DataBase problem.")
                };
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent("ok")
            };
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage delCategories(string id)
        {
            string temp = id;
            try
            {
                int test = Convert.ToInt32(id);
            }
            catch (Exception e)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Non-integer id-parameters.")
                };
            }
            if (!DataBase.control(temp))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Looks like injection.")
                };
            }
            if (!DataBase.del("Category", temp))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. DataBase problem.")
                };
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent("ok")
            };
        }
        [System.Web.Http.HttpGet]
        public HttpResponseMessage delProduct(string id)
        {
            string temp = id;
            try
            {
                int test = Convert.ToInt32(id);
            }
            catch (Exception e)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Non-integer id-parameters.")
                };
            }
            if (!DataBase.control(temp))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Looks like injection.")
                };
            }
            if (!DataBase.del("Product", temp))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. DataBase problem.")
                };
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent("ok")
            };
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage editCategories(string id, string name)
        {
            //UPDATE Category SET Name = 'ft' WHERE Id = 6;            
            try
            {
                int test = Convert.ToInt32(id);
            }
            catch (Exception e)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Non-integer id-parameters.")
                };
            }

            string temp = name;
            if (!DataBase.control(temp))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Looks like injection.")
                };
            }

            if (!DataBase.update("Category", "Name", "'" + name + "'", id))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. DataBase problem.")
                };
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent("ok")
            };
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage editProduct(string id, string field, string value)
        {
            //6_price_ft
            if (field.Equals("price") || field.Equals("count") || field.Equals("categoryid"))
            try
            {
                int test = Convert.ToInt32(id);
                test = Convert.ToInt32(value);
            }
            catch (Exception e)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Non-integer parameters.")
                };
            }

            string temp = field;
            if (temp.Equals("categoryid"))
            {
                if (temp.Length > 100)
                {
                    temp = temp.Substring(0, 100);
                }
            }
            if (field.Equals("id"))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Id could not be changed.")
                };
            }
            if (!DataBase.control(temp) && !DataBase.control(value))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Looks like injection.")
                };
            }
            if (!DataBase.update("Product", temp, "'" + value + "'", id))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. DataBase problem.")
                };
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent("ok")
            };
        }
    }
}
