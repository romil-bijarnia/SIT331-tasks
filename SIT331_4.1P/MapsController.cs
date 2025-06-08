using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MapsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Map> GetAll() =>
            MapDataAccess.GetMaps();

        [HttpGet("{id}")]
        public ActionResult<Map> Get(int id)
        {
            var map = MapDataAccess.GetMap(id);
            if (map == null) return NotFound();
            return map;
        }

        [HttpPost]
        public ActionResult<Map> Create(Map map)
        {
            var created = MapDataAccess.InsertMap(map);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Map map)
        {
            if (id != map.Id) return BadRequest();
            var ok = MapDataAccess.UpdateMap(map);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ok = MapDataAccess.DeleteMap(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
