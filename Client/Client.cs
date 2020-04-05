using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Remoting;

namespace Client
{
    class Client
    {
        static void Main(string[] args)
        {
            RemotingConfiguration.Configure("Client.exe.config", false);
            Remote obj = new Remote();
            Console.WriteLine(obj.Hello());
            Console.ReadLine();
        }
    }
}
