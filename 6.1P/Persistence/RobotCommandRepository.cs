using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class RobotCommandRepository : IRobotCommandDataAccess, IRepository
    {
        private IRepository _repo => this;

        public List<RobotCommand> GetRobotCommands()
        {
            return _repo.ExecuteReader<RobotCommand>(
                "SELECT * FROM public.robotcommand");
        }

        public RobotCommand GetRobotCommand(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("id", id)
            };

            return _repo.ExecuteReader<RobotCommand>(
                "SELECT * FROM public.robotcommand WHERE id = @id",
                sqlParams).SingleOrDefault();
        }

        public RobotCommand InsertRobotCommand(RobotCommand rc)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("name", rc.Name),
                new("description", rc.Description ?? (object)DBNull.Value),
                new("ismovecommand", rc.IsMoveCommand)
            };

            return _repo.ExecuteReader<RobotCommand>(
                @"INSERT INTO public.robotcommand (""Name"", description, ismovecommand, createddate, modifieddate)
                  VALUES (@name, @description, @ismovecommand, current_timestamp, current_timestamp)
                  RETURNING *;",
                sqlParams).Single();
        }

        public bool UpdateRobotCommand(RobotCommand rc)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("id", rc.Id),
                new("name", rc.Name),
                new("description", rc.Description ?? (object)DBNull.Value),
                new("ismovecommand", rc.IsMoveCommand)
            };

            var result = _repo.ExecuteReader<RobotCommand>(
                @"UPDATE public.robotcommand
                  SET ""Name"" = @name,
                      description = @description,
                      ismovecommand = @ismovecommand,
                      modifieddate = current_timestamp
                  WHERE id = @id
                  RETURNING *;",
                sqlParams);

            return result.Any();
        }

        public bool DeleteRobotCommand(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("id", id)
            };

            var result = _repo.ExecuteReader<RobotCommand>(
                "DELETE FROM public.robotcommand WHERE id = @id RETURNING *;",
                sqlParams);

            return result.Any();
        }
    }
}
