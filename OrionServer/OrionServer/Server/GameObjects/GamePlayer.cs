using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class GamePlayer : GameObject
    {
        GameClient _client;
        public GamePlayer(GameClient c)
        {
            _client = c;
        }

        public GameClient Client
        {
            get { return _client; }
            set { _client = value; }
        }
    }
}
