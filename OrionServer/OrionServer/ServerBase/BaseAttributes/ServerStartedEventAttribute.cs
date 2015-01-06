using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerBase
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class ServerStartedEventAttribute : Attribute
    {
        public ServerStartedEventAttribute()
        { }
    }
}
