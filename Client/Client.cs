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
            MyService obj = new MyService();
            Console.WriteLine(obj.Hello());
            Console.ReadLine();
        }
    }

    public class MyService : MarshalByRefObject
    {
        public string Hello()
        {
            return null;
        }

    }
}
