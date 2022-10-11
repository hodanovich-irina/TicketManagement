using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketManagement.UserAPI.Dto;

namespace TicketManagement.UserAPI.DataAccess
{
    /// <summary>
    /// Identity context.
    /// </summary>
    public class UserApiDbContext : IdentityDbContext<UserDto>
    {
        public UserApiDbContext(DbContextOptions<UserApiDbContext> options)
            : base(options)
        {
        }
    }
}