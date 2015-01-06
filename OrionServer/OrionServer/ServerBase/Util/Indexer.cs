using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerBase.Util
{
    public class Indexer
    {
        private Queue<int> _availableIndexes = new Queue<int>();
        private int _index = 0;

        public int GetIndex()
        {
            if(_availableIndexes.Count <= 0)
                return _index++;
            return _availableIndexes.Dequeue();
        }

        public void SetAvailable(int i)
        {
            _availableIndexes.Enqueue(i);
        }

    }
}
