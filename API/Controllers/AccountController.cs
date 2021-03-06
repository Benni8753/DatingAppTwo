using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _injMapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _injMapper = mapper;
            this._context = context;
            this._tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // calls the UserExists function to check if the name is taken.
            if (await UserExists(registerDto.Username)) return BadRequest("Usename is taken. This message has been created by AccountController");

            var user = _injMapper.Map<AppUser>(registerDto);
            // hmac helps to encrypt the password. It will set hash the password the user is given.
            using var hmac = new HMACSHA512();


            user.UserName = registerDto.Username;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;


            _context.AllMyUsers.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs

            };
        }
        // The method below checks if the username is already existing in the database.
        private async Task<bool> UserExists(string username)
        {
            return await _context.AllMyUsers.AnyAsync(AUserFromDb => AUserFromDb.UserName == username.ToLower());
        }
        //this is the endpoint when the user wants to login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.AllMyUsers
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(AUserFromDb => AUserFromDb.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs
            };
        }

    }
}