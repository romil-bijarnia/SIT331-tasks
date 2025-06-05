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
        /// Gets all robot commands.
        /// </summary>
        /// <returns>A list of robot commands.</returns>
        [HttpGet]
        public IEnumerable<RobotCommand> GetAll() =>
            RobotCommandDataAccess.GetRobotCommands();

        /// <summary>
        /// Gets a specific robot command.
        /// </summary>
        /// <param name="id">Command identifier.</param>
        /// <returns>The requested robot command.</returns>
        /// <response code="404">If the command is not found.</response>
        [HttpGet("{id}")]
        public ActionResult<RobotCommand> Get(int id)
        {
            var rc = RobotCommandDataAccess.GetRobotCommand(id);
            if (rc == null) return NotFound();
            return rc;
        }

        /// <summary>
        /// Creates a robot command.
        /// </summary>
        /// <param name="rc">A new robot command.</param>
        /// <returns>The created robot command.</returns>
        /// <response code="201">Returns the newly created command.</response>
        [HttpPost]
        public ActionResult<RobotCommand> Create(RobotCommand rc)
        {
            var created = RobotCommandDataAccess.InsertRobotCommand(rc);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates a robot command.
        /// </summary>
        /// <param name="id">Command identifier.</param>
        /// <param name="rc">Updated robot command.</param>
        /// <response code="204">Command updated successfully.</response>
        /// <response code="400">If identifiers do not match.</response>
        /// <response code="404">If the command is not found.</response>
        [HttpPut("{id}")]
        public IActionResult Update(int id, RobotCommand rc)
        {
            if (id != rc.Id) return BadRequest();
            var ok = RobotCommandDataAccess.UpdateRobotCommand(rc);
            if (!ok) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Deletes a robot command.
        /// </summary>
        /// <param name="id">Command identifier.</param>
        /// <response code="204">Command deleted successfully.</response>
        /// <response code="404">If the command is not found.</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ok = RobotCommandDataAccess.DeleteRobotCommand(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
