# Maze Path Finder API

## run application

to run the api, either use IDE to start using https or run at ./MazePathFinder.Api level

``` bash
dotnet run
```

## run tests

to run tests, either use IDE test explorer or navigate to ./MazePathFinder.Tests and run

``` bash
dotnet test
```

## API access

After running the application, you can access the API at https://localhost:7116/swagger/index.html

You can also use the MazePathfinder.Api.http file to test the API using Visual Studio Code REST Client extension


## Submit new Maze

To submit a new maze, use the following endpoint

``` bash
POST https://localhost:7116/mazes

{
  "map": "S_________\n_XXXXXXXX_\n_X______X_\n_X_XXXX_X_\n_X_X__X_X_\n_X_X__X_X_\n_X_X____X_\n_X_XXXXXX_\n_X________\nXXXXXXXXG_",
  "algorithm": 1
}
```

formatted map: 

```
S_________
_XXXXXXXX_
_X______X_
_X_XXXX_X_
_X_X__X_X_
_X_X__X_X_
_X_X____X_
_X_XXXXXX_
_X________
XXXXXXXXG_
```

The map is a string with the following characters:
- S: Start
- G: Goal
- _: Empty space
- X: Wall


the response will be the maze with the path to the exit marked with '*'

``` json
{
  "id": "8d3b8ed6-46bd-4676-9e17-8a8be740bd77",
  "map": "S_________\n_XXXXXXXX_\n_X______X_\n_X_XXXX_X_\n_X_X__X_X_\n_X_X__X_X_\n_X_X____X_\n_X_XXXXXX_\n_X________\nXXXXXXXXG_",
  "solution": "S*********\n_XXXXXXXX*\n_X______X*\n_X_XXXX_X*\n_X_X__X_X*\n_X_X__X_X*\n_X_X____X*\n_X_XXXXXX*\n_X_______*\nXXXXXXXXG*",
  "usedAlgorithm": "BreadthFirstSearch"
}
```

formatted solution:

```
S*********
_XXXXXXXX*
_X______X*
_X_XXXX_X*
_X_X__X_X*
_X_X__X_X*
_X_X____X*
_X_XXXXXX*
_X_______*
XXXXXXXXG*
```

In case there's no solution, the response will be:

``` json
{
  "id": "7c136afa-e756-4cc5-9393-72a73e7a209f",
  "map": "S_________\n_XXXXXXXXX\n_X______X_\n_X_XXXX_X_\n_X_X__X_X_\n_X_X__X_X_\n_X_X____X_\n_X_XXXXXX_\n_X________\nXXXXXXXXG_",
  "solution": "No solution found",
  "usedAlgorithm": "BreadthFirstSearch"
}
```

It does not responds with a http error code, as the request was successful, but the maze has no solution

## Assumptions

- The maze is at least a 2x2 and a maximum 20x20
- The maze has only one start and one goal
