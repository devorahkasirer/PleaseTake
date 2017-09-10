using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsWithLogin.Data
{
    public class Manager
    {
        private string _connectionString;
        public Manager(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IEnumerable<Ad> GetAds()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Ads";
                connection.Open();
                List<Ad> result = new List<Ad>();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Ad ad = new Ad
                    {
                        Id = (int)reader["Id"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                        DateListed = (DateTime)reader["DateListed"],
                        FileName = (string)reader["FileName"],
                        User = GetUserById((int)reader["UserId"])
                    };
                    result.Add(ad);
                }
                return result;
            }
        }
        public User GetUserById(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Users WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", Id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(!reader.Read())
                {
                    return null;
                }
                return new User
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    PhoneNumber = (int)reader["PhoneNumber"],
                    Email = (string)reader["Email"],
                    PasswordHash = (string)reader["PasswordHash"],
                    PasswordSalt = (string)reader["PasswordSalt"]
                };
            }
        }
        public int AddAd(Ad Ad, int UserId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Ads VALUES (@title, @description, @file, @date, @userId); SELECT @@IDENTITY;";
                command.Parameters.AddWithValue("@title", Ad.Title);
                command.Parameters.AddWithValue("@description", Ad.Description);
                command.Parameters.AddWithValue("@file", Ad.FileName);
                command.Parameters.AddWithValue("@date", Ad.DateListed);
                command.Parameters.AddWithValue("@userId", UserId);
                connection.Open();
                return (int)(decimal)command.ExecuteScalar();
            }
        }
        public void AddUser(User user, string password)
        {
            string salt = PasswordHelper.GenerateSalt();
            string hash = PasswordHelper.HashPassword(password, salt);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Users VALUES (@first, @last, @phone, @email, @hash, @salt)";
                command.Parameters.AddWithValue("@first", user.FirstName);
                command.Parameters.AddWithValue("@last", user.LastName);
                command.Parameters.AddWithValue("@phone", user.PhoneNumber);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@hash", hash);
                command.Parameters.AddWithValue("@salt", salt);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public Ad GetAdById(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Ads WHERE ID = @Id";
                command.Parameters.AddWithValue("@Id", Id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }
                return new Ad
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    Description = (string)reader["Description"],
                    DateListed = (DateTime)reader["DateListed"],
                    FileName = (string)reader["FileName"],
                    User = GetUserById((int)reader["UserId"])
                };
            }
        }
        public void RemoveAd(int Id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Ads WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", Id);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public User Login(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                User user = GetByEmail(email);
                if (user == null)
                {
                    return null;
                }
                bool Match = PasswordHelper.PasswordMatch(password, user.PasswordSalt, user.PasswordHash);
                if (!Match)
                {
                    return null;
                }
                return user;
            }
        }
        public User GetByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Users WHERE Email = @email";
                command.Parameters.AddWithValue("@email", email);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }
                return new User
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    PhoneNumber = (int)reader["PhoneNumber"],
                    Email = (string)reader["Email"],
                    PasswordHash = (string)reader["PasswordHash"],
                    PasswordSalt = (string)reader["PasswordSalt"]
                };
            }
        }
    }
}
