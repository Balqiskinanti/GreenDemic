using GreenDemic.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDemic.DAL
{
    public class ShoppingBagDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;

        //Constructor
        public ShoppingBagDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "GreenDemicConnectionString");
            conn = new SqlConnection(strConn);
        }

        // Retrieve list of all shopping bags
        public List<ShoppingBag> GetAllShoppingBags(int accID)
        {
            List<ShoppingBag> shoppingBagList = new List<ShoppingBag>();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM ShoppingBag WHERE AccID = @selectedAccID ORDER BY ShoppingBagID ASC";
            cmd.Parameters.AddWithValue("@selectedAccID", accID);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                shoppingBagList.Add(
                new ShoppingBag
                {
                    ShoppingBagID = reader.GetInt32(0),
                    CreatedAt = reader.GetDateTime(1),
                    BagName = reader.GetString(2),
                    BagDescription = !reader.IsDBNull(3) ? reader.GetString(3) : null,
                    AccID = reader.GetInt32(4)
                }
                );
            }
            reader.Close();
            conn.Close();
            return shoppingBagList;
        }

        // Add shopping bag
        public int Add(ShoppingBag bag)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO ShoppingBag (CreatedAt, BagName, BagDescription, AccID)
                                OUTPUT INSERTED.ShoppingBagID
                                VALUES(@createdAt, @bagName, @bagDescription, @accID)";
            cmd.Parameters.AddWithValue("@createdAt", bag.CreatedAt);
            cmd.Parameters.AddWithValue("@bagName", bag.BagName);
            if (bag.BagDescription == null)
            {
                cmd.Parameters.AddWithValue("@bagDescription", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@bagDescription", bag.BagDescription);
            }
            cmd.Parameters.AddWithValue("@accID", bag.AccID);

            conn.Open();
            bag.ShoppingBagID = (int)cmd.ExecuteScalar();

            conn.Close();
            return bag.ShoppingBagID;
        }

        // Retrieve single record
        public ShoppingBag GetDetails(int shoppingBagID)
        {
            ShoppingBag shoppingBag = new ShoppingBag();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM ShoppingBag
                                WHERE ShoppingBagID = @selectedShoppingBagID";
            cmd.Parameters.AddWithValue("@selectedShoppingBagID", shoppingBagID);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    shoppingBag.ShoppingBagID = shoppingBagID;
                    shoppingBag.CreatedAt = (DateTime)(!reader.IsDBNull(1) ? reader.GetDateTime(1) : (DateTime?)null);
                    shoppingBag.BagName = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    shoppingBag.BagDescription = !reader.IsDBNull(3) ? reader.GetString(3) : null;
                    shoppingBag.AccID = !reader.IsDBNull(4) ? reader.GetInt32(4) : (int)0;
                }
            }
            reader.Close();
            conn.Close();
            return shoppingBag;
        }

        // Update record
        public int Update(ShoppingBag bag)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE ShoppingBag SET CreatedAt=@createdAt,
                                BagName=@shoppingBagName, BagDescription= @bagDescription
                                WHERE ShoppingBagID = @selectedShoppingBagID";
            cmd.Parameters.AddWithValue("@createdAt", bag.CreatedAt);
            cmd.Parameters.AddWithValue("@shoppingBagName", bag.BagName);
            cmd.Parameters.AddWithValue("@selectedShoppingBagID", bag.ShoppingBagID);
            if (bag.BagDescription == null)
            {
                cmd.Parameters.AddWithValue("@bagDescription", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@bagDescription", bag.BagDescription);
            }

            conn.Open();
            int count = cmd.ExecuteNonQuery();

            conn.Close();
            return count;
        }


        // Delete record
        public int Delete(int shoppingBagID)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"IF OBJECT_ID('TempItem') IS NOT NULL
                                BEGIN
                                DROP TABLE TempItem
                                END;";

            SqlCommand cmd1 = conn.CreateCommand();
            cmd1.CommandText = @"SELECT ItemID INTO TempItem FROM ShoppingBagItem
                                 WHERE ShoppingBagID = @selectedShoppingBagID";
            cmd1.Parameters.AddWithValue("@selectedShoppingBagID", shoppingBagID);

            SqlCommand cmd2 = conn.CreateCommand();
            cmd2.CommandText = @"DELETE FROM ShoppingBagItem
                                WHERE ShoppingBagID = @selectedShoppingBagID";
            cmd2.Parameters.AddWithValue("@selectedShoppingBagID", shoppingBagID);

            SqlCommand cmd3 = conn.CreateCommand();
            cmd3.CommandText = @"DELETE FROM ITEM 
                                WHERE ItemID IN (
                                SELECT * FROM TempItem
                                )";

            SqlCommand cmd4 = conn.CreateCommand();
            cmd4.CommandText = @"DELETE FROM ShoppingBag
                                WHERE ShoppingBagID = @selectedShoppingBagID";
            cmd4.Parameters.AddWithValue("@selectedShoppingBagID", shoppingBagID);

            conn.Open();
            int rowAffected = 0;
            rowAffected += cmd.ExecuteNonQuery();
            rowAffected += cmd1.ExecuteNonQuery();
            rowAffected += cmd2.ExecuteNonQuery();
            rowAffected += cmd3.ExecuteNonQuery();
            rowAffected += cmd4.ExecuteNonQuery();

            conn.Close();
            return rowAffected;
        }

        public List<ShoppingBag> GetThisMonthBags(int accID)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM ShoppingBag WHERE AccID = @selectedAccID AND MONTH(CreatedAt)=MONTH(GETDATE()) 
                                            AND YEAR(CreatedAt)=YEAR(GETDATE())";
            cmd.Parameters.AddWithValue("@selectedAccID", accID);

            List<ShoppingBag> shoppingBagList = new List<ShoppingBag>();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                shoppingBagList.Add(
                new ShoppingBag
                {
                    ShoppingBagID = reader.GetInt32(0),
                    CreatedAt = reader.GetDateTime(1),
                    BagName = reader.GetString(2),
                    BagDescription = !reader.IsDBNull(3) ? reader.GetString(3) : null,
                    AccID = reader.GetInt32(4)
                }
                );
            }
            reader.Close();
            conn.Close();
            return shoppingBagList;
        }
    }
}
