using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra
{
    public class UserRepo(AppDbContext appDbContext) : IUserRepo
    {
        public async Task<int> Create(User user)
        {
            await appDbContext.User.AddAsync(user);

            return await appDbContext.SaveChangesAsync();
        }

        public async Task<User?> GetByNameOrEmailAsync(string name, string email)
            => await appDbContext.User.FirstOrDefaultAsync(x => x.Name.Equals(name) || x.Email.Equals(email));

        public async Task<User?> GetUserByEmailAndPasswordAsync(string email, string encryptedPassword) => await appDbContext.User.FirstOrDefaultAsync(x => x.Email == email && x.Password == encryptedPassword);


    }
}
