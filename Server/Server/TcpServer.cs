using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class TcpServer
    {
        private const int NULL = 0;

        public void RunServer(int port)
        {
            var tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = null;
                NetworkStream networkStream = null;

                try
                {
                    tcpClient = tcpListener.AcceptTcpClient();
                    Console.WriteLine($"Клиент подключился - {tcpClient.Client.LocalEndPoint.ToString()}");

                    networkStream = tcpClient.GetStream();
                    var responseWriter = new StreamWriter(networkStream) { AutoFlush = true };

                    while (true)
                    {
                        if (IsDisconnected(tcpClient))
                            throw new Exception("Клиент отключен");

                        if (networkStream.DataAvailable)             
                        {
                            string request = Read(networkStream);

                            responseWriter.Write(request);
                        }
                    }
                }
                catch (Exception exception)
                {
                    networkStream.Close();
                    tcpClient.Close();
                    Console.WriteLine($"Error: {exception.Message}");
                }
            }
        }

        private bool IsDisconnected(TcpClient tcpClient)
        {
            if (tcpClient.Client.Poll(NULL, SelectMode.SelectRead))
            {
                const int BUFFER_SIZE = 1;
                byte[] buffer = new byte[BUFFER_SIZE];

                if (tcpClient.Client.Receive(buffer, SocketFlags.Peek) == NULL)
                    return true;
            }

            return false;
        }

        private string Read(NetworkStream networkStream)
        {
            const int BUFFER_SIZE = 1024;
            byte[] buffer = new byte[BUFFER_SIZE];

            int bufferSize = networkStream.Read(buffer, NULL, buffer.Length);

            string index = Encoding.UTF8.GetString(buffer, NULL, bufferSize);

            
            var streetList = GetStreets(index);

            if (streetList.Count == 0)
            {
                return "Not found\n";
            }

            string streets = string.Empty;

            foreach (var street in streetList)
            {
                streets += $"{street.Address}\n";
            }

            return streets;
        }

        private List<Street> GetStreets(string index)
        {
            List<Street> streets = new List<Street>();

            using (var dataContext = new DataContext())
            {
                streets = dataContext.Streets.Where(street => street.Index.ToLower() == index.ToLower()).ToList();
            }

            return streets;
        }
    }
}
