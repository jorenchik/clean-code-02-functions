using System;
using System.Collections.Generic;

public class Game
{
    private int x = 0, y = 0;
    private List<string> items = new List<string>();
    private int health = 10;
    private bool gameRunning = true;
    private Random rand = new Random();

    public void startGame()
    {
        Console.WriteLine("Game started. Type 'exit' to quit.");
        while (gameRunning)
        {
            Console.Write("Enter command: ");
            string c = Console.ReadLine();
            doSomething(c);
        }
    }

    private void doSomething(string c)
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
            if (arg != "")
            {
                items.Add(arg);
                Console.WriteLine("Got " + arg);
            }
            else Console.WriteLine("pick what?");
        }
        else if (command == "use")
        {
            if (arg != "")
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
