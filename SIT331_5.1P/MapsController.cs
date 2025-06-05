using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
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
        /// <response code="200">Returns all maps in the system.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Map> GetAll() =>
            MapDataAccess.GetMaps();

        /// <summary>
        /// Retrieves a single map by identifier.
        /// </summary>
        /// <param name="id">The identifier of the desired map.</param>
        /// <returns>The matching <see cref="Map"/> if found; otherwise a 404 result.</returns>
        /// <response code="200">Returns the requested map.</response>
        /// <response code="404">If a map with the specified ID is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <remarks>
        /// Sample request:  
        /// POST /api/Maps  
        /// {  
        ///     "columns": 10,  
        ///     "rows": 10,  
        ///     "name": "My First Map",  
        ///     "isSquare": true,  
        ///     "description": "This is a test map."  
        /// }
        /// </remarks>
        /// <response code="201">Returns the newly created map.</response>
        /// <response code="400">If the map is null.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Map> Create(Map map)
        {
            if (map == null) return BadRequest();
            var created = MapDataAccess.InsertMap(map);
            // Returns a 201 Created response with the URI of the new resource in the Location header
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing map.
        /// </summary>
        /// <param name="id">The identifier of the map being updated.</param>
        /// <param name="map">The updated map values.</param>
        /// <returns>An empty result with a 204 status code on success.</returns>
        /// <response code="204">Map updated successfully (no content).</response>
        /// <response code="400">If the provided ID does not match the map's ID.</response>
        /// <response code="404">If a map with the specified ID is not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <response code="204">Map deleted successfully (no content).</response>
        /// <response code="404">If a map with the specified ID is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var ok = MapDataAccess.DeleteMap(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
