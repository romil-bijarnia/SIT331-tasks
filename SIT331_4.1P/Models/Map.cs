namespace robot_controller_api.Models
{
    public class Map
    {
        public int    Id      { get; set; }
        public string Name    { get; set; } = string.Empty;
        public int    Rows    { get; set; }
        public int    Columns { get; set; }
        public bool   IsSquare{ get; set; }
    }
}
