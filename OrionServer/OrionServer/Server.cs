using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrionServer
{
    class Server
    {
        private TcpListener _tcpListener;
        private Thread _listenThread;

        public List<GameClient> _clients;

        public Server()
        {
            this._tcpListener = new TcpListener(IPAddress.Any, 3000);
            this._listenThread = new Thread(new ThreadStart(ListenForClients));
            this._listenThread.Start();
        }

        private void ListenForClients()
        {
            this._tcpListener.Start();

            while (true)
            { 
                TcpClient client = this._tcpListener.AcceptTcpClient(); 
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0)
                {
                    break;
                }

                ASCIIEncoding encoder = new ASCIIEncoding();
                System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead));
            }

            tcpClient.Close();
        }
    }
}
