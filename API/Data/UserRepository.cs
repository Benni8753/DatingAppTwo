using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _injDataContext;
        private readonly IMapper _injMapper;
        public UserRepository(DataContext injDataContext, IMapper mapper)
        {
            _injMapper = mapper;
            _injDataContext = injDataContext;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _injDataContext.AllMyUsers
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_injMapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _injDataContext.AllMyUsers
                .ProjectTo<MemberDto>(_injMapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _injDataContext.AllMyUsers.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _injDataContext.AllMyUsers
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _injDataContext.AllMyUsers
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _injDataContext.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _injDataContext.Entry(user).State = EntityState.Modified;
        }
    }
}