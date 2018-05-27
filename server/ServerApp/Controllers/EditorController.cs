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
        public HttpResponseMessage addCategories(string id)
        {
            string temp=id;
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
        public HttpResponseMessage addProduct(string id)             
        {
            //name=one_price=100_count=2_cat=1_desc=
            //one_100_2_1_
            string temp = id;
            if (!DataBase.control(temp))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Looks like injection.")
                };
            }
           
            var parts = temp.Split('_');
            if (parts.Length != 5)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Uncorrect count of filed or used uncorrect symbol _")
                };
            }
            temp = "'" + parts[0] + "'," +
                parts[1] + "," +
                parts[2] + "," +                
                "'" + parts[4] + "',"+
                parts[3] ;           
            
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
        public HttpResponseMessage editCategories(string id)
        {
            //UPDATE Category SET Name = 'ft' WHERE Id = 6;
            //6_ft
            string temp = id;
            if (!DataBase.control(temp))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Looks like injection.")
                };
            }
            var parts = temp.Split('_');
            if (parts.Length != 2)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Uncorrect count of filed or used uncorrect symbol _")
                };
            }
            if (!DataBase.update("Category", "Name", "'"+parts[1]+"'", parts[0]))
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
        public HttpResponseMessage editProduct(string id)
        {
            //6_price_ft
            string temp = id;
            if (!DataBase.control(temp))
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Looks like injection.")
                };
            }
            var parts = temp.Split('_');
            if (parts.Length != 3)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Error. Uncorrect count of filed or used uncorrect symbol _")
                };
            }
            if (!DataBase.update("Product", parts[1], "'" + parts[2] + "'", parts[0]))
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
