using System.Numerics;
using ZeroElectric.Vinculum;

namespace rogue
{
    public class enemy
    {
        public string name;       // Vihollisen nimi
        public int hp;
        public int DrawIndex;    // kuva id

        public enemy(string name, int hp, int drawIndex)
        {
            this.name = name;
            this.hp = hp;
            DrawIndex = drawIndex;
        }

        public override string ToString()
        {
            return $"Enemy: {name} HP:{hp}";
        }
    }
}
