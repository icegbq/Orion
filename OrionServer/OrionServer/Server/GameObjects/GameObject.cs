using ServerBase.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class GameObject
    {
        private FVector _pos, _vel, _rot;

        private int _objectID;

        public void AddToWorld()
        {
            _pos = new FVector();
            _vel = new FVector();
            _rot = new FVector();
            WorldMgr.AddToWorld(this);
        }

        public FVector Position
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public FVector Velocity
        {
            get { return _vel; }
            set { _vel = value; }
        }

        public FVector Rotation
        {
            get { return _rot; }
            set { _rot = value; }
        }

        public int ObjectID
        {
            get { return _objectID; }
            set { _objectID = value; }
        }

    }
}
