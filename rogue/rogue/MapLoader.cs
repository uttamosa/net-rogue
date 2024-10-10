using TurboMapReader;

namespace rogue
{
    internal class MapLoader
    {
        public static Map CreateMapFromFile(TiledMap map)
        {
            TurboMapReader.MapLayer groundlayer = map.GetLayerByName("ground");
            TurboMapReader.MapLayer enemyLayer = map.GetLayerByName("enemies");
            TurboMapReader.MapLayer itemLayer = map.GetLayerByName("items");

            Map roguemap = new Map();

            int levelwidth = groundlayer.width;
            roguemap.mapWidth = levelwidth;
            int howManyTiles = groundlayer.data.Length;
            int[] groundTiles = groundlayer.data;

            MapLayer myGroundLayer = new MapLayer(howManyTiles);
            myGroundLayer.name = "ground";
            myGroundLayer.mapTiles = groundTiles;

            int[] enemyTiles = enemyLayer.data;

            MapLayer myEnemyLayer = new MapLayer(howManyTiles);
            myEnemyLayer.name = "enemies";
            myEnemyLayer.mapTiles = enemyTiles;

            int[] itemTiles = itemLayer.data;

            MapLayer myItemLayer = new MapLayer(howManyTiles);
            myItemLayer.name = "items";
            myItemLayer.mapTiles = itemTiles;

            roguemap.layers[0] = myGroundLayer;
            roguemap.layers[1] = myEnemyLayer;
            roguemap.layers[2] = myItemLayer;

            return roguemap;
        }

        public Map LoadMapFromFile(string filename)
        {
            TiledMap loadedTileMap = MapReader.LoadMapFromFile(filename);
        
            Map loadedmap = CreateMapFromFile(loadedTileMap);
        
            return loadedmap;
        }
    }
}