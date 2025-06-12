using System;
using System.Collections.Generic;
using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
 
    public class UserRepository : IUserDataAccess
    {
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=;Database=postgres";

        public List<UserModel> GetAllUsers()
        {
            var users = new List<UserModel>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM public.\"user\";", conn);
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                users.Add(Map(dr));
            }

            return users;
        }

        public List<UserModel> GetAdmins()
        {
            var admins = new List<UserModel>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM public.\"user\" WHERE role = 'Admin';", conn);
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                admins.Add(Map(dr));
            }

            return admins;
        }

        public UserModel GetUserById(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM public.\"user\" WHERE id = @id;", conn);
            cmd.Parameters.AddWithValue("id", id);

            using var dr = cmd.ExecuteReader();
            return dr.Read() ? Map(dr) : null;
        }

        public UserModel GetUserByEmail(string email)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM public.\"user\" WHERE email = @e;", conn);
            cmd.Parameters.AddWithValue("e", email);

            using var dr = cmd.ExecuteReader();
            return dr.Read() ? Map(dr) : null;
        }

        public UserModel InsertUser(UserModel user)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
                INSERT INTO public.""user"" 
                (email, firstname, lastname, passwordhash, description, role, createddate, modifieddate)
                VALUES 
                (@e, @f, @l, @p, @d, @r, @c, @m)
                RETURNING id;", conn);

            cmd.Parameters.AddWithValue("e", user.Email);
            cmd.Parameters.AddWithValue("f", user.FirstName);
            cmd.Parameters.AddWithValue("l", user.LastName);
            cmd.Parameters.AddWithValue("p", user.PasswordHash);
            cmd.Parameters.AddWithValue("d", (object?)user.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("r", (object?)user.Role ?? DBNull.Value);
            cmd.Parameters.AddWithValue("c", user.CreatedDate);
            cmd.Parameters.AddWithValue("m", user.ModifiedDate);

            user.Id = (int)cmd.ExecuteScalar();
            return user;
        }

        public bool UpdateUser(int id, UserModel updated)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
                UPDATE public.""user""
                SET firstname = @f,
                    lastname = @l,
                    description = @d,
                    role = @r,
                    modifieddate = @m
                WHERE id = @id;", conn);

            cmd.Parameters.AddWithValue("f", updated.FirstName);
            cmd.Parameters.AddWithValue("l", updated.LastName);
            cmd.Parameters.AddWithValue("d", (object?)updated.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("r", (object?)updated.Role ?? DBNull.Value);
            cmd.Parameters.AddWithValue("m", updated.ModifiedDate);
            cmd.Parameters.AddWithValue("id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool UpdateCredentials(int id, string email, string passwordHash)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
                UPDATE public.""user""
                SET email = @e,
                    passwordhash = @p,
                    modifieddate = current_timestamp
                WHERE id = @id;", conn);

            cmd.Parameters.AddWithValue("e", email);
            cmd.Parameters.AddWithValue("p", passwordHash);
            cmd.Parameters.AddWithValue("id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool DeleteUser(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM public.\"user\" WHERE id = @id;", conn);
            cmd.Parameters.AddWithValue("id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        private UserModel Map(NpgsqlDataReader dr)
        {
            return new UserModel
            {
                Id = dr.GetInt32(dr.GetOrdinal("id")),
                Email = dr.GetString(dr.GetOrdinal("email")),
                FirstName = dr.GetString(dr.GetOrdinal("firstname")),
                LastName = dr.GetString(dr.GetOrdinal("lastname")),
                PasswordHash = dr.GetString(dr.GetOrdinal("passwordhash")),
                Description = dr.IsDBNull(dr.GetOrdinal("description")) ? null : dr.GetString(dr.GetOrdinal("description")),
                Role = dr.IsDBNull(dr.GetOrdinal("role")) ? null : dr.GetString(dr.GetOrdinal("role")),
                CreatedDate = dr.GetDateTime(dr.GetOrdinal("createddate")),
                ModifiedDate = dr.GetDateTime(dr.GetOrdinal("modifieddate"))
            };
        }
    }

}
