using ServerBase;
using ServerBase.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer
{
    public class WorldMgr
    {
        #region PrivateMembers
        private static GameObject[] _worldObjects = new GameObject[Constants.MaxObjectCount];
        private static GameClient[] _clients = new GameClient[Constants.MaxClientCount];

        private static Indexer _indexer = new Indexer();
        private static Indexer _cIndexer = new Indexer();

        private static Timer _worldTimer = null;
        #endregion

        private static void Tick(object state)
        {
            SendUpdates();
        }

        /// <summary>
        /// Update each player with server view
        /// ignore target clients
        /// </summary>
        private static void SendUpdates()
        {
            foreach (GameClient sender in _clients)
            {
                if (sender == null)
                    continue;

                foreach (GameClient c in _clients)
                {
                    if (c == null)
                        continue;
                    if (c == sender)
                        continue;
                    c.PacketLib.SendMovementUpdate(sender.Player);
                    c.PacketLib.SendRotationUpdate(sender.Player);
                }
            }
        }

        /// <summary>
        /// Called when a server receives a tcpclient
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static GameClient AddClient(GameClient c)
        {
            int index = _cIndexer.GetIndex();

            if (index >= Constants.MaxClientCount)
            {
                Console.WriteLine("Failed to add client to list, count greater than MaxClientCount.");
                return null;
            }

            GamePlayer p = new GamePlayer(c);
            c.Player = p; 
            c.Index = index; 
            _clients[index] = c;
            p.AddToWorld(); 

            c.PacketLib.SendHello(); 
            return c;
        }

        public static void RemoveClient(GameClient c)
        {
            _clients[c.Index] = null;
            _cIndexer.SetAvailable(c.Index);
            RemoveFromWorld(c.Player);
        }

        /// <summary>
        /// Tell all clients to add this object
        /// to object list, and create it.
        /// 
        /// If this is a GamePlayer we must 
        /// prevent the invoking client from 
        /// receiving this packet. Tell everyone
        /// else to add him
        /// </summary>
        /// <param name="o"></param>
        public static void AddToWorld(GameObject o)
        {
            o.ObjectID = _indexer.GetIndex();
            _worldObjects[o.ObjectID] = o;

            foreach (GameClient c in _clients)
            {
                if (c == null)
                    continue;
                if (o is GamePlayer)
                {
                    GamePlayer p = o as GamePlayer;
                    if (p.Client == c)
                        continue;
                }

                c.PacketLib.SendCreateObject(o);
            }

            GamePlayer player = o as GamePlayer;
            if (player != null)
            {
                foreach (GameObject obj in _worldObjects)
                {
                    if (obj == null)
                        continue;

                    if (obj == o)
                        continue;

                    player.Client.PacketLib.SendCreateObject(obj);

                }
            }
        }

        /// <summary>
        /// Tell all the clients to remove this
        /// object from their object list and 
        /// destroy it
        /// </summary>
        /// <param name="o"></param>
        public static void RemoveFromWorld(GameObject o)
        {
            _worldObjects[o.ObjectID] = null;
            _indexer.SetAvailable(o.ObjectID);

            foreach (GameClient c in _clients)
            {
                if (c == null)
                    continue;
                if (o is GamePlayer)
                {
                    GamePlayer p = o as GamePlayer;
                    if (p.Client == c)
                        continue;
                }

                c.PacketLib.SendDestroyObject(o);
            }
        }

        /// <summary>
        /// Initializes a threaded timer which invokes
        /// updates and sends world information to each
        /// client. I.e. player positions etc.
        /// </summary>
        [ServerStartedEvent]
        public static void InitWorldTimer()
        {
            _worldTimer = new Timer(Tick, null, 0, 10);
            Console.WriteLine("Initializing WorldMgr.");
        }
        
    }
}
