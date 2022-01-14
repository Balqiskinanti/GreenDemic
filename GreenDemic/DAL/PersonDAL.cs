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
                    MaxCal = !reader.IsDBNull(2) ? reader.GetInt32(2) : 0,
                    AccID = reader.GetInt32(3),
                    Height = reader.GetInt32(4),
                    Weight = reader.GetInt32(5),
                    Birthday = reader.GetDateTime(6),
                    Gender = reader.GetByte(7),
                    ExType = reader.GetByte(8)
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
            cmd.CommandText = @"INSERT INTO Person (UserName, MaxCal, AccID, Height, Weight, Birthday, Gender, ExType)
                                OUTPUT INSERTED.UserID
                                VALUES(@userName, @maxCal, @accID, @height, @weight, @birthday, @gender, @extype)";
            cmd.Parameters.AddWithValue("@username", person.UserName);
            if(person.MaxCal is null)
            {
                cmd.Parameters.AddWithValue("@maxCal", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@maxCal", person.MaxCal);
            }
            cmd.Parameters.AddWithValue("@accID", person.AccID);
            cmd.Parameters.AddWithValue("@height", person.Height);
            cmd.Parameters.AddWithValue("@weight", person.Weight);
            cmd.Parameters.AddWithValue("@birthday", person.Birthday);
            cmd.Parameters.AddWithValue("@gender", person.Gender);
            cmd.Parameters.AddWithValue("@extype", person.ExType);

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
                    person.MaxCal = !reader.IsDBNull(2) ? reader.GetInt32(2) : 0;
                    person.AccID = reader.GetInt32(3);
                    person.Height = reader.GetInt32(4);
                    person.Weight = reader.GetInt32(5);
                    person.Birthday = reader.GetDateTime(6);
                    person.Gender = reader.GetByte(7);
                    person.ExType = reader.GetByte(8);
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
                                MaxCal=@maxCal, Height=@height, Weight=@weight, Birthday=@birthday, Gender=@gender, ExType=@extype
                                WHERE UserID = @selectedUserID";
            cmd.Parameters.AddWithValue("@userName", person.UserName);
            if (person.MaxCal is null)
            {
                cmd.Parameters.AddWithValue("@maxCal", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@maxCal", person.MaxCal);
            }
            cmd.Parameters.AddWithValue("@selectedUserID", person.UserID);
            cmd.Parameters.AddWithValue("@height", person.Height);
            cmd.Parameters.AddWithValue("@weight", person.Weight);
            cmd.Parameters.AddWithValue("@birthday", person.Birthday);
            cmd.Parameters.AddWithValue("@gender", person.Gender);
            cmd.Parameters.AddWithValue("@extype", person.ExType);

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
