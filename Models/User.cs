using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Bug_Tracker_Api.Models
{
    public class User
    {
        public long Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        internal AppDb Db { get; set; }

        public User()
        {
        }
        internal User (AppDb db)
        {
            Db = db;
        }
        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `User` (`First_Name`, `Last_Name`, `Email`, `Password`) VALUES (@first_Name, @last_Name, @email, @password);";
            BindParams(cmd);
            foreach(var p in cmd.Parameters) {
                Console.WriteLine("Param: {0}", p);
            }
            await cmd.ExecuteNonQueryAsync();
            Id = (int)cmd.LastInsertedId;

        }
        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"Update `User` SET `First_Name` = @first_Name, `Last_Name` = @last_Name, `Email` = @email, `Password` = @password WHERE `Id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `User` WHERE `Id` = @id;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = Id
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@first_Name",
                DbType = DbType.String,
                Value = First_Name
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@last_Name",
                DbType = DbType.String,
                Value = Last_Name
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@email",
                DbType = DbType.String,
                Value = Email
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@password",
                DbType = DbType.String,
                Value = Password
            });
        }
    }
}
