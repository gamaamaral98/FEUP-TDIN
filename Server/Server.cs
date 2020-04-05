using System;
// using MongoDB.Driver;
using System.Runtime.Remoting;

namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            // MongoClient dbClient = new MongoClient("mongodb://localhost:27017");
            // IMongoDatabase database = dbClient.GetDatabase("TDIN_Chat");

            // var collectionsList = database.ListCollections().ToList();

            // Console.WriteLine("The list of collections on this database is: ");
            //  foreach (var collection in collectionsList)
            //  {
            //     Console.WriteLine(collection);
            // }

            RemotingConfiguration.Configure("Server.exe.config", false);
            Console.WriteLine("Press return to exit");

            Console.ReadLine();
        }
    }
}



