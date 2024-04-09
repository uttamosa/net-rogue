using System.Data;
using System.Numerics;
using ZeroElectric.Vinculum;

namespace rogue
{
    class Game
    {
        public PlayerCharacter player;
        public Map level;
        public MapLoader loader = new MapLoader();
        public static readonly int tileSize = 32;

        int screen_width = 1280;
        int screen_height = 720;

        int game_width;
        int game_height;
        RenderTexture game_screen;

        private void Init()
        {
            player = CreateCharacter();
            player.position = new Vector2(1, 1);
            level = loader.LoadMapFromFile($"maps/level{player.taso}.json");

            Raylib.InitWindow(screen_width, screen_height, "rogue");
            Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);

            game_width = 480;
            game_height = 270;

            game_screen = Raylib.LoadRenderTexture(game_width, game_height);

            Raylib.SetTextureFilter(game_screen.texture, TextureFilter.TEXTURE_FILTER_BILINEAR);
            Raylib.SetWindowMinSize(game_width, game_height);
            Raylib.SetTargetFPS(30);
        }

        private string AskName()
        {
            while (true)
            {
                Console.WriteLine("mikä nimi");
                string? inputnimi = Console.ReadLine();

                if (inputnimi != null)
                {
                    try
                    {
                        string nimi = System.Convert.ToString(inputnimi);
                        return nimi;

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
            }
        }
        private Race AskSpecies()
        {
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
                            return race;
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
        }

        //private Role AskRole() {}

        private PlayerCharacter CreateCharacter()
        {
            PlayerCharacter player = new PlayerCharacter();
            player.nimi = AskName();
            player.rotu = AskSpecies();
            //player.role = AskRole();
            return player;
        }

        public void Run()
        {
            Init();
            gameloop();
            Raylib.CloseWindow();
            Raylib.UnloadRenderTexture(game_screen);
        }
        private void DrawGame()
        {
            Raylib.BeginDrawing();
            Console.Clear();
            level.draw();
            player.Draw();
            Raylib.EndDrawing();
        }

        private void UpdateGame()
        {
            if (Console.KeyAvailable == false)
            {
                // No input: Sleep for 33 ms and return.
                System.Threading.Thread.Sleep(33);
                return;
            }

            int moveX = 0;
            int moveY = 0;

            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    moveY = -1; break;

                case ConsoleKey.DownArrow:
                    moveY = 1; break;

                case ConsoleKey.LeftArrow:
                    moveX = -1; break;

                case ConsoleKey.RightArrow:
                    moveX = 1; break;
            }

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
        }

        private void gameloop()
        {
            while (true)
            {
                DrawGame();
                UpdateGame();
            }
        }
    }
}