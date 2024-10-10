namespace rogue
{
    public class MapLayer
    {
        public string name;
        public int[] mapTiles;
        public MapLayer(int mapSize)
        {
            name = "";
            mapTiles = new int[mapSize];
        }
    }
}