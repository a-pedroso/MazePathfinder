using MazePathfinder.Domain.Maze;
using Microsoft.EntityFrameworkCore;

namespace MazePathfinder.Infrastructure.Persistence;

internal class MazeRepository(ApplicationDbContext applicationDbContext) : IMazeRepository
{
    public async Task<MazeEntity> AddMazeAsync(MazeEntity maze, CancellationToken cancellationToken)
    {
        applicationDbContext.Mazes.Add(maze);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return maze;
    }

    public async Task<IReadOnlyCollection<MazeEntity>> GetMazesAsync(CancellationToken cancellationToken)
    {
        var mazes = await applicationDbContext
            .Mazes.AsNoTracking()
            .ToListAsync(cancellationToken);

        return mazes.AsReadOnly();
    }

    public async Task<MazeEntity?> GetMazeByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await applicationDbContext
            .Mazes.AsNoTracking()
            .SingleOrDefaultAsync(maze => maze.Id == id, cancellationToken);
    }
}
