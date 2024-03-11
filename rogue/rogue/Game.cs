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
            Map level01 = loader.LoadMapFromFile("maps/mapfile");

            // Draw the player
            Console.SetCursorPosition((int)player.position.X, (int)player.position.Y);
            Console.Clear();
            level01.draw();
            Console.Write("@");

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
                int indeksi = ((int)player.position.X + moveX) + ((int)player.position.Y + moveY) * level01.mapWidth;
                int numero = level01.mapTiles[indeksi];

                if (numero == 1) 
                {
                    player.move(moveX, moveY);
                }
                // Move the player

                // -----------Draw:
                // Clear the screen so that player appears only in one place
                Console.Clear();
                level01.draw();
                Console.SetCursorPosition((int)player.position.X, (int)player.position.Y);
                Console.Write("@");
            }
        }
    }
}