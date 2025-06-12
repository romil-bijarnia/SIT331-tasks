    using robot_controller_api.Models;

    namespace robot_controller_api.Persistence;
    public interface IRobotCommandDataAccess
    {
         abstract bool DeleteRobotCommand(int id);
        abstract RobotCommand GetRobotCommand(int id);
        abstract List<RobotCommand> GetRobotCommands();
        abstract RobotCommand InsertRobotCommand(RobotCommand rc);
        abstract bool UpdateRobotCommand(RobotCommand rc);
    }