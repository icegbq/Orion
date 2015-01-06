using ServerBase.PacketHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks; 

namespace ServerBase
{
    public class PacketProcessor
    {
        private static Dictionary<int, IPacketHandler> _packetHandlers = new Dictionary<int, IPacketHandler>();

        [ServerStartedEvent]
        public static void Init()
        {
            Console.WriteLine("Initializing PacketProcessor.");
            GetPacketHandlers();
        }

        private static void GetPacketHandlers()
        {
            int count = 0;
            foreach (Type t in Assembly.GetAssembly(typeof(GameServer.GameServer)).GetTypes())
            {
                if (!t.IsClass)
                    continue;

                if (t.GetInterface("ServerBase.IPacketHandler") == null)
                    continue;

                var packetHandlerAttr = (PacketHandlerAttribute[])t.GetCustomAttributes(typeof (PacketHandlerAttribute), true);
               
                if (packetHandlerAttr.Length > 0)
                {
                    // try to register packethandler
                    if(RegisterPacketHandler(packetHandlerAttr[0].Code, (IPacketHandler) Activator.CreateInstance(t)))
                        count++;
                }
            }

            Console.WriteLine("PacketHandlers found: " + count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        private static bool RegisterPacketHandler(int code, IPacketHandler handler)
        {
            IPacketHandler test = null;
            _packetHandlers.TryGetValue(code, out test);
            if (test != null)
            {
                Console.WriteLine("Packet handler " + code + " already exists.");
                return false;
            }
            _packetHandlers[code] = handler;
            return true;
        }

        /// <summary>
        /// Returns the packethandler for the given 
        /// packet code.
        /// </summary>
        /// <param name="code">This is the value corresponding to the
        /// eClientPackets id.</param>
        /// <returns></returns>
        public static IPacketHandler GetHandler(int code)
        {
            IPacketHandler test = null;
            _packetHandlers.TryGetValue(code, out test);

            if (test == null)
            {
                Console.WriteLine("No packethandler found for packet " + code);
            }

            return test;
        }
    }
}
