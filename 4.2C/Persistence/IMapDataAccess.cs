    using robot_controller_api.Models;

    namespace robot_controller_api.Persistence;
    public interface IMapDataAccess
    {
        bool DeleteMap(int id);
        Map GetMap(int id);
        List<Map> GetMaps();
        Map InsertMap(Map map);
        bool UpdateMap(Map map);
    }