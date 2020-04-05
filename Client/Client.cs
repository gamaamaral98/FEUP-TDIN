using System;

using System.Runtime.Remoting;
using Common;

namespace Client
{
    class Remote : MarshalByRefObject, IRemote
    {
        public string Hello()
        {
            return null;
        }

    }
    class Client
    {
        static void Main(string[] args)
        {
            RemotingConfiguration.Configure("Client.exe.config", false);
            Remote obj = (Remote)RemotingServices.Connect(typeof(Remote), "tcp://localhost:9000/Server/RemObj");
            Console.WriteLine(obj.Hello());
            Console.ReadLine();
        }
    }
}
