using System.Numerics;
using ZeroElectric.Vinculum;
using TurboMapReader;

namespace rogue
{

    class Game
    {
        enum GameState
        {
            MainMenu,
            GameLoop
        }

        GameState currentGameState;

        public PlayerCharacter player;
        public Map level;
        public MapLoader loader = new MapLoader();
        public static readonly int tileSize = 16;

        int screen_width = 1280;
        int screen_height = 720;

        int game_width;
        int game_height;
        RenderTexture game_screen;

        public static Texture Image;

        private void Init()
        {
            MapLoader loader = new MapLoader();
            currentGameState = GameState.MainMenu;
            player = CreateCharacter();
            player.position = new Vector2(1, 1);
            level = loader.LoadMapFromFile($"maps/level{player.taso}.tmj");

            Raylib.InitWindow(screen_width, screen_height, "rogue");
            Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
            DrawMainMenu();

            Image = Raylib.LoadTexture("images/colored-transparent_packed.png");

            level.LoadEnemiesAndItems(Image);

            game_width = 440;
            game_height = 440;

            game_screen = Raylib.LoadRenderTexture(game_width, game_height);

            Raylib.SetTextureFilter(game_screen.texture, TextureFilter.TEXTURE_FILTER_POINT);
            Raylib.SetWindowMinSize(game_width, game_height);
            Raylib.SetTargetFPS(30);
            player.SetImageAndIndex(Image, 49, 25);
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

        private void DrawGameToTexture()
        {
            // Kaikki piirtäminen tehdään tekstuuriin eikä suoraan ruudulle
            Raylib.BeginTextureMode(game_screen);
            // Kaikki pelin piirtäminen tapahtuu tässä välissä
            DrawGame();

            Raylib.EndTextureMode();

            DrawGameScaled();
        }
        public void DrawMainMenu()
        {
            // Tyhjennä ruutu ja aloita piirtäminen
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);

            // Laske ylimmän napin paikka ruudulla.
            int button_width = 100;
            int button_height = 20;
            int button_x = Raylib.GetScreenWidth() / 2 - button_width / 2;
            int button_y = Raylib.GetScreenHeight() / 2 - button_height / 2;

            // Piirrä pelin nimi nappien yläpuolelle
            RayGui.GuiLabel(new Rectangle(button_x, button_y - button_height * 2, button_width, button_height), "Rogue");
            
            Raylib.EndDrawing();

            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Start Game") == 1)
            {
                currentGameState = GameState.GameLoop;
            }
        }
        private void DrawGameScaled()
        {

            // Tässä piirretään tekstuuri ruudulle
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.DARKGRAY);

            int draw_width = Raylib.GetScreenWidth();
            int draw_height = Raylib.GetScreenHeight();
            float scale = Math.Min((float)draw_width / game_width, (float)draw_height / game_height);

            // Note: when drawing on texture, the Y-axis is
            //flipped, need to multiply height by -1
            Rectangle source = new Rectangle(0.0f, 0.0f,
                game_screen.texture.width,
                game_screen.texture.height * -1.0f);

            Rectangle destination = new Rectangle(
                (draw_width - (float)game_width * scale) * 0.5f,
                (draw_height - (float)game_height * scale) * 0.5f,
                game_width * scale,
                game_height * scale);

            Raylib.DrawTexturePro(game_screen.texture,
                source, destination,
                new Vector2(0, 0), 0.0f, Raylib.WHITE);

            Raylib.EndDrawing();
        }
        private void DrawGame()
        {
            level.draw();
            player.Draw();
        }
        private void UpdateGame()
        {
            int moveX = 0;
            int moveY = 0;

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
            {
                moveY = -1;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
            {
                moveY = 1;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
            {
                moveX = -1;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
            {
                moveX = 1;
            }

            int playernewx = (int)player.position.X + moveX;
            int playernewy = (int)player.position.Y + moveY;
            

            MapTile maptileindex = level.GetTileAt(playernewx, playernewy);
            if (maptileindex == MapTile.Floor)
            {
                player.move(moveX, moveY);
            }
        }
        private void gameloop()
        {
            while (true)
            {
                while (Raylib.WindowShouldClose() == false)
                {
                    switch (currentGameState)
                    {
                        case GameState.MainMenu:
                            DrawMainMenu();
                            break;

                        case GameState.GameLoop:
                            UpdateGame();
                            DrawGameToTexture();
                            break;
                    }
                }
            }
        }
    }
}