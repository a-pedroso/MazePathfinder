namespace MazePathfinder.Domain.Maze;
public interface IMazeRepository
{
    public Task<IReadOnlyCollection<MazeEntity>> GetMazesAsync(CancellationToken cancellationToken);
    public Task<MazeEntity> AddMazeAsync(MazeEntity maze, CancellationToken cancellationToken);
    public Task<MazeEntity?> GetMazeByIdAsync(Guid id, CancellationToken cancellationToken);
}
