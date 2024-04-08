using ZeroElectric.Vinculum;

namespace rogue
{
    internal class Map
    {
        public int mapWidth;
        public int[] mapTiles;

        public void draw()
        {
            Console.ForegroundColor = ConsoleColor.Gray; // Change to map color
            int mapHeight = mapTiles.Length / mapWidth; // Calculate the height: the amount of rows

            for (int y = 0; y < mapHeight; y++) // for each row
            {
                for (int x = 0; x < mapWidth; x++) // for each column in the row
                {
                    int index = x + y * mapWidth; // Calculate index of tile at (x, y)
                    int tileId = mapTiles[index]; // Read the tile value at index

                    Color color = Raylib.GRAY;
                    int pixelX = x * Game.tileSize;
                    int pixelY = y * Game.tileSize;

                    // Draw the tile graphics
                    Console.SetCursorPosition(x, y);
                    switch (tileId)
                    {
                        case 2:
                            Raylib.DrawRectangle(pixelX, pixelY, Game.tileSize, Game.tileSize, color);
                            Raylib.DrawText("#", pixelX, pixelY, Game.tileSize, Raylib.WHITE);
                            break;
                        case 3:
                            Raylib.DrawRectangle(pixelX, pixelY, Game.tileSize, Game.tileSize, color);
                            Raylib.DrawText("=", pixelX, pixelY, Game.tileSize, Raylib.VIOLET);
                            break;
                        default:
                            Raylib.DrawRectangle(pixelX, pixelY, Game.tileSize, Game.tileSize, color);
                            Raylib.DrawText("#", pixelX, pixelY, Game.tileSize, Raylib.GRAY);
                            break;
                    }
                }
            }

        }
    }
}
