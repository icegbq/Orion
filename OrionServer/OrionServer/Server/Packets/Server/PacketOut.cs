using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerBase; 

namespace GameServer
{
    public class PacketOut : BasePacketOut
    {
        private byte _packetID;
        public PacketOut(PacketLib.eServerPackets id)
        {
            _packetID = (byte)id;
            base.WriteShort(0x00); // size region
            base.WriteByte(_packetID);
        }
    }
}
