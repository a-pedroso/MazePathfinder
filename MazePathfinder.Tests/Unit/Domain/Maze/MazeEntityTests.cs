namespace MazePathfinder.Tests.Unit.Domain.Maze;

using MazePathfinder.Domain.AlgorithmServices;
using MazePathfinder.Domain.Common;
using MazePathfinder.Domain.Maze;

public class MazeEntityTests
{
    [Fact]
    public void MazeEntity_CreateReturnSucessResult_WhenCalledWithValidParameters()
    {
        // Arrange
        string sourceMap = "S__\n_X_\n_G_";
        // Act
        Result<MazeEntity> result = MazeEntity.CreateEntity(sourceMap);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(result.Value.Map, sourceMap);
    }

    [Fact]
    public void MazeEntity_CreateReturnFailureResult_WhenCalledWithInvalidParameters()
    {
        // Arrange
        string sourceMap = "S__\n_X_\n";
        // Act
        Result<MazeEntity> result = MazeEntity.CreateEntity(sourceMap);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
    }

    [Theory]
    [InlineData("S_________\n_XXXXXXXX_\n_X______X_\n_X_XXXX_X_\n_X_X__X_X_\n_X_X__X_X_\n_X_X____X_\n_X_XXXXXX_\n_X________\nXXXXXXXXG_", AlgorithmsEnum.BreadthFirstSearch)]
    [InlineData("S_________\n_XXXXXXXX_\n_X______X_\n_X_XXXX_X_\n_X_X__X_X_\n_X_X__X_X_\n_X_X____X_\n_X_XXXXXX_\n_X________\nXXXXXXXXG_", AlgorithmsEnum.ParcelHero)]
    [InlineData("S__\n_X_\n_G_", AlgorithmsEnum.BreadthFirstSearch)]
    [InlineData("S__\n_X_\n_G_", AlgorithmsEnum.ParcelHero)]
    [InlineData("__________\n_XXXXXXXX_\nSX______X_\n_X_XXXXXX_\n_X________\nXXXXXXXX_G", AlgorithmsEnum.BreadthFirstSearch)]
    [InlineData("__________\n_XXXXXXXX_\nSX______X_\n_X_XXXXXX_\n_X________\nXXXXXXXX_G", AlgorithmsEnum.ParcelHero)]
    public void MazeEntity_SolveMazeReturnTrue_WhenCalledWithValidParameters(string map, AlgorithmsEnum algorithm)
    {
        // Arrange
        MazeEntity maze = MazeEntity.CreateEntity(map).Value;
        
        // Act
        bool result = maze.SolveMaze(algorithm);

        // Assert
        Assert.True(result);
        Assert.NotNull(maze.Solution);
        Assert.NotEmpty(maze.Solution);
    }
}
