using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Board
    {
        private readonly Tile[,] _board;
        private List<Character> _characters { get; set; } = new List<Character>();
        public Board(List<Character> characters, int boardSize = 0, int blockedTiles = 0)
        {
            _characters = characters;
            if (boardSize == 0)
                boardSize = 10;
            _board = new Tile[boardSize, boardSize];
            ResetBoard();
            if (blockedTiles == 0)
                blockedTiles = boardSize;
            GenerateBlockedTiles(blockedTiles);
            DisplayBoard();
        }

        private void ResetBoard()
        {
            for (int i = 0; i < _board.GetLength(0); i++)
            {
                for (int j = 0; j < _board.GetLength(1); j++)
                {
                    _board[i, j] = new Tile();
                }
            }
        }

        private void GenerateBlockedTiles(int numberOfBlockedTiles)
        {
            Random random = new();
            for (int i = 0; i < numberOfBlockedTiles; i++)
            {
                _board[random.Next(_board.GetLength(0)), random.Next(_board.GetLength(1))].Blocked = true;
            }
        }

        public void DisplayBoard()
        {
            Console.Clear();
            char m1 = BoardCell.FreeCell;
            char m2 = BoardCell.FreeCell;
            char m3 = BoardCell.FreeCell;

            for (int i = 0; i < _board.GetLength(0); i++)
            {
                switch (i)
                {
                    case 0:
                        Console.Write($"{BoardLine.UpperLeftCorner}{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.HorizontalDown}");
                        break;
                    case var value when value == _board.GetLength(0) - 1:
                        Console.Write($"{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.UpperRightCorner}");
                        break;
                    default:
                        Console.Write($"{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.HorizontalDown}");
                        break;
                }
            }
            Console.WriteLine();

            for (int i = 0; i < _board.GetLength(0); i++)
            {
                Console.Write($"{BoardLine.Vertical}");
                for (int j = 0; j < _board.GetLength(0); j++)
                {
                    m1 = BoardCell.FreeCell;
                    m2 = BoardCell.FreeCell;
                    m3 = BoardCell.FreeCell;

                    if (_board[i, j].Blocked)
                    {
                        m1 = BoardCell.BlockedCell;
                        m2 = BoardCell.BlockedCell;
                        m3 = BoardCell.BlockedCell;
                    }

                    // intermediare column separators
                    if (j > 0 && j <= (_board.GetLength(0) - 1))
                    {
                        Console.Write($"{BoardLine.Vertical}");
                    }

                    // is there a character on the current position?
                    for (int k = 0; k < _characters.Count; k++)
                    {
                        if (_characters[k].Line == i && _characters[k].Column == j)
                        {
                            m1 = BoardCell.FreeCell;
                            m2 = (_characters[k].Type == CharacterType.NPC ? BoardCell.NPCCell : BoardCell.PlayerCell);
                            m3 = BoardCell.FreeCell;
                        }
                    }
                    Console.Write($"{m1}{m2}{m3}");
                }
                Console.WriteLine($"{BoardLine.Vertical}");

                // intermediate lines

                if (i < (_board.GetLength(0) - 1))
                {
                    for (int j = 0; j < _board.GetLength(0); j++)
                    {
                        switch (j)
                        {
                            case 0:
                                Console.Write($"{BoardLine.VerticalRight}{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.Horizontal}");
                                break;
                            case var value when value == _board.GetLength(0) - 1:
                                Console.Write($"{BoardLine.HorizontalVertical}{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.VerticalLeft}");
                                break;
                            default:
                                Console.Write($"{BoardLine.HorizontalVertical}{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.Horizontal}");
                                break;
                        }
                    }
                    Console.WriteLine();
                }
            }

            for (int i = 0; i < _board.GetLength(0); i++)
            {
                switch (i)
                {
                    case 0:
                        Console.Write($"{BoardLine.LowerLeftCorner}{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.HorizontalUp}");
                        break;
                    case var value when value == _board.GetLength(0) - 1:
                        Console.Write($"{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.LowerRightCorner}");
                        break;
                    default:
                        Console.Write($"{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.Horizontal}{BoardLine.HorizontalUp}");
                        break;
                }
            }
            Console.WriteLine();
        }

        public void MovePlayerUp()
        {
            if (_characters[0].Line > 0)
            {
                if (!_board[_characters[0].Line - 1, _characters[0].Column].Blocked)
                    _characters[0].Line--;
            }
        }

        public void MovePlayerLeft()
        {
            if (_characters[0].Column > 0)
            {
                if (!_board[_characters[0].Line, _characters[0].Column - 1].Blocked)
                    _characters[0].Column--;
            }
        }

        public void MovePlayerDown()
        {
            if (_characters[0].Line < _board.GetLength(0) - 1)
            {
                if (!_board[_characters[0].Line + 1, _characters[0].Column].Blocked)
                    _characters[0].Line++;
            }
        }

        public void MovePlayerRight()
        {
            if (_characters[0].Column < _board.GetLength(0) - 1)
            {
                if (!_board[_characters[0].Line, _characters[0].Column + 1].Blocked)
                    _characters[0].Column++;
            }
        }

        public void MoveNPC()
        {
            bool moved = false;

            //top 
            if (_characters[1].Line > 0)
                if (_characters[1].Line > _characters[0].Line)
                    if (!_board[_characters[1].Line - 1, _characters[1].Column].Blocked)
                    {
                        _characters[1].Line--;
                        moved = true;
                    }

            // left
            if (_characters[1].Column > 0 && !moved)
                if (_characters[1].Column > _characters[0].Column)
                    if (!_board[_characters[1].Line, _characters[1].Column - 1].Blocked)
                    {
                        _characters[1].Column--;
                        moved = true;
                    }

            // down
            if (_characters[1].Line < _board.GetLength(0) - 1 && !moved)
                if (_characters[1].Line < _characters[0].Line)
                    if (!_board[_characters[1].Line + 1, _characters[1].Column].Blocked)
                    {
                        _characters[1].Line++;
                        moved = true;
                    }

            // right
            if (_characters[1].Column < _board.GetLength(0) - 1 && !moved)
                if (_characters[1].Column < _characters[0].Column)
                    if (!_board[_characters[1].Line, _characters[1].Column + 1].Blocked)
                        _characters[1].Column++;
            DisplayBoard();
        }
    }
}
