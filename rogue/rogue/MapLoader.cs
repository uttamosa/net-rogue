using Newtonsoft.Json;

namespace rogue
{
    internal class MapLoader
    {
        public Map LoadTestMap()
        {
            Map test = new Map();
            test.mapWidth = 8;

            test.mapTiles = new int[] {
                2, 2, 2, 2, 2, 2, 2, 2,
                2, 1, 1, 1, 1, 1, 1, 2,
                2, 1, 1, 1, 1, 1, 1, 2,
                2, 1, 1, 1, 1, 1, 1, 2,
                2, 1, 1, 1, 1, 1, 1, 2,
                2, 1, 1, 1, 1, 1, 3, 2,
                2, 2, 2, 2, 2, 2, 2, 2
            };
            return test;
        }

        public Map LoadMapFromFile(string filename)
        {
            bool exists = File.Exists(filename);
            if (exists == false)
            {
                Console.WriteLine($"File {filename} not found");
                return LoadTestMap(); // Return the test map as fallback
            }

            string fileContents = File.ReadAllText(filename);

            Map loadedmap = JsonConvert.DeserializeObject<Map>(fileContents);
            
            return loadedmap;
        }
    }
}