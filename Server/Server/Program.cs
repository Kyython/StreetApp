using System;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // Добавление тестовых данных
            //using (var dataContext = new DataContext())
            //{
            //    dataContext.Streets.AddRange(new List<Street> { new Street { Index = "Z01D2K1", Address = "Астана, проспект Бауыржан Момышұлы, дом 2/10" },
            //        new Street { Index = "Z01D2K1", Address = "Астана, проспект Бауыржан Момышұлы, дом 2/11" } });

            //    dataContext.SaveChanges();
            //}

            int port = 12345;

            ThreadPool.QueueUserWorkItem(x =>
            {
                var server = new TcpServer();
                server.RunServer(port);
            });

            Console.WriteLine("Нажмите Enter чтобы завершить программу!");
            Console.ReadLine();
        }

    }
}
