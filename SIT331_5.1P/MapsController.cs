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
        /// <summary>
        /// Retrieves all available maps.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Map}"/> containing every map.</returns>
        [HttpGet]
        public IEnumerable<Map> GetAll() =>
            MapDataAccess.GetMaps();

        /// <summary>
        /// Retrieves a single map by identifier.
        /// </summary>
        /// <param name="id">The identifier of the desired map.</param>
        /// <returns>The matching <see cref="Map"/> if found; otherwise a 404 result.</returns>
        [HttpGet("{id}")]
        public ActionResult<Map> Get(int id)
        {
            var map = MapDataAccess.GetMap(id);
            if (map == null) return NotFound();
            return map;
        }

        /// <summary>
        /// Creates a new map entry.
        /// </summary>
        /// <param name="map">The map to insert.</param>
        /// <returns>The created map with a 201 status code.</returns>
        [HttpPost]
        public ActionResult<Map> Create(Map map)
        {
            var created = MapDataAccess.InsertMap(map);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing map.
        /// </summary>
        /// <param name="id">The identifier of the map being updated.</param>
        /// <param name="map">The updated map values.</param>
        /// <returns>An empty result with a 204 status code on success.</returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, Map map)
        {
            if (id != map.Id) return BadRequest();
            var ok = MapDataAccess.UpdateMap(map);
            if (!ok) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Deletes the specified map.
        /// </summary>
        /// <param name="id">Identifier of the map to delete.</param>
        /// <returns>An empty 204 result on success.</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ok = MapDataAccess.DeleteMap(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
