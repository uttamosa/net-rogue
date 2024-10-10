using System.Numerics;
using ZeroElectric.Vinculum;

namespace rogue
{
    internal class enemy
    {
        public string name;       // Vihollisen nimi
        public Vector2 position;  // Missä vihollinen on kentässä
        private Texture graphics; // Viittaus kuvaan jossa vihollisen kuva on
        private int DrawIndex;    // Missä kohdassa kuvaa vihollinen on

        public enemy(string name, Vector2 position, Texture graphics, int drawIndex)
        {
            // this. viittaa olioon itseensä. Eli olion
            // name muuttujan arvoksi tulee parametrin name arvo.
            this.name = name;
            this.position = position;
            this.graphics = graphics;
            this.DrawIndex = drawIndex;
        }

        public void draw()
        {

        }
    }
}
