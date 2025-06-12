using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class MapRepository : IMapDataAccess, IRepository
    {
        private IRepository _repo => this;

        public List<Map> GetMaps()
        {
            return _repo.ExecuteReader<Map>("SELECT * FROM public.map");
        }

        public Map GetMap(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("id", id)
            };

            return _repo.ExecuteReader<Map>(
                "SELECT * FROM public.map WHERE id = @id",
                sqlParams).SingleOrDefault();
        }

        public Map InsertMap(Map map)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("name", map.Name),
                new("rows", map.Rows),
                new("columns", map.Columns)
            };

            return _repo.ExecuteReader<Map>(
                @"INSERT INTO public.map (""Name"", rows, columns)
                  VALUES (@name, @rows, @columns)
                  RETURNING *;",
                sqlParams).Single();
        }

        public bool UpdateMap(Map map)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("id", map.Id),
                new("name", map.Name),
                new("rows", map.Rows),
                new("columns", map.Columns)
            };

            var result = _repo.ExecuteReader<Map>(
                @"UPDATE public.map
                  SET ""Name"" = @name,
                      rows = @rows,
                      columns = @columns
                  WHERE id = @id
                  RETURNING *;",
                sqlParams);

            return result.Any();
        }

        public bool DeleteMap(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("id", id)
            };

            var result = _repo.ExecuteReader<Map>(
                "DELETE FROM public.map WHERE id = @id RETURNING *;",
                sqlParams);

            return result.Any();
        }
    }
}
