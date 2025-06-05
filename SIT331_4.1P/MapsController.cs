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
        /// Gets all maps.
        /// </summary>
        /// <returns>A list of maps.</returns>
        [HttpGet]
        public IEnumerable<Map> GetAll() =>
            MapDataAccess.GetMaps();

        /// <summary>
        /// Gets a specific map.
        /// </summary>
        /// <param name="id">Map identifier.</param>
        /// <returns>The requested map.</returns>
        /// <response code="404">If the map is not found.</response>
        [HttpGet("{id}")]
        public ActionResult<Map> Get(int id)
        {
            var map = MapDataAccess.GetMap(id);
            if (map == null) return NotFound();
            return map;
        }

        /// <summary>
        /// Creates a map.
        /// </summary>
        /// <param name="map">A new map.</param>
        /// <returns>The created map.</returns>
        /// <response code="201">Returns the newly created map.</response>
        [HttpPost]
        public ActionResult<Map> Create(Map map)
        {
            var created = MapDataAccess.InsertMap(map);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates a map.
        /// </summary>
        /// <param name="id">Map identifier.</param>
        /// <param name="map">Updated map.</param>
        /// <response code="204">Map updated successfully.</response>
        /// <response code="400">If identifiers do not match.</response>
        /// <response code="404">If the map is not found.</response>
        [HttpPut("{id}")]
        public IActionResult Update(int id, Map map)
        {
            if (id != map.Id) return BadRequest();
            var ok = MapDataAccess.UpdateMap(map);
            if (!ok) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Deletes a map.
        /// </summary>
        /// <param name="id">Map identifier.</param>
        /// <response code="204">Map deleted successfully.</response>
        /// <response code="404">If the map is not found.</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ok = MapDataAccess.DeleteMap(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
