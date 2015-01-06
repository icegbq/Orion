using ServerBase;
using ServerBase.PacketHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    [PacketHandlerAttribute(PacketLib.eClientPackets.Hello)]
    public class Hello : IPacketHandler
    {
        public void HandlePacket(BaseClient client, BasePacketIn packet)
        {
            if (!(client is GameClient))
                return;
            GameClient c = client as GameClient;

            if (c.Player == null)
                return;

            int x = (int)packet.ReadInt();
            short y = (short)packet.ReadShort();

        }
    }
}
