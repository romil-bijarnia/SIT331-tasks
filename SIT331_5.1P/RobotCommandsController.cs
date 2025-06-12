using System.Collections.Generic;
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
        [HttpGet]
        public IEnumerable<RobotCommand> GetAll() =>
            RobotCommandDataAccess.GetRobotCommands();

        /// <summary>
        /// Retrieves a robot command by identifier.
        /// </summary>
        /// <param name="id">The identifier of the command.</param>
        /// <returns>The matching <see cref="RobotCommand"/> if found; otherwise a 404 result.</returns>
        [HttpGet("{id}")]
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
        [HttpPost]
        public ActionResult<RobotCommand> Create(RobotCommand rc)
        {
            var created = RobotCommandDataAccess.InsertRobotCommand(rc);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing robot command.
        /// </summary>
        /// <param name="id">Identifier of the command being updated.</param>
        /// <param name="rc">The updated command values.</param>
        /// <returns>An empty 204 result on success.</returns>
        [HttpPut("{id}")]
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
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ok = RobotCommandDataAccess.DeleteRobotCommand(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
