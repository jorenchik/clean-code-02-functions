using System;
using System.Collections.Generic;

public class Game
{
    private int x = 0, y = 0;
    private List<string> items = new List<string>();
    private int health = 10;
    private bool gameRunning = true;
    private Random rand = new Random();


    public int showOptions(string command)
    {
        if (command == "command")
        {
            Console.WriteLine("move <direction>, pick <object>, use <object>");
            return 0;
        }
        else if (command == "move")
        {
            Console.WriteLine("north, south, east, west");
            return 0;
        }
        else if (command == "pick")
        {
            Console.WriteLine("You can pick anything!!");
            return 0;
        }
        else if (command == "use")
        {
            if (items.Count > 0)
            {
                Console.WriteLine(String.Join(", ", items));
            }
            else
            {
                Console.WriteLine("There is nothing to use ):");
            }
            return 0;
        }
        return 1;
    }

    public void startGame()
    {
        Console.WriteLine("Game started. Type 'exit' to quit.");
        while (gameRunning)
        {
            Console.Write("Enter command: ");
            string c = Console.ReadLine();
            doCommand(c);
        }
    }

    private void doCommand(string c)
    {
        if (c.ToLower() == "exit")
        {
            gameRunning = false;
            Console.WriteLine("Game exited.");
            return;
        }

        var parts = c.Split(' ');
        var command = parts[0].ToLower();
        var arg = parts.Length > 1 ? parts[1] : "";

        if (command == "move")
        {
            if (arg == "north") y++;
            else if (arg == "south") y--;
            else if (arg == "east") x++;
            else if (arg == "west") x--;
            else if (arg == "?") showOptions("move");
            else Console.WriteLine("move where?");
            Console.WriteLine("Position: " + x + ", " + y);

            if (rand.Next(0, 5) == 2)
            {
                Console.WriteLine("Encountered an enemy!");
                int enemyHealth = 5;
                while (enemyHealth > 0 && health > 0)
                {
                    Console.WriteLine("Fight or Run?");
                    var action = Console.ReadLine().ToLower();
                    if (action == "fight")
                    {
                        Console.WriteLine("You hit the enemy!");
                        enemyHealth -= 2;
                        if (enemyHealth <= 0)
                        {
                            Console.WriteLine("Enemy defeated!");
                            break;
                        }

                        Console.WriteLine("Enemy hits you!");
                        health -= 2;
                        if (health <= 0)
                        {
                            Console.WriteLine("You are defeated!");
                            gameRunning = false; // End game loop on defeat
                            return;
                        }
                    }
                    else if (action == "run")
                    {
                        Console.WriteLine("You ran away!");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid action!");
                    }
                }
            }
        }
        else if (command == "pick")
        {
            if (arg == "?")
            {
                showOptions("pick");
            }
            else if (arg != "")
            {
                items.Add(arg);
                Console.WriteLine("Got " + arg);
            }
            else Console.WriteLine("pick what?");
        }
        else if (command == "use")
        {
            if (arg == "?")
            {
                showOptions("use");
            }
            else if (arg != "")
            {
                if (items.Contains(arg))
                {
                    Console.WriteLine("Using " + arg);
                    items.Remove(arg);
                }
                else Console.WriteLine("Don't have " + arg);
            }
            else Console.WriteLine("use what?");
        }
        else if (command == "?")
        {
            showOptions("command");
        }
        else Console.WriteLine("What?");
    }
}


static class Program
{
    static int Main(string[] args)
    {
        System.Console.WriteLine("Hello world");
        Game game = new Game();
        game.startGame();
        return 0;
    }
}
