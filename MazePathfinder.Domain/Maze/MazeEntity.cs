namespace MazePathfinder.Domain.Maze;

using FluentValidation;
using MazePathfinder.Domain.AlgorithmServices;
using MazePathfinder.Domain.Common;

public class MazeEntity
{
    private MazeEntity(
        Guid id,
        string map)
    {
        Id = id;
        Map = map;
    }

    public Guid Id { get; }
    public string Map { get; }
    public string? Solution { get; private set; }
    public AlgorithmsEnum? UsedAlgorithm { get; private set; }

    public static Result<MazeEntity> CreateEntity(string map)
    {
        MazeEntity maze = new(Guid.NewGuid(), map);

        MazeEntityValidator validator = new();
        var validationResult = validator.Validate(maze);

        Result<MazeEntity> result = validationResult.IsValid
            ? Result<MazeEntity>.Success(maze)
            : Result<MazeEntity>.Failure(validationResult.Errors.Select(s => s.ErrorMessage).ToArray());

        return result;
    }

    public bool SolveMaze(AlgorithmsEnum algorithm)
    {
        UsedAlgorithm = algorithm;
        IAlgorithmService algorithmService = AlgorithmServiceFactory.GetAlgorithmService(algorithm);
        Solution = algorithmService.FindPath(Map);

        if (string.IsNullOrEmpty(Solution))
        {
            Solution = "No solution found";
            return false;
        }

        return true;
    }
}

public class MazeEntityValidator : AbstractValidator<MazeEntity>
{
    public MazeEntityValidator()
    {
        RuleFor(x => x.Map)
            .NotEmpty()
                .WithMessage("{PropertyName} is required.")
            .DependentRules(() =>
            {
                RuleFor(r => r.Map)
                    .Must(ValidateMap)
                        .WithMessage("{PropertyName} must be at max 20x20 and at least 2x2 and only with chars 'S' (only once), 'G' (only once), '_' and 'X'");
            });
    }

    private bool ValidateMap(string map)
    {
        var lines = map.Split('\n');
        int x_lenth = lines[0].Length;

        // validate number of rows 
        if (lines.Length < 2 || lines.Length > 20)
        {
            return false;
        }

        // validate the length of each row
        foreach (string line in lines)
        {
            if (line.Length < 2 || line.Length > 20 || x_lenth != line.Length)
            {
                return false;
            }
        }

        return ValidateMapContent(map);

        static bool ValidateMapContent(string str)
        {
            int sCount = 0;
            int gCount = 0;

            // Allowed characters in the maze
            List<char> validChars = ['S', 'G', '_', 'X'];

            foreach (char c in str)
            {
                // Ignore newlines
                if (c == '\n')
                    continue;

                // Check if the character is valid
                if (!validChars.Contains(c))
                    return false;

                // Count occurrences of 'S' and 'G'
                if (c == 'S') sCount++;
                if (c == 'G') gCount++;
            }

            // Ensure 'S' and 'G' appear exactly once
            return sCount == 1 && gCount == 1;
        }
    }
}