using System.Collections.Generic;
using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public static class MapDataAccess
    {
        private const string CONNECTION_STRING =
            "Host=localhost;Username=postgres;Password=;Database=sit331";

        public static List<Map> GetMaps()
        {
            var list = new List<Map>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                "SELECT id, \"Name\", rows, columns, issquare FROM map",
                conn);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new Map
                {
                    Id       = dr.GetInt32(0),
                    Name     = dr.GetString(1),
                    Rows     = dr.GetInt32(2),
                    Columns  = dr.GetInt32(3),
                    IsSquare = dr.GetBoolean(4)
                });
            }
            return list;
        }

        public static Map GetMap(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                "SELECT id, \"Name\", rows, columns, issquare FROM map WHERE id = @id",
                conn);
            cmd.Parameters.AddWithValue("id", id);
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new Map
                {
                    Id       = dr.GetInt32(0),
                    Name     = dr.GetString(1),
                    Rows     = dr.GetInt32(2),
                    Columns  = dr.GetInt32(3),
                    IsSquare = dr.GetBoolean(4)
                };
            }
            return null;
        }

        public static Map InsertMap(Map map)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                @"INSERT INTO map (""Name"", rows, columns)
                  VALUES (@name, @rows, @columns) RETURNING id",
                conn);
            cmd.Parameters.AddWithValue("name", map.Name);
            cmd.Parameters.AddWithValue("rows", map.Rows);
            cmd.Parameters.AddWithValue("columns", map.Columns);

            map.Id = (int)cmd.ExecuteScalar();
            return map;
        }

        public static bool UpdateMap(Map map)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                @"UPDATE map
                  SET ""Name"" = @name,
                      rows = @rows,
                      columns = @columns
                  WHERE id = @id",
                conn);
            cmd.Parameters.AddWithValue("name", map.Name);
            cmd.Parameters.AddWithValue("rows", map.Rows);
            cmd.Parameters.AddWithValue("columns", map.Columns);
            cmd.Parameters.AddWithValue("id", map.Id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool DeleteMap(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                "DELETE FROM map WHERE id = @id",
                conn);
            cmd.Parameters.AddWithValue("id", id);
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
