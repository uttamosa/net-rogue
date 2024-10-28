using System.Numerics;
using ZeroElectric.Vinculum;

namespace rogue
{
    internal class enemy
    {
        public string name;       // Vihollisen nimi
        public Vector2 position;  // Missä vihollinen on kentässä
        public Texture graphics; // Viittaus kuvaan jossa vihollisen kuva on
        public int DrawIndex;    // Missä kohdassa kuvaa vihollinen on

        public enemy(string name, Vector2 position, Texture graphics, int drawIndex)
        {
            this.name = name;
            this.position = position;
            this.graphics = graphics;
            DrawIndex = drawIndex;
        }

    }
}
