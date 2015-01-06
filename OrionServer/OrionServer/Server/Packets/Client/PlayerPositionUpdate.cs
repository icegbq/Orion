using ServerBase;
using ServerBase.PacketHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    [PacketHandlerAttribute(PacketLib.eClientPackets.PlayerPositionUpdate)]
    public class PlayerPositionUpdate : IPacketHandler
    {
        public void HandlePacket(BaseClient client, BasePacketIn packet)
        {
            if (!(client is GameClient))
                return;
            GameClient c = client as GameClient;

            if (c.Player == null)
                return;

            c.Player.Position.X = (float)packet.ReadInt();
            c.Player.Position.Y = (float)packet.ReadInt();
            c.Player.Position.Z = (float)packet.ReadInt();

            //Console.WriteLine(c.Index + " " + c.Player.X + " " + c.Player.Y + " " + c.Player.Z);
        }
    }
}
