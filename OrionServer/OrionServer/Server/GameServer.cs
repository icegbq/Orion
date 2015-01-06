using ServerBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class GameServer : BaseServer
    {
        protected override BaseClient onClientConnect(System.Net.Sockets.TcpClient tcpClient)
        {
            return WorldMgr.AddClient(new GameClient(tcpClient));
        }

        protected override void onServerStart()
        {  
            foreach (Type t in Assembly.GetAssembly(typeof(GameServer)).GetTypes())
            {
                foreach (MethodInfo m in t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {

                    object[] myAttribs = m.GetCustomAttributes(typeof(ServerStartedEventAttribute), false);
                    if (myAttribs.Length != 0)
                    {
                        m.Invoke(null, null);
                    }
                }
            }

            base.onServerStart();
        }
         
    }
}
