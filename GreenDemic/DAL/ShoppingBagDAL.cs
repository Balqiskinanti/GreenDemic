using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using GreenDemic.Models;
namespace GreenDemic.DAL
{
    public class ShoppingBagDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public ShoppingBagDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "GreenDemicConnectionString");

            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }


            public List<ShoppingBag> GetAllShoppingBag()
            {
                //Create a SqlCommand object from connection object
                SqlCommand cmd = conn.CreateCommand();
                //Specify the SELECT SQL statement 
                cmd.CommandText = @"SELECT * FROM ShoppingBag ORDER BY ShoppingBagID";
                //Open a database connection
                conn.Open();
                //Execute the SELECT SQL through a DataReader
                SqlDataReader reader = cmd.ExecuteReader();
                //Read all records until the end, save data into a staff list
                List<ShoppingBag> shoppingbagList = new List<ShoppingBag>();
                while (reader.Read())
                {
                    shoppingbagList.Add(
                    new ShoppingBag
                    {
                        ShoppingBagID = reader.GetInt32(0), //0: 1st column
                        CreatedAt = reader.GetString(1), //1: 2nd column
                        BagName = reader.GetString(2), //2: 3rd column
                        BagDescription = reader.GetString(3), //3: 4th column
                        AccID = reader.GetInt32(4), //3: 4th column
                    }
                    );
                }
                //Close DataReader
                reader.Close();
                //Close the database connection
                conn.Close();
                return shoppingbagList;

            }
    }

    
}