using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Tests.Models;

namespace MinimalMediatR.Tests.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}