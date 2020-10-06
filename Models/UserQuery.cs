using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;


namespace Bug_Tracker_Api.Models
{
    public class UserQuery
    {
        public AppDb Db { get; }

        public UserQuery(AppDb db)
        {
            Db = db;
        }
        [HttpGet]
        public async Task<List<User>> GetUsersAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `User`;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<User> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `User` WHERE `Id` = @id;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task DeleteAllAsync()
        {
            using var transaction = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `User`;";
            cmd.Transaction = transaction;
            await cmd.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
        }
        private async Task<List<User>> ReadAllAsync(DbDataReader reader)
        {
            var users = new List<User>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var user = new User(Db)
                    {
                        Id = reader.GetInt32(0),
                        First_Name = reader.GetString(1),
                        Last_Name = reader.GetString(2),
                        Email = reader.GetString(3),
                        Password = reader.GetString(4),

                    };
                    users.Add(user);
                }
            }
            return users;
        }
    }
}
