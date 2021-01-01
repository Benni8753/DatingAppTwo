using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _injUserRepository;
        private readonly IMapper _injMapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _injMapper = mapper;
            _injUserRepository = userRepository;
        }
        // example ==> api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllTheUsersFromDb()
        {
            var users = await _injUserRepository.GetMembersAsync();

            return Ok(users);
        }


        // example ==> api/users/3
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetASingleUserFromDb(string username)
        {
            return await _injUserRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto receivingMemberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _injUserRepository.GetUserByUsernameAsync(username);

            _injMapper.Map(receivingMemberUpdateDto, user);

            _injUserRepository.Update(user);

            if(await _injUserRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user. This was send from the HttpPut in UsersController");
        }
    }
}