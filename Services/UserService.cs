using management_webapp_bn.Data;
using management_webapp_bn.DTOs;
using management_webapp_bn.Models;
using Microsoft.EntityFrameworkCore;

namespace management_webapp_bn.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }


    }
}
