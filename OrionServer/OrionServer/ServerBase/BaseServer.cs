using ServerBase;
using ServerBase.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks; 

namespace ServerBase
{
    public class BaseServer
    {
        private TcpListener _tcpListener;
        private Thread _listenThread;
          
        public BaseServer()
        {
            this._tcpListener = new TcpListener(IPAddress.Any, 3000);
            this._listenThread = new Thread(new ThreadStart(ListenForClients));
            this._listenThread.Start();
        }

        private void ListenForClients()
        {
            this._tcpListener.Start();

            onServerStart();

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

            tcpClient.SendTimeout = 5000;
            tcpClient.ReceiveTimeout = 5000;
  
            
            BaseClient c = onClientConnect(tcpClient);
            Console.WriteLine("Client " + c.Index + " has connected.");

            if (c == null)
                return;

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

                c.ReceiveBinary(message, bytesRead);
            }

            Console.WriteLine("Client " + c.Index + " has disconnected.");
            c.HandleDisconnect();
            tcpClient.Close();
        }

        protected virtual void onServerStart()
        { 
            Console.WriteLine("Server is now listening for connections.");
        }

        protected virtual BaseClient onClientConnect(TcpClient tcpClient)
        {
            BaseClient client = new BaseClient(tcpClient);
            
            return client;
        }
    }
}
