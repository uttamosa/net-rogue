using Newtonsoft.Json;
using System;
using System.Numerics;
using ZeroElectric.Vinculum;

namespace rogue
{
    public enum MapTile : int
    {
        Floor = 0,
        Wall = 638,
        goblin = 128,
        potion = 579,
        potion2 = 579
    }

    public class Map
    {
        List<enemy> enemies;
        List<item> items;

        public int mapWidth;
        public int mapHeight;
        public MapLayer[] layers;

        public static List<int> WallTileNumbers = new List<int> { 638 };
        public static List<int> EnemyTileNumbers = new List<int> { 128 };
        public static List<int> ItemTileNumbers = new List<int> { 579 };

        public Map()
        {
            mapWidth = 1;
            mapHeight = 1;
            layers = new MapLayer[3];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new MapLayer(mapWidth * mapHeight);
            }
            enemies = new List<enemy>() { };
            items = new List<item>() { };
        }
        //???

        public string GetEnemyName(int spriteIndex)
        {
            switch (spriteIndex)
            {
                case 108: return "Ghost";
                case 109: return "Cyclops";
                default: return "Unknown";
            }
        }

        public MapTile GetTileAt(int x, int y)
        {
            // Calculate index: index = x + y * mapWidth
            int indexInMap = x + y * mapWidth;

            // Use the index to get a map tile from map's array
            MapLayer groundLayer = GetLayer("ground");
            int[] mapTiles = groundLayer.mapTiles;
            int tileId = mapTiles[indexInMap];

            if (WallTileNumbers.Contains(tileId))
            {
                return MapTile.Wall;
            }
            else
            {
                return MapTile.Floor;
            }
        }

        public MapTile GetEnemyAt(int x, int y)
        {
            int indexInMap = x + y * mapWidth;

            MapLayer enemyLayer = GetLayer("enemies");
            int[] mapTiles = enemyLayer.mapTiles;
            int tileId = mapTiles[indexInMap];
            if (EnemyTileNumbers.Contains(tileId))
            {
                return MapTile.goblin;
            }
            else
            {
                return MapTile.Floor;
            }
        }

        public MapTile GetItemAt(int x, int y)
        {
            int indexInMap = x + y * mapWidth;

            MapLayer itemLayer = GetLayer("items");
            int[] mapTiles = itemLayer.mapTiles;
            int tileId = mapTiles[indexInMap];
            if (ItemTileNumbers.Contains(tileId))
            {
                return MapTile.potion;
            }
            else
            {
                return MapTile.Floor;
            }
        }

        public MapLayer GetLayer(string layerName)  
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].name == layerName)
                {
                    return layers[i];
                }
            }
            Console.WriteLine($"Error: No layer with name: {layerName}");
            return null; // Wanted layer was not found!
        }

        public void LoadEnemiesAndItems(Texture spriteAtlas)
        {
            enemies = new List<enemy>();

            MapLayer enemyLayer = GetLayer("enemies");

            int[] enemyTiles = enemyLayer.mapTiles;
            int mapHeight = enemyTiles.Length / mapWidth;
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    // Laske paikka valmiiksi
                    Vector2 position = new Vector2(x, y);

                    int index = x + y * mapWidth;
                    int tileId = enemyTiles[index];
                    switch (tileId)
                    {
                        case 0:
                            // Tässä kohdassa kenttää ei ole vihollista
                            break;
                        case 128:
                            // Tässä kohdassa kenttää on örkki
                            // tileId on sama kuin drawIndex
                            //enemies.Add(new enemy("goblin", position, spriteAtlas, tileId));
                            break;
                    }
                }
            }
            items = new List<item>();

            MapLayer itemLayer = GetLayer("items");

            int[] itemTiles = itemLayer.mapTiles;
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    // Laske paikka valmiiksi
                    Vector2 position = new Vector2(x, y);

                    int index = x + y * mapWidth;
                    int tileId = itemTiles[index];
                    switch (tileId)
                    {
                        case 444:
                            items.Add(new item("potion", position, spriteAtlas, tileId));
                            break;

                        case 579:
                            items.Add(new item("potion2", position, spriteAtlas, tileId));
                            break;
                    }
                }
            }
        }

        public Vector2 GetSpritePosition(int spriteIndex, int spritesPerRow)
        {
            float spritePixelX = spriteIndex % spritesPerRow * Game.tileSize;
            float spritePixelY = spriteIndex / spritesPerRow * Game.tileSize;
            return new Vector2(spritePixelX, spritePixelY);
        }

        List<enemy> enemyTypes;
        StreamReader? fileReader;
        public void LoadEnemyTypes(string filename)
        {
            enemyTypes = new List<enemy>();

            // TODO: Tarkista että tiedosto on olemassa.
            if (File.Exists("..\\Enemies.json"))
            {
                fileReader = new StreamReader(filename);
                // TODO: Lue tiedoston sisältö samalla tavalla kuin
                // kentän lataamisessa.
                string fileContents = fileReader.ReadToEnd();

                // TODO: Käytä NewtonSoft.JSON kirjastoa ja muuta tiedoston
                // sisältö List<Enemy> muotoon
                enemyTypes = JsonConvert.DeserializeObject<List<enemy>>(fileContents);
            }
        }
        
        public void draw()
        {
            Vector2 spriteposition;
            MapLayer groundLayer = GetLayer("ground");
            int[] mapTiles = groundLayer.mapTiles;

            Console.ForegroundColor = ConsoleColor.DarkGray; // Change to map color
            int mapHeight = mapTiles.Length / mapWidth; // Calculate the height: the amount of rows

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    int index = x + y * mapWidth;
                    int spriteId = mapTiles[index];

                    if (spriteId != 0)
                    {
                        spriteId -= 1;
                    }
                    spriteposition = GetSpritePosition(spriteId, 49);

                    var source = new Rectangle(spriteposition.X, spriteposition.Y, Game.tileSize, Game.tileSize);
                    Vector2 position = new Vector2(x * Game.tileSize, y * Game.tileSize);
                    Raylib.DrawTextureRec(Game.Image, source, position, Raylib.WHITE);
                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemy currentEnemy = enemies[i];
                //Vector2 enemyPosition = currentEnemy.position;
                int enemySpriteIndex = currentEnemy.DrawIndex;

                Vector2 enemySpritePosition = GetSpritePosition(enemySpriteIndex, 49);
                var enemyspriterec = new Rectangle(enemySpritePosition.X, enemySpritePosition.Y, Game.tileSize, Game.tileSize);
                
                //Raylib.DrawTextureRec(currentEnemy.graphics, enemyspriterec, enemyPosition * 16, Raylib.WHITE);
            }
            for (int i = 0; i < items.Count; i++)
            {
                item currentitem = items[i];
                Vector2 itemPosition = currentitem.position;
                int itemSpriteIndex = currentitem.DrawIndex;

                Vector2 itemSpritePosition = GetSpritePosition(itemSpriteIndex, 49);
                var itempriterec = new Rectangle(itemSpritePosition.X, itemSpritePosition.Y, Game.tileSize, Game.tileSize);

                Raylib.DrawTextureRec(currentitem.graphics, itempriterec, itemPosition * 16, Raylib.WHITE);
            }
        }
    }
}
