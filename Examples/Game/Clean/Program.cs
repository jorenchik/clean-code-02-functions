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
    private int health = 10;
    private bool gameRunning = true;
    private Random rand = new Random();

    public void StartGame()
    {
        Console.WriteLine("Game started. Type 'exit' to quit.");
        while (gameRunning)
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
            case Command.Help: ShowHelp(); break;
            default: ShowUnknownCommandMessage(); break;
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

    private void ShowHelp()
    {
        Console.WriteLine("Available commands:");
        Console.WriteLine("move <north|south|east|west> - Move in a direction");
        Console.WriteLine("pick <item> - Pick up an item");
        Console.WriteLine("use <item> - Use an item");
        Console.WriteLine("exit - Exit the game");
        Console.WriteLine("? - Show this help message");
    }

    private void ShowUnknownCommandMessage()
    {
        Console.WriteLine("Unknown command. Try '?' for help.");
    }

    private void ExitGame()
    {
        gameRunning = false;
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
        int enemyHealth = 5;
        while (enemyHealth > 0 && health > 0)
        {
            Console.WriteLine("Fight or Run?");
            Action action = ParseAction(Console.ReadLine().ToLower());

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
    }

    private int PerformFightAction(int enemyHealth)
    {
        Console.WriteLine("You hit the enemy!");
        enemyHealth -= 2;

        if (enemyHealth > 0)
        {
            Console.WriteLine("Enemy hits you!");
            health -= 2;

            if (health <= 0)
            {
                Console.WriteLine("You are defeated!");
                gameRunning = false;
            }
        }
        else
        {
            Console.WriteLine("Enemy defeated!");
        }

        return enemyHealth;
    }
}

class Program
{
    static void Main(string[] args)
    {
        new Game().StartGame();
    }
}
