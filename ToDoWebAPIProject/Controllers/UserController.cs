using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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

        [HttpGet("test-filter-speed")]
        public async Task<ActionResult> TestFilterSpeed()
        {
            // Warm-up EF Core
            await _userContext.users.FirstOrDefaultAsync();

            // Unfiltered query
            var stopwatch = Stopwatch.StartNew();
            var allUsers = await _userContext.users.ToListAsync();
            stopwatch.Stop();
            var unfilteredTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Unfiltered: {allUsers.Count} rows, Time: {unfilteredTime} ms");

            // Filtered query
            stopwatch.Restart();
            var filteredUsers = await _userContext.users
                .Where(u => u.Title == "Coding")
                .ToListAsync();
            stopwatch.Stop();
            var filteredTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Filtered: {filteredUsers.Count} rows, Time: {filteredTime} ms");

            // Calculate difference
            var diff = unfilteredTime - filteredTime;
            var reductionPercent = (double)diff / unfilteredTime * 100;

            return Ok(new
            {
                Unfiltered = new { Count = allUsers.Count, TimeMs = unfilteredTime },
                Filtered = new { Count = filteredUsers.Count, TimeMs = filteredTime },
                DifferenceMs = diff,
                ReductionPercent = reductionPercent
            });
        }

        [HttpPost("GenerateDummyTodos")]
        public async Task<ActionResult> GenerateDummyTodos(int count = 10000)
        {
            var random = new Random();
            var titles = new[] { "Coding", "Cooking", "Playing", "Study" }; 

            for (int i = 1; i <= count; i++)
            {
                var user = new User
                {
                    Title = titles[random.Next(titles.Length)],
                    Description = $"This is description {i}",
                    DueDate = DateTime.Now.AddMinutes(random.Next(0, 1440)) 
                };

                _userContext.users.Add(user);

                // Save every 100 users
                if (i % 100 == 0)
                    await _userContext.SaveChangesAsync();
            }

            await _userContext.SaveChangesAsync();

            return Ok($"{count} dummy todos added!");
        }


        [HttpGet("filtered")]
        public async Task<ActionResult<IEnumerable<User>>> GetFilteredUsers(
            int? Id = null,
            string? Title = null
            )

        {
            var stopwatch = Stopwatch.StartNew();

            var query = _userContext.users.AsQueryable();

            if (Id.HasValue)
            {
                query = query.Where(u => u.ID == Id.Value);
            }

            if (!string.IsNullOrEmpty(Title))
            {
                query = query.Where(u => u.Title == Title);
            }

            var users = await query.ToListAsync();

            stopwatch.Stop();

            Console.WriteLine($"Time Taken: {stopwatch.ElapsedMilliseconds} ms");


            return Ok(users);
        }



        [HttpPost]
        [Route("AddUser")]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            _userContext.users.Add(user);
            await _userContext.SaveChangesAsync();
            return Ok(user);
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
            var user = await _userContext.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> UserDelete(int id)
        {
            var user = await _userContext.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _userContext.users.Remove(user);
            await _userContext.SaveChangesAsync();

            return Ok(user);
        }


    }
}
