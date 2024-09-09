namespace MazePathfinder.Infrastructure.Persistence;

using MazePathfinder.Domain.Maze;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<MazeEntity> Mazes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MazeEntity>(entity =>
        {
            entity.ToTable("mazes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Map).IsRequired();
            entity.Property(e => e.Solution);
            entity.Property(e => e.UsedAlgorithm);
        });
    }
}
