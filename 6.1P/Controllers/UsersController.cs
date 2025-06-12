using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using robot_controller_api.Persistence;
using System;
using System.Collections.Generic;
using Isopoh.Cryptography.Argon2;

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class UsersController : ControllerBase
    {
        private readonly IUserDataAccess _userRepo;

        public UsersController(IUserDataAccess userRepo)
        {
            _userRepo = userRepo;
        }

        // GET /users
        [HttpGet]
        public ActionResult<IEnumerable<UserModel>> GetAll()
        {
            return Ok(_userRepo.GetAllUsers());
        }

        // GET /users/admin
        [HttpGet("admin")]
        public ActionResult<IEnumerable<UserModel>> GetAdmins()
        {
            return Ok(_userRepo.GetAdmins());
        }

        // GET /users/{id}
        [HttpGet("{id}")]
        public ActionResult<UserModel> GetById(int id)
        {
            var user = _userRepo.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // POST /users (Register)
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<UserModel> Register(UserModel user)
        {
            // Check for duplicate email
            var existingUser = _userRepo.GetUserByEmail(user.Email);
            if (existingUser != null)
                return Conflict("A user with this email already exists.");

            user.PasswordHash = Argon2.Hash(user.PasswordHash);
            user.CreatedDate = DateTime.UtcNow;
            user.ModifiedDate = DateTime.UtcNow;

            var created = _userRepo.InsertUser(user);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /users/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, UserModel updated)
        {
            updated.ModifiedDate = DateTime.UtcNow;
            var ok = _userRepo.UpdateUser(id, updated);
            if (!ok)
                return NotFound();
            return NoContent();
        }

        // DELETE /users/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ok = _userRepo.DeleteUser(id);
            if (!ok)
                return NotFound();
            return NoContent();
        }

        // PATCH /users/{id}
        [HttpPatch("{id}")]
        public IActionResult UpdateCredentials(int id, LoginModel model)
        {
            var ok = _userRepo.UpdateCredentials(id, model.Email, model.Password);
            if (!ok)
                return NotFound();
            return NoContent();
        }
    }
}
