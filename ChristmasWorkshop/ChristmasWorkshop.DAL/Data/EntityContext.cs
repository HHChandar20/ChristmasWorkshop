using ChristmasWorkshop.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ChristmasWorkshop.DAL.Data;

public class EntityContext : DbContext
{
    public DbSet<Light> Lights { get; set; }

    public EntityContext(DbContextOptions<EntityContext> options) : base(options)
    {
    }
}