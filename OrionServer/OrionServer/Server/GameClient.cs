using ServerBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class GameClient : BaseClient
    {
        private GamePlayer _player;
        private PacketLib _packetLib = null;

        public GameClient(TcpClient client) 
            : base(client)
        {
        }

        /// <summary>
        /// Our GamePlayer, should be
        /// added to world at some point
        /// via WorldMgr
        /// </summary>
        public GamePlayer Player
        {
            get { return _player; }
            set { _player = value; }
        }

        /// <summary>
        /// PacketLib builds and sends
        /// packets to the client
        /// </summary>
        public PacketLib PacketLib
        {
            get 
            {
                if (_packetLib == null)
                    _packetLib = new PacketLib(this);
                return _packetLib; 
            }
            set { _packetLib = value; }
        }

        public override void HandleDisconnect()
        {
            base.HandleDisconnect();
            WorldMgr.RemoveClient(this);
        }
        

    }
}
