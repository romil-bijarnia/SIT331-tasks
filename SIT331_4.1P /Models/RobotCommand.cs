using System;

namespace robot_controller_api.Models
{
    public class RobotCommand
    {
        public int     Id            { get; set; }
        public string  Name          { get; set; } = string.Empty;
        public string? Description   { get; set; }
        public bool    IsMoveCommand { get; set; }
        public DateTime CreatedDate  { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
