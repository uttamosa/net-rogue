﻿namespace rogue
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

                    // Draw the tile graphics
                    Console.SetCursorPosition(x, y);
                    switch (tileId)
                    {
                        case 1:
                            Console.Write("."); // Floor
                            break;
                        case 2:
                            Console.Write("#"); // Wall
                            break;
                        case 3:
                            Console.Write("="); // Wall
                            break;
                        default:
                            Console.Write(" ");
                            break;
                    }
                }
            }

        }
    }
}
