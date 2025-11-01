namespace enigma_simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputFileName = GetInputFileName(args);
                var c = new Controller();
                c.Initialize(inputFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        static string GetInputFileName(string[] args)
        {
            if (args.Length > 1)
            {
                throw new Exception("Too many arguments provided. Only provide the input file name.");
            }
            if (args.Length < 1)
            {
                throw new Exception("Input file name not provided. Provide input file name.");
            }
            return args[0];
        }
    }
}
