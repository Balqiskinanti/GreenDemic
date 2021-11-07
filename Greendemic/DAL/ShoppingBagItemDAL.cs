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
        public List<ShoppingBagItem> GetAllItem()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM ShoppingBagItem ORDER BY ShoppingBagID";
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

                    ItemID = reader.GetInt32(2), //1: 2nd column
                                                    //Get the first character of a string
                    Qty = reader.GetInt32(3), //2: 3rd column

                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return itemlist;
        }
        public int Add(ShoppingBagItem item)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated CompetitionID after insertion
            cmd.CommandText = @"INSERT INTO ShoppingBagItem (ShoppingBagID, ItemID, Qty)
OUTPUT INSERTED.ShoppingBagID
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
            item.ShoppingBagID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return item.ShoppingBagID;
        }
        public List<ShoppingBagItem> GetDetails(int itemID)
        {
            ShoppingBagItem item = new ShoppingBagItem();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement that
            //retrieves all attributes of a admin record.
            cmd.CommandText = @"SELECT * FROM ShoppingBagItem
 WHERE ShoppingBagID = @selectedItemID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “compId”.
            cmd.Parameters.AddWithValue("@selectedItemID", item.ShoppingBagID);
            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            List<ShoppingBagItem> itemList = new List<ShoppingBagItem>();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    itemList.Add(
                        new ShoppingBagItem
                        {
                            ShoppingBagID = reader.GetInt32(0), //0: 1st column

                            ItemID = reader.GetInt32(2), //1: 2nd column
                                                         //Get the first character of a string
                            Qty = reader.GetInt32(3), //2: 3rd column
                        });
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return itemList;
        }
        public List<ShoppingBagItem> Update(ShoppingBagItem item)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE ShoppingBagItem SET ItemID=@itemID,
 Qty=@qty, WHERE ShoppingBagID = @selectedItemID";
            List<ShoppingBagItem> itemList = new List<ShoppingBagItem>();
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@itemID", item.ItemID);
            cmd.Parameters.AddWithValue("@qty", item.Qty);

            cmd.Parameters.AddWithValue("@selectedItemID", item.ShoppingBagID);
            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return itemList;
        }
        public List<ShoppingBagItem> Delete(ShoppingBagItem itemID)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement
            //to delete a competition record specified by a Competition ID
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM ShoppingBagItem
 WHERE ShoppingBagID = @selectedItemID";
            List<ShoppingBagItem> itemList = new List<ShoppingBagItem>();
            cmd.Parameters.AddWithValue("@selectedItemID", itemID.ShoppingBagID);
            //Open a database connection
            conn.Open();
            int rowAffected = 0;
            //Execute the DELETE SQL to remove the competition record
            rowAffected += cmd.ExecuteNonQuery();
            //Close database connection
            conn.Close();
            //Return number of row of competition record updated or deleted
            return itemList;
        }
    }
}
