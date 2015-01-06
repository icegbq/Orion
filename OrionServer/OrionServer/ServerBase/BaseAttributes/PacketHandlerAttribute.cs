using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer;

namespace ServerBase.PacketHandler
{
	[AttributeUsage(AttributeTargets.Class)]
    public class PacketHandlerAttribute : Attribute
    {
        protected int _code;
        protected string _desc;
        public PacketHandlerAttribute(PacketLib.eClientPackets code, string description)
        {
            _code = (int)code;
            _desc = description;
        }

        public PacketHandlerAttribute(PacketLib.eClientPackets code)
        {
            _code = (int)code;
        }

        public int Code
        { get { return _code; } }

        public string Description
        { get { return _desc; } }
    }
}
