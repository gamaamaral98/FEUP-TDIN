using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Collections.Generic;
using System.Threading;
using MongoDB.Driver;

class Server {
  static void Main(string[] args) {
    RemotingConfiguration.Configure("Server.exe.config", false);
    Console.WriteLine("[Server]: Press Return to terminate.");
    Console.ReadLine();
  }
}

public class SingleServer : MarshalByRefObject, ISingleServer {
    Hashtable onlineUsers = new Hashtable();
    
    MongoClient dbClient;
    IMongoDatabase database;

    public int InitiateDB()
    {
        dbClient = new MongoClient("mongodb://localhost:27017");
        database = dbClient.GetDatabase("TDIN_Chat");

        return 0;
    }

    public int RegisterAddress(String username, string address)
    {
        if (onlineUsers.Contains(username))
            return 1;
        else
        {
            IClientRem rem = (IClientRem)RemotingServices.Connect(typeof(IClientRem), address);
            Console.WriteLine("[SingleServer]: Sending active clients list");
            onlineUsers.Add(username, address);
            Console.WriteLine("[SingleServer]: Registered " + address);
            return 0;
        }
    }

    public int Login(string username, string password)
    {
        return 0;
    }

    public int Register(string username, string password)
    {
        return 0;
    }

    public int Logout(string username)
    {
        return 0;
    }

    public List<string> getUsers()
    {
        List<string> usernames = new List<string>();
        foreach(DictionaryEntry user in onlineUsers)
        {
            usernames.Add(user.Key.ToString());
        }
        return usernames;
    }

    public void InviteToConversation(String sender, String receiver)
    {

    }

    public void AcceptConversation(String sender, String receiver)
    {

    }

    public void RefuseConversation(String sender, String receiver)
    {

    }
}
