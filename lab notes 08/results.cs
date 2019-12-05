class Program
    {
        static void Main(string[] args)
        {
            int insertsCount = 1000;

            DateTime start = DateTime.Now;

            InsertPostgresBenchmark.insertTest(insertsCount);

            DateTime end = DateTime.Now;

            var duration = (end - start).TotalSeconds;

            Console.WriteLine("Statistics of insertions");
            Console.WriteLine("Number of inserts:" + insertsCount.ToString());
            Console.WriteLine("Duration:" + duration.ToString());
            Console.WriteLine("Inserts per seconds:" + (insertsCount / duration).ToString());

            Console.ReadKey();
        }
    }