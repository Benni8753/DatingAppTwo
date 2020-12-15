using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }
        // example ==> api/users
        [HttpGet]
        public async Task<IEnumerable<AppUser>> GetAllTheUsersFromDb() => await _context.AllMyUsers.ToListAsync();

        // example ==> api/users/3
        [HttpGet("{wantedId}")]
        public async Task<AppUser> GetASingleUserFromDb(int wantedId) => await _context.AllMyUsers.FindAsync(wantedId);
    }
}