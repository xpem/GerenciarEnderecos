using Domain.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Infra
{
    public class AddressRepo(AppDbContext appDbContext) : IAddressRepo
    {
        public async Task Create(Address address)
        {
            await appDbContext.Address.AddAsync(address);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<Address?> GetByIdAsync(int id, int uid) => await appDbContext.Address.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);

        public async Task<List<Address>> GetAsync(int uid) => await appDbContext.Address.Where(x => x.UserId == uid).ToListAsync();

        public async Task DeleteAsync(int id, int uid)
        {
            appDbContext.ChangeTracker?.Clear();
            await appDbContext.Address.Where(x => x.Id == id && x.UserId == uid).ExecuteDeleteAsync();
        }

        public async Task Update(Address address)
        {
            appDbContext.ChangeTracker?.Clear();
            appDbContext.Address.Update(address);
            await appDbContext.SaveChangesAsync();
        }
    }
}
