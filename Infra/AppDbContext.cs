using Domain.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> User => Set<User>();

        public DbSet<Address> Address => Set<Address>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
        {
        }

        //migrations
        //no console do gerenciador de pacotes selecione o dal referente:
        //EntityFrameworkCore\Add-Migration "Init" -Context AppDbContext
        //EntityFrameworkCore\update-database -Context AppDbContext

        //to remove last migration snapshot
        //EntityFrameworkCore\Remove-Migration -Context AppDbContext
    }
}
