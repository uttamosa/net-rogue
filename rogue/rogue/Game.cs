using System.Numerics;

namespace rogue
{
    class Game
    {
        public void Run()
        {
            PlayerCharacter player = new PlayerCharacter();
            
            player.position = new Vector2(1, 1);

            while (true)
            {
                Console.WriteLine("mikä nimi");
                string? inputnimi = Console.ReadLine();
                
                if (inputnimi != null)
                {
                    try
                    {
                        string nimi = System.Convert.ToString(inputnimi);
                        player.nimi = nimi;

                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("väärä");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("piti laittaa jotain");
                    continue;
                }

                while (true)
                {
                    Console.WriteLine("mikä rotu?");
                    string? inputrotu = Console.ReadLine();
                    if (inputrotu != null)
                    {
                        if (int.TryParse(inputrotu, out _) != true)
                        {
                            try
                            {
                                Race race = (Race)System.Enum.Parse(typeof(Race), inputrotu);
                                Console.WriteLine(race);
                                break;
                            }
                            catch (ArgumentException)
                            {
                                Console.WriteLine();
                                Console.WriteLine("ei ole sellasta koita uudestaa");
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("täh");
                        }
                    }
                }
                break;
            }

            MapLoader loader = new MapLoader();
            Map level = loader.LoadMapFromFile($"maps/level{player.taso}.json");

            // Draw the player
            Console.SetCursorPosition((int)player.position.X, (int)player.position.Y);
            Console.Clear();
            level.draw();
            Console.Write("@");

            Console.SetCursorPosition(16, 8);
            Console.Write($"taso {player.taso}");

            while (true)
            {
                // ------------Update:
                // Prepare to read movement input
                int moveX = 0;
                int moveY = 0;
                // Wait for keypress and compare value to ConsoleKey enum
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    moveY = -1;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    moveY = 1;
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    moveX = -1;
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    moveX = 1;
                }
                
                //check wall collision
                int indeksi = ((int)player.position.X + moveX) + ((int)player.position.Y + moveY) * level.mapWidth;
                int numero = level.mapTiles[indeksi];

                if (numero == 1) 
                {
                    player.move(moveX, moveY);
                }
                if (numero == 3)
                {
                    player.taso += 1;
                    level = loader.LoadMapFromFile($"maps/level{player.taso}.json");

                    player.position.X = 1;
                    player.position.Y = 1;
                }
                Console.Clear();
                level.draw();
                Console.SetCursorPosition((int)player.position.X, (int)player.position.Y);
                Console.Write("@");

                Console.SetCursorPosition(16, 8);
                Console.Write($"taso {player.taso}");
            }
        }
    }
}