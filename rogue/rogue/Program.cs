namespace rogue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool again = true;
            
            while (again)
            {
                Game rogue = new Game();
                rogue.Run();

                Console.WriteLine("uudestaan? Y/N");
                if (Console.ReadLine() == "N")
                {
                    again = false;
                }
            }
        }
    }
}