using GameServer;
using OrionServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServerBase
{
    public class AbstractPacketLib
    { 
        protected readonly BaseClient _client;

        public AbstractPacketLib(BaseClient client)
        {
            _client = client;
        }

        public virtual byte GetPacketCode(PacketLib.eServerPackets packetCode)
        {
            return (byte)packetCode;
        }

        public void SendTCP(byte[] bytes)
        {
            //string hex = ByteArrayToString(bytes);
            //Console.WriteLine(hex);
            _client.SendBinary(bytes);
        }

        public void SendTCP(BasePacketOut packet)
        {
            packet.WritePacketLength();
            byte[] buf = packet.GetBuffer();
            SendTCP(buf);
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
