using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using Greendemic.Models;

namespace Greendemic.DAL
{
    public class ShoppingBagItemDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public ShoppingBagItemDAL()
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
        public List<ShoppingBagItem> GetAllShoppingBagItem(int shoppingBagID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM ShoppingBagItem WHERE ShoppingBagID = @selectedShoppingBagID";
            cmd.Parameters.AddWithValue("@selectedShoppingBagID", shoppingBagID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a competition list
            List<ShoppingBagItem> itemList = new List<ShoppingBagItem>();
            while (reader.Read())
            {
                itemList.Add(
                new ShoppingBagItem
                {
                    ShoppingBagID = reader.GetInt32(0), //0: 1st column

                    ItemID = reader.GetInt32(1), //1: 2nd column
                                                 //Get the first character of a string
                    Qty = reader.GetInt32(2), //2: 3rd column

                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return itemList;
        }
        public void Add(ShoppingBagItem item)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated CompetitionID after insertion
            cmd.CommandText = @"INSERT INTO ShoppingBagItem (ShoppingBagID, ItemID, Qty)
VALUES(@shoppingbagID, @itemID, @qty)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@shoppingbagID", item.ShoppingBagID);
            cmd.Parameters.AddWithValue("@itemID", item.ItemID);
            cmd.Parameters.AddWithValue("@qty", item.Qty);

            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //CompetitionID after executing the INSERT SQL statement
            cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
        }
        public ShoppingBagItem GetDetails(int itemID, int shoppingBagID)
        {
            ShoppingBagItem item = new ShoppingBagItem();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement that
            //retrieves all attributes of a admin record.
            cmd.CommandText = @"SELECT * FROM ShoppingBagItem
                                WHERE ShoppingBagID = @selectedShoppingBagID AND ItemID = @selectedItemID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “compId”.
            cmd.Parameters.AddWithValue("@selectedShoppingBagID", shoppingBagID);
            cmd.Parameters.AddWithValue("@selectedItemID",itemID);
            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    item.ShoppingBagID = !reader.IsDBNull(0) ? reader.GetInt32(0) : (int)0;
                    item.ItemID = !reader.IsDBNull(1) ? reader.GetInt32(1) : (int)0;
                    item.Qty = !reader.IsDBNull(2) ? reader.GetInt32(2) : (int)0;
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return item;
        }
    }
}