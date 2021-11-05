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
    public class AccountDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public AccountDAL()
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

            public List<Account> GetAllAccount()
            {
                //Create a SqlCommand object from connection object
                SqlCommand cmd = conn.CreateCommand();
                //Specify the SELECT SQL statement 
                cmd.CommandText = @"SELECT * FROM Account ORDER BY AccID";
                //Open a database connection
                conn.Open();
                //Execute the SELECT SQL through a DataReader
                SqlDataReader reader = cmd.ExecuteReader();
                //Read all records until the end, save data into a staff list
                List<Account> accountList = new List<Account>();
                while (reader.Read())
                {
                    accountList.Add(
                    new Account
                    {
                        AccID = reader.GetInt32(0), //0: 1st column
                        AccName = reader.GetString(1), //1: 2nd column
                        UserName = reader.GetString(2), //2: 3rd column
                        Pass_word = reader.GetString(3), //3: 4th column
                    }
                    );
                }
                //Close DataReader
                reader.Close();
                //Close the database connection
                conn.Close();
                return accountList;

            }
    }
}
