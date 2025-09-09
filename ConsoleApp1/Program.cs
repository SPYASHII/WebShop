namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Start();

            Console.ReadKey();
        }

        static void Start()
        {
            StartTaskAsync();


            Console.WriteLine("start");
        }
        static async Task StartTaskAsync()
        {
            await Task.Run(Cycle);
        }
        static void Cycle()
        {
            for (int i = 0; i < 10; i++)
            {
                Task.Delay(300).Wait();
                Console.WriteLine("1\t");
            }
        }
    }
}
