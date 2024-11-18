using System.Numerics;
using ZeroElectric.Vinculum;
using TurboMapReader;
using RayGuiCreator;

namespace rogue
{

    class Game
    {
        enum GameState
        {
            MainMenu,
            GameLoop,
            CharacterCreator,
            Settings,
            PauseMenu
        }

        public enum Difficulty
        {
            Easy,
            Medium,
            Catastrofic
        }

        GameState currentGameState;
        Difficulty currentDifficulty;
        public float volume;

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
            Raylib.SetExitKey(0);
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

        private PlayerCharacter CreateCharacter()
        {
            PlayerCharacter player = new PlayerCharacter();

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
            Raylib.ClearBackground(Raylib.DARKGRAY); //tekee jotain ja peli ei ilman tätä clearaa vanhaa kuvaa
            DrawGame();

            Raylib.EndTextureMode();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.DARKGRAY);

            DrawGameScaled();

            Raylib.EndDrawing();
        }

        public void DrawMainMenu()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);

            int menuStartX = 500;
            int menuStartY = 0;
            int rowHeight = Raylib.GetScreenHeight() / 30;
            int menuWidth = Raylib.GetScreenWidth() / 4;

            MenuCreator creator = new MenuCreator(menuStartX, menuStartY, rowHeight, menuWidth);

            creator.Label("Main Menu");

            if (creator.Button("Start"))
            {
                currentGameState = GameState.CharacterCreator;
            }

            if (creator.Button("Quit"))
            {
                Raylib.CloseWindow();
                Environment.Exit(1);
            }

            Raylib.EndDrawing();
        }

        public void DrawCharacterCreationMenu()
        {
            // List of possible difficulty choices. The indexing starts at 0
            MultipleChoiceEntry difficultyDropDown = new MultipleChoiceEntry(
                new string[] { "Easy", "Medium", "Catastrophic" });

            // List of possible class choices.
            MultipleChoiceEntry RaceChoices = new MultipleChoiceEntry(
                new string[] { "Human", "Dog", "Cat", "Woman", "Turtle" });

            // Volume value is modified by the volume slider
            float volume = 1.0f;

            // Textbox data for player's name
            TextBoxEntry playerNameEntry = new TextBoxEntry(15);

            while (true)
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib.BLACK);

                int width = Raylib.GetScreenWidth() / 2;

                // Fit 22 rows on the screen
                int rows = 30;
                int rowHeight = Raylib.GetScreenHeight() / rows;

                // Center the menu horizontally
                int x = (Raylib.GetScreenWidth() / 2) - (width / 2);

                // Center the menu vertically
                int y = (Raylib.GetScreenHeight() - (rowHeight * rows)) / 2;

                // 3 pixels between rows, text 3 pixels smaller than row height
                MenuCreator c = new MenuCreator(x, y, rowHeight, width, 3, -3);
                c.Label("Main menu");

                c.Label("Player name");
                c.TextBox(playerNameEntry);

                c.Label("Character race");
                c.DropDown(RaceChoices);

                c.Label("Volume");
                c.Slider("quiet", "LOUD", ref volume, 0.0f, 1.0f);

                c.Label("Difficulty toggle");
                c.DropDown(difficultyDropDown);


                if (c.Button("Finish"))
                {
                    player.nimi = playerNameEntry.ToString();
                    player.rotu = (Race)Enum.Parse(typeof(Race), RaceChoices.GetEntryAt(0));
                    currentDifficulty = (Difficulty)Enum.Parse(typeof(Difficulty), difficultyDropDown.GetEntryAt(0));

                    currentGameState = GameState.GameLoop;
                    break;
                }
                // Draws open dropdowns over other menu items
                int menuHeight = c.EndMenu();

                // Draws a rectangle around the menu
                int padding = 2;
                Raylib.DrawRectangleLines(
                    x - padding,
                    y - padding,
                    width + padding * 2,
                    menuHeight + padding * 2,
                    MenuCreator.GetLineColor());
                Raylib.EndDrawing();
            }
        }

        public void DrawSettings()
        {
            while (true)
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib.BLACK);

                int width = Raylib.GetScreenWidth() / 2;
                int rows = 30;
                int rowHeight = Raylib.GetScreenHeight() / rows;
                int x = (Raylib.GetScreenWidth() / 2) - (width / 2);
                int y = (Raylib.GetScreenHeight() - (rowHeight * rows)) / 2;

                MenuCreator c = new MenuCreator(x, y, rowHeight, width, 3, -3);

                c.Label("Volume");
                c.Slider("quiet", "LOUD", ref volume, 0.0f, 1.0f);

                if (c.Button("Finish"))
                {
                    Console.WriteLine(volume);
                    currentGameState = GameState.PauseMenu;
                    break;
                }

                int menuHeight = c.EndMenu();

                int padding = 2;
                Raylib.DrawRectangleLines(
                    x - padding,
                    y - padding,
                    width + padding * 2,
                    menuHeight + padding * 2,
                    MenuCreator.GetLineColor());
                Raylib.EndDrawing();
            }
        }

        public void DrawPauseMenu()
        {
            while (true)
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib.BLACK);

                int width = Raylib.GetScreenWidth() / 2;
                int rows = 30;
                int rowHeight = Raylib.GetScreenHeight() / rows;
                int x = (Raylib.GetScreenWidth() / 2) - (width / 2);
                int y = (Raylib.GetScreenHeight() - (rowHeight * rows)) / 2;

                MenuCreator c = new MenuCreator(x, y, rowHeight, width, 3, -3);

                if (c.Button("Continue"))
                {
                    currentGameState = GameState.GameLoop;
                    break;
                }

                if (c.Button("Settings"))
                {
                    currentGameState = GameState.Settings;
                    break;
                }

                c.Label("");

                if (c.Button("Quit"))
                {
                    currentGameState = GameState.MainMenu;
                    break;
                }

                int menuHeight = c.EndMenu();

                int padding = 5;
                Raylib.DrawRectangleLines(
                    x - padding,
                    y - padding,
                    width + padding * 2,
                    menuHeight + padding * 2,
                    MenuCreator.GetLineColor());
                Raylib.EndDrawing();
            }
        }

        private void DrawGameScaled()
        {
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

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_W))
            {
                moveY = -1;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_S))
            {
                moveY = 1;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_A))
            {
                moveX = -1;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_D))
            {
                moveX = 1;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
            {
                currentGameState = GameState.PauseMenu;
            }

            int playernewx = (int)player.position.X + moveX;
            int playernewy = (int)player.position.Y + moveY;
            

            MapTile maptileindex = level.GetTileAt(playernewx, playernewy);
            MapTile mapenemyindex = level.GetEnemyAt(playernewx, playernewy);
            MapTile mapitemindex = level.GetItemAt(playernewx, playernewy);
            
            if (maptileindex == MapTile.Floor)
            {
                player.move(moveX, moveY);
            }
            if (mapenemyindex == MapTile.goblin)
            {
                Console.WriteLine("osuit vihuun");
            }
            if (mapitemindex == MapTile.potion)
            {
                Console.WriteLine("osuit potioniin");
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
                            DrawGameToTexture();
                            UpdateGame();
                            break;

                        case GameState.CharacterCreator:
                            DrawCharacterCreationMenu();
                            break;

                        case GameState.Settings:
                            DrawSettings();
                            break;

                        case GameState.PauseMenu:
                            DrawPauseMenu();
                            break;
                    }
                }
                if (Raylib.WindowShouldClose() == true)
                {
                    Raylib.CloseWindow();
                }
            }
        }
    }
}