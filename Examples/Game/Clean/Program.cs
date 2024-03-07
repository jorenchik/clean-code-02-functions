using System;
using System.Collections.Generic;

public class Game
{
    private enum Command
    {
        Unknown,
        Exit,
        Move,
        Pick,
        Use,
        Help,
    }

    private enum Action
    {
        Unknown,
        Fight,
        Run,
    }

    private Point point = new Point(0, 0); 
    private List<string> items = new List<string>();
    private int playersHealth = 10;
    private bool isGameRunning = true;
    private Random rand = new Random();
    private int enemyHealth = 5;
    private const int enemyHealthReduction = 2;
    private const int oneOverNEncounterProbability = 5;


    public void StartGame()
    {
        Console.WriteLine("Game started. Type 'exit' to quit.");
        while (isGameRunning)
        {
            Console.Write("Enter command: ");
            string userInput = Console.ReadLine();
            ProcessCommand(userInput.Trim().ToLower());
        }
    }

    private void ProcessCommand(string userInput)
    {
        string[] parts = userInput.Split(' ', 2);
        Command command = ParseCommand(parts[0]);
        string argument = parts.Length > 1 ? parts[1] : "";
        ExecuteCommandWithArgument(command, argument);
    }


    private Command ParseCommand(string command)
    {
        return command switch
        {
            "exit" => Command.Exit,
            "move" => Command.Move,
            "pick" => Command.Pick,
            "use" => Command.Use,
            "?" => Command.Help,
            _ => Command.Unknown,
        };
    }

    private void ExecuteCommandWithArgument(Command command, string argument) {
        switch (command)
        {
            case Command.Exit: ExitGame(); break;
            case Command.Move: TryMovingPlayer(ParseDirection(argument)); break;
            case Command.Pick: PickItem(argument); break;
            case Command.Use: UseItem(argument); break;
            case Command.Help: WriteHelpLines(); break;
            default: WriteUnknownCommandMessage(); break;
        }
    }

    private void ExitGame()
    {
        isGameRunning = false;
        Console.WriteLine("Game exited.");
    }

    private void TryMovingPlayer(Direction direction)
    {
        try
        {
            point.Move(direction);
            Console.WriteLine($"Position: {point.x}, {point.y}");
            RandomEncounter();
        }
        catch (System.ArgumentException)
        {
            Console.WriteLine("Invalid direction. Try 'move <north|south|east|west>'.");
            return;
        }
    }


    private void RandomEncounter()
    {
        if (rand.Next(0, oneOverNEncounterProbability) == 2)
        {
            EncounterEnemy();
        }
    }

    private Direction ParseDirection(string direction)
    {
        return direction switch
        {
            "north" => Direction.North,
            "south" => Direction.South,
            "east" => Direction.East,
            "west" => Direction.West,
            _ => Direction.Unknown,
        };
    }

    private Action ParseAction(string action)
    {
        return action switch
        {
            "fight" => Action.Fight,
            "run" => Action.Run,
            _ => Action.Unknown,
        };
    }

    private void PickItem(string item)
    {
        if (string.IsNullOrEmpty(item))
        {
            Console.WriteLine("Pick what?");
            return;
        }
        items.Add(item);
        Console.WriteLine($"Got {item}");
    }

    private void UseItem(string item)
    {
        if (string.IsNullOrEmpty(item))
        {
            Console.WriteLine("Use what?");
            return;
        }
        if (items.Remove(item))
            Console.WriteLine($"Using {item}");
        else
            Console.WriteLine($"Don't have {item}");
    }

    private void WriteHelpLines()
    {
        Console.WriteLine("Available commands:");
        Console.WriteLine("move <north|south|east|west> - Move in a direction");
        Console.WriteLine("pick <item> - Pick up an item");
        Console.WriteLine("use <item> - Use an item");
        Console.WriteLine("exit - Exit the game");
        Console.WriteLine("? - Show this help message");
    }

    private void WriteUnknownCommandMessage()
    {
        Console.WriteLine("Unknown command. Try '?' for help.");
    }



    private void EncounterEnemy()
    {
        Console.WriteLine("Encountered an enemy!");
        InteractWithEnemyWhileEitherAlive();
    }

    private void InteractWithEnemyWhileEitherAlive()
    {
        while (enemyHealth > 0 && playersHealth > 0)
        {
            Action action = ParseEnemyAction();
            TakeEnemyActionWithHealth(action);
        }

    }

    private Action ParseEnemyAction()
    {
        Console.WriteLine("Fight or Run?");
        Action action = ParseAction(Console.ReadLine().ToLower());
        return action;
    }

    private void TakeEnemyActionWithHealth(Action action)
    {
        switch (action)
        {
            case Action.Fight:
                PerformFightAction();
                break;
            case Action.Run:
                Console.WriteLine("You ran away!");
                return;
            default:
                Console.WriteLine("Invalid action!");
                break;
        }
    }

    private int PerformFightAction()
    {
        Console.WriteLine("You hit the enemy!");
        enemyHealth -= enemyHealthReduction;
        if (enemyHealth > 0)
            HandleEnemyHit();
        else
            Console.WriteLine("Enemy defeated!");
        return enemyHealth;
    }

    private void HandleEnemyHit()
    {
        Console.WriteLine("Enemy hits you!");
        playersHealth -= 2;
        if (playersHealth <= 0)
        {
            Console.WriteLine("You are defeated!");
            isGameRunning = false;
        }
    }

}


public class Point
{
    public int x { get; set; }
    public int y { get; set; }

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.North: y++; break;
            case Direction.South: y--; break;
            case Direction.East: x++; break;
            case Direction.West: x--; break;
            default: throw new ArgumentException("Invalid direction");
        }
    }
}

public enum Direction
{
    Unknown,
    North,
    South,
    East,
    West,
}


class Program
{
    static void Main(string[] args)
    {
        new Game().StartGame();
    }
}
