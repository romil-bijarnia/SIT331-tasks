using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RobotCommandsController : ControllerBase
    {
        private readonly IRobotCommandDataAccess _robotCommandsRepo;

        public RobotCommandsController(IRobotCommandDataAccess robotCommandsRepo)
        {
            _robotCommandsRepo = robotCommandsRepo;
        }

        // ✅ Accessible by any authenticated user (Admin or User)
        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        public IEnumerable<RobotCommand> GetAll()
        {
            return _robotCommandsRepo.GetRobotCommands();
        }

        [HttpGet("{id}")]
        public ActionResult<RobotCommand> Get(int id)
        {
            var rc = _robotCommandsRepo.GetRobotCommand(id);
            if (rc == null) return NotFound();
            return rc;
        }

        [HttpPost]
        public ActionResult<RobotCommand> Create(RobotCommand rc)
        {
            var created = _robotCommandsRepo.InsertRobotCommand(rc);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, RobotCommand rc)
        {
            if (id != rc.Id) return BadRequest();

            var ok = _robotCommandsRepo.UpdateRobotCommand(rc);
            if (!ok) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ok = _robotCommandsRepo.DeleteRobotCommand(id);
            if (!ok) return NotFound();

            return NoContent();
        }
    }
}
