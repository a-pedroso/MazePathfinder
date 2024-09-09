namespace MazePathfinder.Tests.Integration.Endpoints.Mazes;

using MazePathfinder.Domain.Maze;
using MazePathfinder.Infrastructure.Persistence;
using MazePathfinder.Tests.Integration.Core;

public class WebAppFactoryFixtureGetEndpoints : BaseWebAppFactoryFixture
{
    protected override void PopulateTestData(ApplicationDbContext context)
    {
        var result1 = MazeEntity.CreateEntity("S_________\nXXXXXXXXG_");
        context.Mazes.Add(result1.Value!);

        var result2 = MazeEntity.CreateEntity("S_________\nXXXXXXXXG_");
        context.Mazes.Add(result2.Value!);

        var result3 = MazeEntity.CreateEntity("S_________\nXXXXXXXXG_");
        context.Mazes.Add(result3.Value!);

        context.SaveChanges();
    }
}
