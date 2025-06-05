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
    public class RobotCommandsController : ControllerBase
    {
        /// <summary>
        /// Retrieves all robot commands.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{RobotCommand}"/> containing every command.</returns>
        /// <response code="200">Returns all robot commands.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<RobotCommand> GetAll() =>
            RobotCommandDataAccess.GetRobotCommands();

        /// <summary>
        /// Retrieves a robot command by identifier.
        /// </summary>
        /// <param name="id">The identifier of the command.</param>
        /// <returns>The matching <see cref="RobotCommand"/> if found; otherwise a 404 result.</returns>
        /// <response code="200">Returns the requested command.</response>
        /// <response code="404">If a command with the specified ID is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RobotCommand> Get(int id)
        {
            var rc = RobotCommandDataAccess.GetRobotCommand(id);
            if (rc == null) return NotFound();
            return rc;
        }

        /// <summary>
        /// Creates a new robot command entry.
        /// </summary>
        /// <param name="rc">The command to insert.</param>
        /// <returns>The created command with a 201 status code.</returns>
        /// <remarks>
        /// Sample request:  
        /// POST /api/RobotCommands  
        /// {  
        ///     "name": "DANCE",  
        ///     "isMoveCommand": true,  
        ///     "description": "Salsa on the Moon"  
        /// }
        /// </remarks>
        /// <response code="201">Returns the newly created robot command.</response>
        /// <response code="400">If the robot command is null.</response>
        /// <response code="409">If a robot command with the same name already exists.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<RobotCommand> Create(RobotCommand rc)
        {
            if (rc == null) return BadRequest();
            // Check for duplicate command name to avoid conflicts
            if (RobotCommandDataAccess.GetRobotCommands().Any(c => c.Name == rc.Name))
            {
                return Conflict();
            }
            var created = RobotCommandDataAccess.InsertRobotCommand(rc);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing robot command.
        /// </summary>
        /// <param name="id">Identifier of the command being updated.</param>
        /// <param name="rc">The updated command values.</param>
        /// <returns>An empty 204 result on success.</returns>
        /// <response code="204">Robot command updated successfully (no content).</response>
        /// <response code="400">If the provided ID does not match the command's ID.</response>
        /// <response code="404">If a command with the specified ID is not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, RobotCommand rc)
        {
            if (id != rc.Id) return BadRequest();
            var ok = RobotCommandDataAccess.UpdateRobotCommand(rc);
            if (!ok) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Deletes the specified robot command.
        /// </summary>
        /// <param name="id">Identifier of the command to delete.</param>
        /// <returns>An empty 204 result on success.</returns>
        /// <response code="204">Robot command deleted successfully (no content).</response>
        /// <response code="404">If a command with the specified ID is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var ok = RobotCommandDataAccess.DeleteRobotCommand(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
