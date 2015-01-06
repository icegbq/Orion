using ServerBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class PacketLib : AbstractPacketLib
    {
        public PacketLib(GameClient c)
            : base(c)
        { 
            
        }
        // packets we handle
        public enum eClientPackets : byte
        {
            Hello,
            PlayerConnect,
            PlayerPositionUpdate,
            PlayerRotationUpdate,
        }
        
        // packets we send
        public enum eServerPackets : byte
        {
            Hello,
            PlayerCreate,
            PlayerPosition,
            PlayerRotation,
            PlayerDestroy,
        }

        public void SendMovementUpdate(GameObject o)
        {
            using (var pak = new PacketOut(eServerPackets.PlayerPosition))
            {
                pak.WriteInt((uint)o.ObjectID);
                pak.WriteFloat(o.Position.X);
                pak.WriteFloat(o.Position.Y);
                pak.WriteFloat(o.Position.Z);
                pak.WriteFloat(o.Velocity.X);
                pak.WriteFloat(o.Velocity.Y);
                pak.WriteFloat(o.Velocity.Z);
                SendTCP(pak);
            }
        }

        public void SendCreateObject(GameObject o)
        {
            using (var pak = new PacketOut(eServerPackets.PlayerCreate))
            {
                pak.WriteInt((uint)o.ObjectID);
                pak.WriteFloat(o.Position.X);
                pak.WriteFloat(o.Position.Y);
                pak.WriteFloat(o.Position.Z);
                SendTCP(pak);
            }
            Console.WriteLine("Client: " + this._client.Index + " added object " + o.ObjectID);
        }

        public void SendDestroyObject(GameObject o)
        {
            using (var pak = new PacketOut(eServerPackets.PlayerDestroy))
            {
                pak.WriteInt((uint)o.ObjectID);
                SendTCP(pak);
            }
        }

        public void SendRotationUpdate(GameObject o)
        {
            using (var pak = new PacketOut(eServerPackets.PlayerRotation))
            {
                pak.WriteInt((uint)o.ObjectID); 
                pak.WriteFloat(o.Rotation.X);
                pak.WriteFloat(o.Rotation.Y);
                pak.WriteFloat(o.Rotation.Z);
                SendTCP(pak);
            }
        }

        public void SendHello()
        {
            using (var pak = new PacketOut(eServerPackets.Hello))
            {
                pak.WriteInt(5);
                pak.WriteFloat(3.1415f);
                pak.WriteString("Hello this is a test string.");
                pak.WriteFloat(2014.512f); 
                SendTCP(pak);
            }
        }
    }
}
