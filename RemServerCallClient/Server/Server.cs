using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Collections.Generic;
using System.Threading;
using MongoDB.Driver;
using MongoDB.Bson;

class Server {
  static void Main(string[] args) {
    RemotingConfiguration.Configure("Server.exe.config", false);
    Console.WriteLine("[Server]: Press Return to terminate.");
    Console.ReadLine();
  }
}

public class UserModel
{
    public ObjectId Id { get; set; }
    public string username { get; set; }
    public string password { get; set; }
}

public class SingleServer : MarshalByRefObject, ISingleServer {

    public event AlterDelegate alterEvent;
    Hashtable onlineUsers = new Hashtable();

    static MongoClient dbClient = new MongoClient("mongodb://localhost:27017");
    IMongoDatabase database = dbClient.GetDatabase("TDIN_Chat");

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
            alert(Operation.Add, username);
            return 0;
        }
    }

    /*
     * 0 for correct login
     * 1 for correct username, wrong password
     * 2 for user already logged
     * 3 for non existent user
     */
    public int Login(string username, string password)
    {
        var collection = database.GetCollection<UserModel>("User");
        var filter = Builders<UserModel>.Filter.Eq("username", username);
        var user = collection.Find(filter).FirstOrDefault();

        if(user != null)
        {
            if (user.password != password) return 1;

           else if (onlineUsers.Contains(username)) return 2;

            return 0;
        }

        return 3;
    }

    public int Register(string username, string password)
    {
        var collection = database.GetCollection<UserModel>("User");
        UserModel newUser = new UserModel();
        newUser.username = username;
        newUser.password = password;
        collection.InsertOneAsync(newUser);
        return 0;
    }

    public int Logout(string username)
    {
        onlineUsers.Remove(username);
        Console.WriteLine(username + " disconnected");
        alert(Operation.Remove, username);
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
        IClientRem rem = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)onlineUsers[receiver]);
        rem.ReceiveRequest(sender);
    }

    public void AcceptConversation(String sender, String receiver)
    {
        IClientRem remSender = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)onlineUsers[sender]);
        IClientRem remReceiver = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)onlineUsers[receiver]);
        remSender.AcceptConversation(receiver, (string)onlineUsers[receiver]);
        remReceiver.ReceiveAdress(sender, (string)onlineUsers[sender]);
    }

    public void RefuseConversation(String sender, String receiver)
    {
        IClientRem remSender = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)onlineUsers[sender]);
        remSender.RefuseConversation(receiver);
    }

    void alert(Operation op, String username)
    {
        if (alterEvent != null)
        {
            Delegate[] invkList = alterEvent.GetInvocationList();

            foreach (AlterDelegate handler in invkList)
            {
                new Thread(() => {
                    try
                    {
                        handler(op, username);
                    }
                    catch (Exception)
                    {
                        alterEvent -= handler; 
                    }
                }).Start();
            }
        }
    }
}
