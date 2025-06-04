using System;
using System.Collections.Generic;
using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public static class RobotCommandDataAccess
    {
        private const string CONNECTION_STRING =
            "Host=localhost;Username=postgres;Password=;Database=sit331";

        public static List<RobotCommand> GetRobotCommands()
        {
            var list = new List<RobotCommand>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                "SELECT id, \"Name\", description, ismovecommand, createddate, modifieddate FROM robotcommand",
                conn);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new RobotCommand
                {
                    Id            = dr.GetInt32(0),
                    Name          = dr.GetString(1),
                    Description   = dr.IsDBNull(2) ? null : dr.GetString(2),
                    IsMoveCommand = dr.GetBoolean(3),
                    CreatedDate   = dr.GetDateTime(4),
                    ModifiedDate  = dr.GetDateTime(5)
                });
            }
            return list;
        }

        public static RobotCommand GetRobotCommand(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                "SELECT id, \"Name\", description, ismovecommand, createddate, modifieddate FROM robotcommand WHERE id = @id",
                conn);
            cmd.Parameters.AddWithValue("id", id);
            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new RobotCommand
                {
                    Id            = dr.GetInt32(0),
                    Name          = dr.GetString(1),
                    Description   = dr.IsDBNull(2) ? null : dr.GetString(2),
                    IsMoveCommand = dr.GetBoolean(3),
                    CreatedDate   = dr.GetDateTime(4),
                    ModifiedDate  = dr.GetDateTime(5)
                };
            }
            return null;
        }

        public static RobotCommand InsertRobotCommand(RobotCommand rc)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                @"INSERT INTO robotcommand (""Name"", description, ismovecommand, createddate, modifieddate)
                  VALUES (@name, @desc, @move, @created, @modified) RETURNING id",
                conn);
            cmd.Parameters.AddWithValue("name", rc.Name);
            cmd.Parameters.AddWithValue("desc", (object)rc.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("move", rc.IsMoveCommand);
            cmd.Parameters.AddWithValue("created", rc.CreatedDate);
            cmd.Parameters.AddWithValue("modified", rc.ModifiedDate);

            rc.Id = (int)cmd.ExecuteScalar();
            return rc;
        }

        public static bool UpdateRobotCommand(RobotCommand rc)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                @"UPDATE robotcommand
                  SET ""Name"" = @name,
                      description = @desc,
                      ismovecommand = @move,
                      createddate = @created,
                      modifieddate = @modified
                  WHERE id = @id",
                conn);
            cmd.Parameters.AddWithValue("name", rc.Name);
            cmd.Parameters.AddWithValue("desc", (object)rc.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("move", rc.IsMoveCommand);
            cmd.Parameters.AddWithValue("created", rc.CreatedDate);
            cmd.Parameters.AddWithValue("modified", rc.ModifiedDate);
            cmd.Parameters.AddWithValue("id", rc.Id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool DeleteRobotCommand(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                "DELETE FROM robotcommand WHERE id = @id",
                conn);
            cmd.Parameters.AddWithValue("id", id);
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
