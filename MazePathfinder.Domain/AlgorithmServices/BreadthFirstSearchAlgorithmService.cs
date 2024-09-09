namespace MazePathfinder.Domain.AlgorithmServices;

// Maze Solver service using BFS (Breadth-First Search)
// with copilot and chatgpt help
internal class BreadthFirstSearchAlgorithmService : IAlgorithmService
{
    private readonly (int, int)[] directions = { (-1, 0), (1, 0), (0, -1), (0, 1) }; // Left, Right, Up, Down => (x, y)
    private (int, int) start;
    private (int, int) goal;
    private int rows_y;
    private int cols_x;
    private char[,]? maze;

    public string FindPath(string map)
    {
        var lines = map.Split('\n');
        rows_y = lines.Length;
        cols_x = lines[0].Length; // the length of all rows is the same because the entoty domain validates it

        maze = new char[rows_y, cols_x];

        for (int y = 0; y < rows_y; y++)
        {
            for (int x = 0; x < cols_x; x++)
            {
                maze[y, x] = lines[y][x];
                if (maze[y, x] == 'S') start = (x, y);
                if (maze[y, x] == 'G') goal = (x, y);
            }
        }

        var visited = new bool[cols_x, rows_y];
        var queue = new Queue<((int, int) position, List<(int, int)> path)>();
        queue.Enqueue((start, new List<(int, int)> { start }));

        while (queue.Count > 0)
        {
            var (currentPos, path) = queue.Dequeue();
            var (x, y) = currentPos;

            if (visited[x, y]) continue;
            visited[x, y] = true;

            // If we reached the goal
            if (currentPos == goal)
                return ConvertPathToString(path);

            // Explore neighbors
            foreach (var dir in directions)
            {
                int newX = x + dir.Item1;
                int newY = y + dir.Item2;

                if (IsValidMove(newX, newY) && !visited[newX, newY])
                {
                    var newPath = new List<(int, int)>(path) { (newX, newY) };
                    queue.Enqueue(((newX, newY), newPath));
                }
            }
        }

        return null; // No path found
    }

    private bool IsValidMove(int x, int y)
    {
        return x >= 0 && x < cols_x && y >= 0 && y < rows_y && maze![y, x] != 'X';
    }

    private string ConvertPathToString(List<(int, int)> path)
    {
        char[,] resultMaze = (char[,])maze!.Clone();
        foreach (var (x, y) in path)
        {
            if (resultMaze[y, x] == '_')
                resultMaze[y, x] = '*'; // Mark path with '*'
        }

        var result = new List<string>();
        for (int i = 0; i < rows_y; i++)
        {
            var row = new char[cols_x];
            for (int j = 0; j < cols_x; j++)
            {
                row[j] = resultMaze[i, j];
            }
            result.Add(new string(row));
        }

        return string.Join('\n', result);
    }
}
