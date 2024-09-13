using System;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace MovingObjectClient
{
    static class Program
    {
        static TcpClient client;
        static bool running = true;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start client connection
            client = new TcpClient("127.0.0.1", 8080);
            Thread clientThread = new Thread(ReceiveData);
            clientThread.Start();

            Application.Run(new Form1());

            running = false;
            client.Close();
        }

        static void ReceiveData()
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[8];

            while (running)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 8)
                {
                    int x = BitConverter.ToInt32(buffer, 0);
                    int y = BitConverter.ToInt32(buffer, 4);

                    Form1.UpdateRectanglePosition(x, y);
                }
            }
        }
    }
}
