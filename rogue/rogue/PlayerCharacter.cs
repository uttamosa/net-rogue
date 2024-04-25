using System.Numerics;
using ZeroElectric.Vinculum;

namespace rogue
{
    public enum Race
    {
        human,
        dog,
        cat,
        woman
    }

    public enum Class 
    { 

    }

    internal class PlayerCharacter
    {
        public string? nimi;
        public Race rotu;
        public Class hahmoluokka;
        public int taso = 1;

        public Vector2 position;

        Texture image;
        Color color = Raylib.GREEN;

        int imagePixelX;
        int imagePixelY;

        public void SetImageAndIndex(Texture atlasImage, int imagesPerRow, int index)
        {
            image = atlasImage;
            imagePixelX = (index % imagesPerRow) * Game.tileSize;
            imagePixelY = (int)(index / imagesPerRow) * Game.tileSize;
        }

        public void Draw()
        {
            int pixelX = (int)(position.X * Game.tileSize);
            int pixelY = (int)(position.Y * Game.tileSize);

            Raylib.dra
            Raylib.DrawText("@", pixelX, pixelY, Game.tileSize, Raylib.BLACK);
        }
        public void move(int moveX, int moveY)
        {
            position.X += moveX;
            position.Y += moveY;
            // Prevent player from going outside screen
            if (position.X < 0)
            {
                position.X = 0;
            }
            else if (position.X > Console.WindowWidth - 1)
            {
                position.X = Console.WindowWidth - 1;
            }
            if (position.Y < 0)
            {
                position.Y = 0;
            }
            else if (position.Y > Console.WindowHeight - 1)
            {
                position.Y = Console.WindowHeight - 1;
            }
        }
    }
}
