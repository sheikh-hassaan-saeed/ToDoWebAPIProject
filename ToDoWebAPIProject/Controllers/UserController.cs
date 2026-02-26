using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoWebAPIProject.Models;

namespace ToDoWebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly UserContext _userContext;

        public UserController(UserContext userContext)
        {
            _userContext = userContext;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUser()
        {
            var users = await _userContext.users.ToListAsync();
            if (users.Count == 0)
            {
                return NotFound();
            }
            return Ok(users);
        }
        [HttpPost]
        [Route("AddUser")]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            _userContext.users.Add(user);
            await _userContext.SaveChangesAsync();
            return Ok(User);
        }


        [HttpPut]
        [Route("UpdateUser")]
        public async Task<ActionResult<IEnumerable<User>>> UpdateUser(User user)
        {
            _userContext.users.Update(user);
            await _userContext.SaveChangesAsync();

            return Ok(string.Empty);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserByID(int id)
        {
            var user =  await _userContext.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);

        }

    }
}
