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

    private enum Direction
    {
        Unknown,
        North,
        South,
        East,
        West,
    }

    private enum Action
    {
        Unknown,
        Fight,
        Run,
    }

    private int x = 0, y = 0;
    private List<string> items = new List<string>();
    private int playersHealth = 10;
    private bool isGameRunning = true;
    private Random rand = new Random();
    private const int enemyHealth = 5;

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

        switch (command)
        {
            case Command.Exit: ExitGame(); break;
            case Command.Move: MovePlayer(ParseDirection(argument)); break;
            case Command.Pick: PickItem(argument); break;
            case Command.Use: UseItem(argument); break;
            case Command.Help: WriteHelpLines(); break;
            default: WriteUnknownCommandMessage(); break;
        }
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

    private void MovePlayer(Direction direction)
    {
        bool moved = ChangePositionForMovement(direction);
        if (!moved)
        {
            Console.WriteLine("Invalid direction. Try 'move <north|south|east|west>'.");
            return;
        }
        Console.WriteLine($"Position: {x}, {y}");
        RandomEncounter();
    }

    private bool ChangePositionForMovement(Direction direction)
    {
        bool moved = direction switch
        {
            Direction.North => (++y, true).Item2,
            Direction.South => (--y, true).Item2,
            Direction.East => (++x, true).Item2,
            Direction.West => (--x, true).Item2,
            _ => false,
        };
        return moved;
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
        {
            Console.WriteLine($"Using {item}");
        }
        else
        {
            Console.WriteLine($"Don't have {item}");
        }
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

    private void ExitGame()
    {
        isGameRunning = false;
        Console.WriteLine("Game exited.");
    }

    private void RandomEncounter()
    {
        if (rand.Next(0, 5) == 2)
        {
            EncounterEnemy();
        }
    }

    private void EncounterEnemy()
    {
        Console.WriteLine("Encountered an enemy!");
        InteractWithEnemyWhileEitherAlive(enemyHealth);
    }

    private void InteractWithEnemyWhileEitherAlive(int enemyHealth)
    {
        while (enemyHealth > 0 && playersHealth > 0)
        {
            Action action = ParseEnemyAction();
            TakeEnemyActionWithHealth(action, enemyHealth);
        }

    }

    private Action ParseEnemyAction()
    {
        Console.WriteLine("Fight or Run?");
        Action action = ParseAction(Console.ReadLine().ToLower());
        return action;
    }

    private void TakeEnemyActionWithHealth(Action action, int enemyHealth)
    {
        switch (action)
        {
            case Action.Fight:
                enemyHealth = PerformFightAction(enemyHealth);
                break;
            case Action.Run:
                Console.WriteLine("You ran away!");
                return;
            default:
                Console.WriteLine("Invalid action!");
                break;
        }
    }

    private int PerformFightAction(int enemyHealth)
    {
        Console.WriteLine("You hit the enemy!");
        enemyHealth -= 2;

        if (enemyHealth > 0)
        {
            HandleEnemyHit();
        }
        else
        {
            Console.WriteLine("Enemy defeated!");
        }

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

class Program
{
    static void Main(string[] args)
    {
        new Game().StartGame();
    }
}
