using System;
using System.Collections.Generic;

public interface ISingleServer {
    int RegisterAddress(String username, string address);
    int InitiateDB();
    int Login(String username, string address);
    int Register(String username, string address);
    int Logout(string username);
    List<string> getUsers();
    void InviteToConversation(String sender, String receiver);
    void AcceptConversation(String sender, String receiver);
    void RefuseConversation(String sender, String receiver);

}

public interface IClientRem {
  void SomeMessage(string message);
}