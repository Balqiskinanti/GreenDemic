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
    public class ItemDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public ItemDAL()
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
        public List<Item> GetAllItem()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Item ORDER BY ItemID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a competition list
            List<Item> itemList = new List<Item>();
            while (reader.Read())
            {
                itemList.Add(
                new Item
                {
                    ItemID = reader.GetInt32(0), //0: 1st column

                    ItemName = reader.GetString(2), //1: 2nd column
                                                    //Get the first character of a string
                    Category = reader.GetString(3), //2: 3rd column
                    Cal = reader.GetInt32(4), //3: 4th column

                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return itemList;
        }
        public int Add(Item item)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated CompetitionID after insertion
            cmd.CommandText = @"INSERT INTO Item (ItemName, Category, Cal)
OUTPUT INSERTED.ItemID
VALUES(@itemName, @category, @cal)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@itemName", item.ItemName);
            cmd.Parameters.AddWithValue("@category", item.Category);
            cmd.Parameters.AddWithValue("@cal", item.Cal);

            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //CompetitionID after executing the INSERT SQL statement
            item.ItemID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return item.ItemID;
        }
        public Item GetDetails(int itemID)
        {
            Item item = new Item();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement that
            //retrieves all attributes of a admin record.
            cmd.CommandText = @"SELECT * FROM Item
                                WHERE ItemID = @selectedItemID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “compId”.
            cmd.Parameters.AddWithValue("@selectedItemID", itemID);
            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    item.ItemID = !reader.IsDBNull(0) ? reader.GetInt32(0) : (int)0;
                    item.ItemName = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    item.Category = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    item.Cal = !reader.IsDBNull(3) ? reader.GetInt32(3) : (int)0;
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return item;
        }
        public int Update(Item item, ShoppingBagItem shoppingBagItem)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            SqlCommand cmd2 = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Item 
                                SET ItemName=@itemName, Category=@category, Cal = @cal 
                                WHERE ItemID = @selectedItemID";
            cmd2.CommandText = @"UPDATE ShoppingBagItem 
                                SET Qty=@qty 
                                WHERE ShoppingBagID = @selectedShoppingBagID AND ItemID = @selectedItemID";
            cmd.Parameters.AddWithValue("@itemName", item.ItemName);
            cmd.Parameters.AddWithValue("@cal", item.Cal);
            cmd.Parameters.AddWithValue("@category", item.Category);
            cmd.Parameters.AddWithValue("@selectedItemID", item.ItemID);

            cmd2.Parameters.AddWithValue("@selectedItemID", shoppingBagItem.ItemID);
            cmd2.Parameters.AddWithValue("@selectedShoppingBagID", shoppingBagItem.ShoppingBagID);
            cmd2.Parameters.AddWithValue("@qty", shoppingBagItem.Qty);
            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int rowAffected = 0;
            rowAffected += cmd2.ExecuteNonQuery();
            rowAffected += cmd.ExecuteNonQuery();
            
            //Close the database connection
            conn.Close();
            return rowAffected;
        }
        public int Delete(int itemID, int shoppingBagID)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement
            //to delete a competition record specified by a Competition ID
            //SqlCommand cmd = conn.CreateCommand();
            SqlCommand cmd1 = conn.CreateCommand();
            //cmd.CommandText = @"DELETE FROM Item
            //                    WHERE ItemID = @selectedItemID";
            cmd1.CommandText = @"DELETE FROM ShoppingBagItem
                                WHERE ItemID = @selectedItemID AND ShoppingBagID = @selectedSBID";
            //cmd.Parameters.AddWithValue("@selectedItemID", itemID);
            cmd1.Parameters.AddWithValue("@selectedItemID", itemID);
            cmd1.Parameters.AddWithValue("@selectedSBID", shoppingBagID);
            //Open a database connection
            conn.Open();
            int rowAffected = 0;
            //Execute the DELETE SQL to remove the competition record
            rowAffected += cmd1.ExecuteNonQuery();
            //rowAffected += cmd.ExecuteNonQuery();
            
            //Close database connection
            conn.Close();
            //Return number of row of competition record updated or deleted
            return rowAffected;
        }
    }
}