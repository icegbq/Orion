using ServerBase;
using ServerBase.PacketHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    [PacketHandlerAttribute(PacketLib.eClientPackets.PlayerRotationUpdate)]
    public class PlayerRotationUpdate : IPacketHandler
    {
        public void HandlePacket(BaseClient client, BasePacketIn packet)
        {
            if (!(client is GameClient))
                return;
            GameClient c = client as GameClient;

            if (c.Player == null)
                return;

            c.Player.Rotation.X = packet.ReadInt();
            c.Player.Rotation.Y = packet.ReadInt();
            c.Player.Rotation.Z = packet.ReadInt();
        }
    }
}
