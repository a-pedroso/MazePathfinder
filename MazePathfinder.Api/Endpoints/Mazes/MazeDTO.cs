namespace MazePathfinder.Api.Endpoints.Mazes;

public record MazeDTO(
    Guid Id, 
    string Map, 
    string? Solution,
    string? UsedAlgorithm);
