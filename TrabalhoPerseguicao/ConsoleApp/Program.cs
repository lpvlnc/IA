using ConsoleApp;
using System.IO;

bool isRunning = true;
int boardSize = 10;
Random random = new();
List<Character> characters = new()
{
    new Character()
    {
        Type = CharacterType.Player,
        Active = true,
        Alive = true,
        Line = random.Next(boardSize),
        Column = random.Next(boardSize),
        Stamina = 10
    },
    new Character()
    {
        Type = CharacterType.NPC,
        Active = true,
        Alive = true,
        Line = 0,
        Column = 0,
        Stamina = 10
    }
};

Board board = new(characters, 10);

while (isRunning)
{
    if (Console.KeyAvailable)
    {
        var key = Console.ReadKey(true);
        Console.WriteLine(key.KeyChar);
        switch (key.Key)
        {
            case ConsoleKey.UpArrow or ConsoleKey.W:
                board.MovePlayerUp();
                break;
            case ConsoleKey.LeftArrow or ConsoleKey.A:
                board.MovePlayerLeft();
                break;
            case ConsoleKey.DownArrow or ConsoleKey.S:
                board.MovePlayerDown();
                break;
            case ConsoleKey.RightArrow or ConsoleKey.D:
                board.MovePlayerRight();
                break;
        }
    }
    board.MoveNPC();
    Thread.Sleep(1000);
}