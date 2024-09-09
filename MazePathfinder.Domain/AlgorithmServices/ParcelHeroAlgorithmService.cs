namespace MazePathfinder.Domain.AlgorithmServices;

using System;
using System.Text;

/*
1. Start from the starting point and define an empty path
2. If it's the goal, stop and return the path provided
3. Find possible moves based on the constraints above
4. Remove from the list of possible moves any cell that you have already travelled on
5. If there are no remaining possible moves, return a value to indicate that the provided path can't reach a solution
6. Repeat the previous steps starting from point 2, for each remaining possible move. For each move, the path should include that move
 */

internal class ParcelHeroAlgorithmService : IAlgorithmService
{
    public string? FindPath(string map)
    {
        var lines = map.Split('\n');
        int x_length = lines[0].Length; // the length of all rows is the same because the entoty domain validates it
        int y_lenth = lines.Length;
        char[,] maze = new char[y_lenth, x_length];
        bool[,] travelled = new bool[x_length, y_lenth];
        (int, int) start = (0, 0);
        (int, int) goal = (0, 0);

        for (int x = 0; x < x_length; x++)    // x
        {
            for (int y = 0; y < y_lenth; y++) // y
            {
                char cell = lines[y][x];

                if (cell == 'S')
                {
                    start = (x, y);
                }

                if (cell == 'G')
                {
                    goal = (x, y);
                }

                maze[y, x] = cell;
            }
        }

        List<(int, int)> path = [];
        if (FindPathRecursive(start, maze, travelled, start, goal, path))
        {
            return ConvertPathToString(path, maze, x_length, y_lenth);
        }

        return null;
    }

    private static bool FindPathRecursive((int x, int y) start, char[,] maze, bool[,] travelled, (int x, int y) current, (int x, int y) goal, List<(int, int)> path)
    {
        int x = current.x;
        int y = current.y;

        // If out of bounds or on a wall or already travelled, return false
        if (x < 0 || y < 0 || y >= maze.GetLength(0) || x >= maze.GetLength(1) || maze[y, x] == 'X' || travelled[x, y])
        {
            return false;
        }

        // Mark the current cell as travelled
        travelled[x, y] = true;

        // Add the current cell to the path
        path.Add((x, y));

        // If it's the goal, return true
        if (current == goal)
        {
            return true;
        }

        // Explore all possible moves (right, down, left, up)
        var moves = new (int x, int y)[]
        {
            (x + 1, y), // right
            (x, y + 1), // down
            (x - 1, y), // left
            (x, y - 1)  // up
        };

        foreach (var move in moves)
        {
            if (FindPathRecursive(start, maze, travelled, move, goal, path))
            {
                return true;
            }
        }

        path.Clear();
        return FindPathRecursive(start, maze, travelled, start, goal, path);

        // Backtrack: unmark the current cell and remove it from the path
        //travelled[y, x] = false;
        //path.RemoveAt(path.Count - 1);

        //return false;
    }

    private static string ConvertPathToString(List<(int, int)> path, char[,] maze, int x_length, int y_lenth)
    {
        char[,] resultMaze = (char[,])maze!.Clone();
        foreach (var (x, y) in path)
        {
            if (resultMaze[y, x] == '_')
                resultMaze[y, x] = '*'; // Mark path with '*'
        }

        var result = new List<string>();
        for (int y = 0; y < y_lenth; y++)
        {
            var row = new char[x_length];
            for (int x = 0; x < x_length; x++)
            {
                row[x] = resultMaze[y, x];
            }
            result.Add(new string(row));
        }

        return string.Join('\n', result);
    }
}
