using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AldousBroderMaze
{
    public class Maze
    {
        private static int _size = 0;
        private readonly int[,] _maze = new int[_size, _size];
        private readonly bool[,] _visited = new bool[_size, _size];
        private readonly bool[,] _solution = new bool[_size, _size];
        private readonly Random _random = new();
        enum Wall { NONE = 0, TOP = 1, RIGHT = 2, BOTTOM = 4, LEFT = 8, ALL = 15 };

        public Maze(int size)
        {
            _size = size;
            _maze = new int[_size, _size];
            _visited = new bool[_size, _size];
            _solution = new bool[_size, _size];
            AldousBroderMaze();
            ShowMazeValues();
            ShowMaze(-1, -1, false, false);
            bool solved = MazeSolving(0, 0, _size - 1, _size - 1);

            if (solved)
            {
                ShowMaze(-1, -1, false, false, false);
                Write("\n");
                ShowMaze(-1, -1, false, false, true);
            }
            else
                Write("\n::: There's no solution for this maze! \n");
        }

        private void Write(string str)
        {
            Console.Write(str);
        }

        private void ResetMaze(Wall wall)
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _maze[i, j] = (int)wall;
                    _visited[i, j] = false;
                }
            }
        }

        private void PickupRandomCell(out int currentCellLine, out int currentCellColumn)
        {
            currentCellLine = _random.Next(0, _size);
            currentCellColumn = _random.Next(0, _size);
        }

        private void OpenWall(int line, int column, Wall wall)
        {
            // Set a wall as open, doing the same for it's neighbour walls

            if ((wall & Wall.TOP) == Wall.TOP)
            {
                if (line > 0)
                {
                    _maze[line - 1, column] -= (int)Wall.BOTTOM;
                    if (_maze[line - 1, column] < 0)
                        _maze[line - 1, column] = 0;
                }
            }

            if ((wall & Wall.BOTTOM) == Wall.BOTTOM)
            {
                if (line < (_size - 1))
                {
                    _maze[line + 1, column] -= (int)Wall.TOP;
                    if (_maze[line + 1, column] < 0)
                        _maze[line + 1, column] = 0;
                }
            }

            if ((wall & Wall.LEFT) == Wall.LEFT)
            {
                if (column > 0)
                {
                    _maze[line, column - 1] -= (int)Wall.RIGHT;
                    if (_maze[line, column - 1] < 0)
                        _maze[line, column - 1] = 0;
                }
            }

            if ((wall & Wall.RIGHT) == Wall.RIGHT)
            {
                if (column < (_size - 1))
                {
                    _maze[line, column + 1] -= (int)Wall.LEFT;
                    if (_maze[line, column + 1] < 0)
                        _maze[line, column + 1] = 0;
                }
            }

            _maze[line, column] -= (int)wall;
            if (_maze[line, column] < 0)
                _maze[line, column] = 0;
        }

        private void AldousBroderMaze()
        {
            Wall wall = Wall.NONE;
            int unvisitedCells = _size * _size;
            int currentCellLine = -1, currentCellColumn = -1, neighborCellLine = -1, neighborCellColumn = -1;
            int guess = -1;
            int count = 0;

            ResetMaze(Wall.ALL);
            PickupRandomCell(out currentCellLine, out currentCellColumn);

            while (unvisitedCells > 0)
            {
                // pickup a random neighbour from the current cell
                bool found = false;
                while (!found)
                {
                    guess = _random.Next(0, 4);
                    currentCellLine = 0;
                    currentCellColumn = 0;
                    switch (guess)
                    {
                        case 0: // TOP
                            if (currentCellLine > 0)
                            {
                                neighborCellLine = currentCellLine - 1;
                                neighborCellColumn = currentCellColumn;
                                found = true;
                                wall = Wall.TOP;
                            }
                            break;
                        case 1: // DOWN
                            if (currentCellLine < (_size - 1))
                            {
                                neighborCellLine = currentCellLine + 1;
                                neighborCellColumn = currentCellColumn;
                                found = true;
                                wall = Wall.BOTTOM;
                            }
                            break;
                        case 2: // LEFT
                            if (currentCellColumn > 0)
                            {
                                neighborCellLine = currentCellLine;
                                neighborCellColumn = currentCellColumn - 1;
                                found = true;
                                wall = Wall.LEFT;
                            }
                            break;
                        case 3: // RIGHT
                            if (currentCellColumn < (_size - 1))
                            {
                                neighborCellLine = currentCellLine;
                                neighborCellColumn = currentCellColumn + 1;
                                found = true;
                                wall = Wall.RIGHT;
                            }
                            break;
                        default:
                            break;
                    }
                }

                if (!_visited[neighborCellLine, neighborCellColumn])
                {
                    OpenWall(currentCellLine, currentCellColumn, wall);
                    _visited[neighborCellLine, neighborCellColumn] = true;
                    unvisitedCells--;
                }

                ShowMaze(currentCellLine, currentCellColumn);
                Write("\n");
                Write($"count: {++count}\nunvisited cells: {unvisitedCells}\nwall: {(int)wall}\nneighborCellLine: {neighborCellLine}\ncurrentCellLine: {currentCellLine}\nneighborCellColumn: {neighborCellColumn}\ncurrentCellColumn: {currentCellColumn}\n\n");
                currentCellLine = neighborCellLine;
                currentCellColumn = neighborCellColumn;
            }
        }

        private void ShowMazeValues()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    Write($" {_maze[i, j]} ");
                }
                Write("\n");
            }
        }

        private void ShowMaze(int currentCellLine = -1, int currentCellColumn = -1, bool clearScreen = true, bool showDetails = true, bool showingSolution = false)
        {
            if (showDetails && currentCellLine < 0 && currentCellColumn < 0)
            {
                Console.Error.WriteLine("ERROR! It's not possible to show the maze details without the current cell line and current cell column values.");
                throw new Exception();
            }
            if (clearScreen)
                Console.Clear();

            // for each line, we have to print 3 levels (top, bottom and both sides of each cell)
            for (int i = 0; i < _size; i++)
            {
                // ##### TOP #####
                for (int j = 0; j < _size; j++)
                {
                    // top (first line only)
                    if (i == 0)
                    {
                        if ((_maze[i, j] & (int)Wall.TOP) == (int)Wall.TOP)
                            Write("+---");
                        else
                            Console.Write("+   ");
                        if (j == (_size - 1))
                            Write("+");
                    }

                    // top (second line on)
                    if (i > 0)
                    {
                        if ((_maze[i - 1, j] & (int)Wall.BOTTOM) == (int)Wall.BOTTOM || (_maze[i, j] & (int)Wall.TOP) == (int)Wall.TOP)
                            Write("+---");
                        else
                            Write("+   ");
                        if (j == (_size - 1))
                            Write("+");
                    }
                }
                Write("\n");

                // #### BOTH SIDES ####
                for (int j = 0; j < _size; j++)
                {
                    char? left = ' ';
                    char right = ' ';
                    char center = ' ';

                    if (showDetails)
                    {
                        if (!_visited[i, j])
                            center = '*';
                        if (i == currentCellLine && j == currentCellColumn)
                            center = 'C';
                    }

                    if (showingSolution)
                    {
                        if (_solution[i, j])
                            center = '#';
                    }

                    //LEFT and RIGHT for the first column only
                    if (j == 0)
                    {
                        if ((_maze[i, j] & (int)Wall.LEFT) == (int)Wall.LEFT)
                            left = '|';
                        if ((_maze[i, j] & (int)Wall.RIGHT) == (int)Wall.RIGHT)
                            right = '|';
                        Write($"{left} {center} {right}");
                    }

                    // LEFT and RIGHT for the last column only
                    left = null;
                    right = ' ';

                    if (j == (_size - 1))
                    {
                        if ((_maze[i, j] & (int)Wall.LEFT) == (int)Wall.LEFT)
                            left = '|';
                        if ((_maze[i, j] & (int)Wall.RIGHT) == (int)Wall.RIGHT)
                            right = '|';
                        if ((_maze[i, j - 1] & (int)Wall.RIGHT) == (int)Wall.RIGHT)
                            left = null;
                        Write($"{left} {center} {right}");
                    }

                    //LEFT and RIGHT for the intermediate columns only
                    left = ' ';
                    right = ' ';
                    if (j > 0 && j < (_size - 1))
                    {
                        // check the walls between two intermediate columns
                        if ((_maze[i, j] & (int)Wall.RIGHT) == (int)Wall.RIGHT || (_maze[i, j + 1] & (int)Wall.LEFT) == (int)Wall.LEFT)
                            right = '|';
                        Write($" {center} {right}");
                    }
                }
                Write("\n");


                // #### BOTOM - last line only ####
                if (i == (_size - 1))
                {
                    for (int j = 0; j < _size; j++)
                    {
                        if ((_maze[i, j] & (int)Wall.BOTTOM) == (int)Wall.BOTTOM)
                            Write("+---");
                        else
                            Write("+   ");
                        if (j == (_size - 1))
                            Write("+");
                    }
                    Write("\n");
                }
            }
            Thread.Sleep(100);
        }

        private bool IsValidPosition(int line, int column)
        {
            // checks wether the position is within the maze
            return ((line >= 0 && line <= (_size - 1)) && (column >= 0 && column <= (_size - 1)));
        }

        private bool MazeSolving(int lineSource, int columnSource, int lineDest, int columnDest)
        {
            // check if destination was reached
            if (lineSource == lineDest && columnSource == columnDest)
            {
                _solution[lineSource, columnSource] = true;
                return true;
            }

            // is it a valid position?
            if (!IsValidPosition(lineSource, columnSource))
                return false;

            // is it already part of the path?
            if (_solution[lineSource, columnSource])
                return false;

            // marks the current cell as part of the path
            _solution[lineSource, columnSource] = true;
            ShowMaze(-1, -1, true, false, true);
            Write("\n");
            Thread.Sleep(50);

            // tries north, if possible
            if (lineSource > 0)
            {
                // is the top wall open?
                if ((_maze[lineSource, columnSource] & (int)Wall.TOP) != (int)Wall.TOP)
                {
                    if (MazeSolving(lineSource - 1, columnSource, lineDest, columnDest))
                        return true;
                }
            }

            // tries south, if possible
            if (lineSource < (_size - 1))
            {
                // is the bottom wall open?
                if ((_maze[lineSource, columnSource] & (int)Wall.BOTTOM) != (int)Wall.BOTTOM)
                {
                    if (MazeSolving(lineSource + 1, columnSource, lineDest, columnDest))
                        return true;
                }
            }

            // tries east, if possible
            if (columnSource < (_size - 1))
            {
                // is the right wall open?
                if ((_maze[lineSource, columnSource] & (int)Wall.RIGHT) != (int)Wall.RIGHT)
                {
                    if (MazeSolving(lineSource, columnSource + 1, lineDest, columnDest))
                        return true;
                }
            }

            // tries west, if possible
            if (columnSource > 0)
            {
                // is the left wall open?
                if ((_maze[lineSource, columnSource] & (int)Wall.LEFT) != (int)Wall.LEFT)
                {
                    if (MazeSolving(lineSource, columnSource - 1, lineDest, columnDest))
                        return true;
                }
            }

            _solution[lineSource, columnSource] = false;
            return false;
        }
    }
}
