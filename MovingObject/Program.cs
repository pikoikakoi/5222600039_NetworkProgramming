using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MovingObjectServer
{
    static class Program
    {
        static TcpListener listener;
        static List<TcpClient> clients = new List<TcpClient>();
        static bool running = true;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start the listener server on a separate thread
            Thread listenerThread = new Thread(StartServer);
            listenerThread.Start();

            Application.Run(new Form1());

            // Stop the server when the application closes
            running = false;
            listener.Stop();
        }

        static void StartServer()
        {
            listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();
            Console.WriteLine("Server started...");

            while (running)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    lock (clients)
                    {
                        clients.Add(client);
                    }

                    Console.WriteLine("Client connected...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public static void BroadcastPosition(int x, int y)
        {
            byte[] data = BitConverter.GetBytes(x).Concat(BitConverter.GetBytes(y)).ToArray();

            lock (clients)
            {
                foreach (var client in clients)
                {
                    try
                    {
                        client.GetStream().Write(data, 0, data.Length);
                    }
                    catch (Exception)
                    {
                        // Remove disconnected client
                        clients.Remove(client);
                    }
                }
            }
        }
    }
}
