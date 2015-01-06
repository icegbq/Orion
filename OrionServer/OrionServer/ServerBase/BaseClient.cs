using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerBase
{
    public class BaseClient
    {
        TcpClient _tcpClient;
        private int _index;

        public BaseClient(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
        }

        public void SendMessage(string msg)
        {
            byte[] toBytes = Encoding.ASCII.GetBytes(msg);

            _tcpClient.GetStream().Write(toBytes, 0, toBytes.Length);
        }

        public void SendBinary(byte[] packet)
        {
            try
            {
                _tcpClient.GetStream().Write(packet, 0, packet.Length);
            }
            catch (Exception e)
            {
                _tcpClient.Close();
                HandleDisconnect();
            }
        }

        public void ReceiveBinary(byte[] msg, int bytesRead)
        {
            //string str = Encoding.ASCII.GetString(msg, 0, bytesRead);
            
            //Console.WriteLine("Received message: " + str);

            HandlePackets(msg, bytesRead);
        }

        protected virtual void HandlePackets(byte[] packet, int bytesRead)
        {
            using (var pack = new BasePacketIn(packet, 0, bytesRead))
            {
                ushort size = pack.ReadShort();
                int code = pack.ReadByte();
                HandlePackets(pack, code);
            }
        }

        protected virtual void HandlePackets(BasePacketIn packet, int code)
        {
            IPacketHandler handler = PacketProcessor.GetHandler(code);
            if(handler != null)
                handler.HandlePacket(this, packet);
        }

        public virtual void HandleDisconnect()
        { 
            
        }

        public int Index
        { 
            get { return _index; }
            set { _index = value; }
        }
    }
}
