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

    public class PlayerCharacter
    {
        public string? nimi;
        public Race rotu;
        public Class hahmoluokka;
        public int taso = 1;

        public Vector2 position;

        public Texture image;
        Color color = Raylib.GREEN;

        int imagePixelX;
        int imagePixelY;

        public void SetImageAndIndex(Texture atlasImage, int imagesPerRow, int index)
        {
            image = atlasImage;
            imagePixelX = (index % imagesPerRow) * Game.tileSize;
            imagePixelY = (index / imagesPerRow) * Game.tileSize;
        }

        public void Draw()
        {
            int pixelX = (int)(position.X * Game.tileSize);
            int pixelY = (int)(position.Y * Game.tileSize);

            Rectangle homma1 = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);
            Rectangle homma2 = new Rectangle(pixelX, pixelY, Game.tileSize, Game.tileSize);
            Vector2 origin = new Vector2(0, 0);

            Raylib.DrawTexturePro(image, homma1, homma2, origin, 0, Raylib.WHITE);
        }
        public void move(int moveX, int moveY)
        {
            position.X += moveX;
            position.Y += moveY;

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
