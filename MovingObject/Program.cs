using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MovingObject
{
    static class Program
    {
        static List<TcpClient> clients = new List<TcpClient>();
        static TcpListener server = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Start TCP Server
            StartServer();
            Application.Run(new Form1());
        }
        static void StartServer()
        {
            try
            {
                //set the server to listen on a specific port
                server = new TcpListener(IPAddress.Any, 1000);
                server.Start();
                Console.WriteLine("server started...");

                //start a new thread to handle incoming client connect
                Thread serverThread = new Thread(() =>
                {
                    while (true)
                    {
                        TcpClient client = server.AcceptTcpClient();
                        lock (clients)
                        {
                            clients.Add(client);
                        }
                        Console.WriteLine("Client connected...");
                    }
                });
                serverThread.IsBackground = true;
                serverThread.Start();
            }
            catch (Exception e)
            {
               Console.WriteLine("Error: " + e.Message);
            }
            
        }
    }
}
    //Broadcast image method
    
