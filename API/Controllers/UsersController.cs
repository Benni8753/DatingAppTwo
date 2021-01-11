using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _injUserRepository;
        private readonly IMapper _injMapper;
        private readonly IPhotoService _injPhotoService;

        public UsersController(IUserRepository userRepository, IMapper mapper,
            IPhotoService injPhotoService)
        {
            _injMapper = mapper;
            _injUserRepository = userRepository;
            _injPhotoService = injPhotoService;
        }
        // example ==> api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllTheUsersFromDb()
        {
            var users = await _injUserRepository.GetMembersAsync();

            return Ok(users);
        }


        // example ==> api/users/3
        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetASingleUserFromDb(string username)
        {
            return await _injUserRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto receivingMemberUpdateDto)
        {
            var user = await _injUserRepository.GetUserByUsernameAsync(User.GetUsername());

            _injMapper.Map(receivingMemberUpdateDto, user);

            _injUserRepository.Update(user);

            if(await _injUserRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user. This was send from the HttpPut in UsersController");
        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _injUserRepository.GetUserByUsernameAsync(User.GetUsername());

            var result = await _injPhotoService.AddPhotoAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId 
            };

            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if(await _injUserRepository.SaveAllAsync())
            {
                // return CreatedAtRoute("GetUser", _injMapper.Map<PhotoDto>(photo));
                return CreatedAtRoute("GetUser", new{username = user.UserName}, _injMapper.Map<PhotoDto>(photo));
            }
                

            return BadRequest("Problem adding photo. This message was created by usercontroller line 87");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _injUserRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if(currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if(await _injUserRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId) 
        {
            var user = await _injUserRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("You cannot delete you main photo");

            if(photo.PublicId != null)
            {
                var result = await _injPhotoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if(await _injUserRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete photo");
        }
    }
}