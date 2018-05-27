using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerApp.Models
{
    public class Product
    {
        public int id;
        public string name;
        public int price;
        public int count;
        public string description;
        public int categoryId;
    }
}