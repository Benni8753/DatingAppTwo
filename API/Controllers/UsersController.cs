using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }
        // example ==> api/users
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<AppUser>> GetAllTheUsersFromDb() => await _context.AllMyUsers.ToListAsync();

        // example ==> api/users/3
        [Authorize] // enforces authorization to request data
        [HttpGet("{wantedId}")]
        public async Task<AppUser> GetASingleUserFromDb(int wantedId) => await _context.AllMyUsers.FindAsync(wantedId);
    }
}