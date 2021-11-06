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
    public class PersonDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;

        //Constructor
        public PersonDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "GreenDemicConnectionString");
            conn = new SqlConnection(strConn);
        }

        // Retrieve list of all person
        public List<Person> GetAllPerson(int accID)
        {
            List<Person> personList = new List<Person>();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Person WHERE AccID = @selectedAccID ORDER BY UserID ASC";
            cmd.Parameters.AddWithValue("@selectedAccID", accID);


            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                personList.Add(
                new Person
                {
                    UserID = reader.GetInt32(0),
                    UserName = reader.GetString(1),
                    MaxCal = reader.GetInt32(2),
                    AccID = reader.GetInt32(3)
                }
                );
            }
            reader.Close();
            conn.Close();
            return personList;
        }

        // Add person
        public int Add(Person person)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Person (UserName, MaxCal, AccID)
                                OUTPUT INSERTED.UserID
                                VALUES(@userName, @maxCal, @accID)";
            cmd.Parameters.AddWithValue("@username", person.UserName);
            cmd.Parameters.AddWithValue("@maxCal", person.MaxCal);
            cmd.Parameters.AddWithValue("@accID", person.AccID);

            conn.Open();
            person.UserID = (int)cmd.ExecuteScalar();

            conn.Close();
            return person.UserID;
        }

        // Retrieve single record
        public Person GetDetails(int userID)
        {
            Person person = new Person();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Person
                                WHERE UserID = @selectedUserID";
            cmd.Parameters.AddWithValue("@selectedUserID", userID);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    person.UserID = reader.GetInt32(0);
                    person.UserName = reader.GetString(1);
                    person.MaxCal = reader.GetInt32(2);
                    person.AccID = reader.GetInt32(3);
                }
            }
            reader.Close();
            conn.Close();
            return person;
        }

        // Update record
        public int Update(Person person)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE Person SET UserName=@userName,
                                MaxCal=@maxCal
                                WHERE UserID = @selectedUserID";
            cmd.Parameters.AddWithValue("@userName", person.UserName);
            cmd.Parameters.AddWithValue("@maxCal", person.MaxCal);
            cmd.Parameters.AddWithValue("@selectedUserID", person.UserID);

            conn.Open();
            int count = cmd.ExecuteNonQuery();

            conn.Close();
            return count;
        }


        // Delete record
        public int Delete(int userID)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM Person
                                WHERE UserID = @selectedUserID";
            cmd.Parameters.AddWithValue("@selectedUserID", userID);

            conn.Open();
            int rowAffected = 0;
            rowAffected += cmd.ExecuteNonQuery();

            conn.Close();
            return rowAffected;
        }
    }
}
