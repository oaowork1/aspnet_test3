using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ServerApp.Models
{
    internal class DataBase
    {
        public static DataSet get(string query)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~");
                String connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + "bd1.accdb; Persist Security Info=False;";
                OleDbConnection con = new OleDbConnection(connect);
                con.Open();

                DataSet ds = new DataSet();
                OleDbDataAdapter dataAdapter;

                dataAdapter = new OleDbDataAdapter(query, con);
                dataAdapter.Fill(ds);

                con.Close();
                return ds;
            }
            catch (Exception e) { }
            return null;
        }

        public static bool add(string table, string parameters, string temp)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~");
                String connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + "bd1.accdb; Persist Security Info=False;";
                OleDbConnection sqlConnection1 = new OleDbConnection(connect);

                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                //cmd.CommandText = "INSERT INTO Product ( Name, Price, [Count], Description, CategoryId ) SELECT 'NewOne2', 100, 5, 'ho-ho-ho', 1;";
                //                   INSERT INTO Product (Name, Price, Count, Description, CategoryId)SELECT 'one',100,2,'hi',1;
                cmd.CommandText = "INSERT INTO " + table + " (" + parameters + ")SELECT " + temp + ";";
                cmd.Connection = sqlConnection1;

                sqlConnection1.Open();
                cmd.ExecuteNonQuery();
                sqlConnection1.Close();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public static bool control(string temp)
        {
            if (temp.ToLower().Contains("from") || temp.ToLower().Contains("insert") || temp.ToLower().Contains("select"))
            {
                return false;
            }
            return true;
        }

        public static bool del(string table, string parameter)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~");
                String connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + "bd1.accdb; Persist Security Info=False;";
                OleDbConnection sqlConnection1 = new OleDbConnection(connect);

                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "DELETE * FROM " + table + " WHERE Id=" + parameter + ";";
                cmd.Connection = sqlConnection1;

                sqlConnection1.Open();
                cmd.ExecuteNonQuery();
                sqlConnection1.Close();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public static bool update(string table, string parameter, string equal1, string equal2)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~");
                String connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + "bd1.accdb; Persist Security Info=False;";
                OleDbConnection sqlConnection1 = new OleDbConnection(connect);

                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                //UPDATE Category SET Name = 'ft' WHERE Id=6;
                cmd.CommandText = "UPDATE " + table +
                    " SET " + parameter + " = " + equal1 +
                    " WHERE Id=" + equal2 + ";";
                cmd.Connection = sqlConnection1;

                sqlConnection1.Open();
                cmd.ExecuteNonQuery();
                sqlConnection1.Close();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}