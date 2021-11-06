﻿using GreenDemic.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDemic.DAL
{
    public class AccountDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;

        //Constructor
        public AccountDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "GreenDemicConnectionString");
            conn = new SqlConnection(strConn);
        }

        // Retrieve list of all accounts
        public List<Account> GetAllAccounts()
        {
            List<Account> accountList = new List<Account>();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Account ORDER BY AccID ASC";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                accountList.Add(
                new Account
                {
                    AccID = reader.GetInt32(0),
                    AccName = reader.GetString(1),
                    UserName = reader.GetString(2),
                    Pass_word = reader.GetString(3)
                }
                );
            }
            reader.Close();
            conn.Close();
            return accountList;
        }

        // Add account (sign up)
        public int Add(Account account)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Account (AccName, UserName, Pass_word)
                                OUTPUT INSERTED.AccID
                                VALUES(@accName, @userName, @password)";
            cmd.Parameters.AddWithValue("@accName", account.AccName);
            cmd.Parameters.AddWithValue("@userName", account.UserName);
            cmd.Parameters.AddWithValue("@password", account.Pass_word);

            conn.Open();
            account.AccID = (int)cmd.ExecuteScalar();

            conn.Close();
            return account.AccID;
        }

        // Retrieve single record
        public Account GetDetails(int accID)
        {
            Account account = new Account();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Account
                                WHERE AccID = @selectedAccountID";
            cmd.Parameters.AddWithValue("@selectedAccountID", accID);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    account.AccID = accID;
                    account.AccName = !reader.IsDBNull(3) ? reader.GetString(1) : null;
                    account.UserName = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    account.Pass_word = !reader.IsDBNull(3) ? reader.GetString(3) : null;
                }
            }
            reader.Close();
            conn.Close();
            return account;
        }

        // Update record
        public int Update(Account account)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE Account SET AccName=@accName,
                                UserName=@userName, Pass_word= @password
                                WHERE AccID = @selectedAccID";
            cmd.Parameters.AddWithValue("@selectedAccID", account.AccID);
            cmd.Parameters.AddWithValue("@accName", account.AccName);
            cmd.Parameters.AddWithValue("@userName", account.UserName);
            cmd.Parameters.AddWithValue("@password", account.Pass_word);

            conn.Open();
            int count = cmd.ExecuteNonQuery();

            conn.Close();
            return count;
        }


        // Delete record
        public int Delete(int accID)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM Account
                                WHERE AccID = @selectedAccID";
            cmd.Parameters.AddWithValue("@selectedAccID", accID);

            conn.Open();
            int rowAffected = 0;
            rowAffected += cmd.ExecuteNonQuery();

            conn.Close();
            return rowAffected;
        }
    }
}