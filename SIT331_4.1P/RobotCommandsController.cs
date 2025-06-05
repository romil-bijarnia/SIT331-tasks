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
        [HttpGet]
        public IEnumerable<RobotCommand> GetAll() =>
            RobotCommandDataAccess.GetRobotCommands();

        [HttpGet("{id}")]
        public ActionResult<RobotCommand> Get(int id)
        {
            var rc = RobotCommandDataAccess.GetRobotCommand(id);
            if (rc == null) return NotFound();
            return rc;
        }

        [HttpPost]
        public ActionResult<RobotCommand> Create(RobotCommand rc)
        {
            var created = RobotCommandDataAccess.InsertRobotCommand(rc);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, RobotCommand rc)
        {
            if (id != rc.Id) return BadRequest();
            var ok = RobotCommandDataAccess.UpdateRobotCommand(rc);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ok = RobotCommandDataAccess.DeleteRobotCommand(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
