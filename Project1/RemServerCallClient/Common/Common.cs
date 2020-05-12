using System;
using System.Collections.Generic;

using System.Drawing;
using System.Windows.Forms;

public enum Operation { Add, Remove};
public delegate void AlterDelegate(Operation op, String username);

[Serializable]
public class Message
{
    public String sender;
    public String msg;
    public String tab;

    public Message(String sender, String msg, String tab)
    {
        this.sender = sender;
        this.msg = msg;
        this.tab = tab;
    }
}

public class Item
{
    public String title;
    public RichTextBox textBox;
    public Boolean status;
    public Item(String title)
    {
        this.title = title;
        this.status = true;
    }

    public void AddReceiverText(String msg, String username)
    {
        FormatMessage(msg, username, HorizontalAlignment.Left);
    }

    public void AddSenderText(String msg, String username)
    {
        FormatMessage(msg, username, HorizontalAlignment.Right);
    }

    public void ConversationRequest()
    {
        textBox.SelectionAlignment = HorizontalAlignment.Center;
        textBox.SelectionFont = new Font(textBox.Font, FontStyle.Italic);
        textBox.AppendText("Conversation request sent." + Environment.NewLine);
    }

    public void AcceptConversation(String username)
    {
        textBox.Invoke((MethodInvoker)delegate ()
        {
            textBox.SelectionAlignment = HorizontalAlignment.Center;
            textBox.SelectionFont = new Font(textBox.Font, FontStyle.Italic);
            textBox.AppendText(username + " accepted your request." + Environment.NewLine + Environment.NewLine);
        });
    }

    public void RefuseConversation(String username)
    {
        textBox.Invoke((MethodInvoker)delegate ()
        {
            textBox.SelectionAlignment = HorizontalAlignment.Center;
            textBox.SelectionFont = new Font(textBox.Font, FontStyle.Italic);
            textBox.AppendText(username + " refused your request." + Environment.NewLine);
        });
    }

    public void ChangeMsgStatus(String username)
    {
        this.status = true;
        textBox.SelectionAlignment = HorizontalAlignment.Center;
        textBox.SelectionFont = new Font(textBox.Font, FontStyle.Italic);
        textBox.AppendText(username + " has disconnected.");
    }

    private void FormatMessage(String msg, String username, HorizontalAlignment alignment)
    {
        textBox.SelectionAlignment = alignment;
        textBox.SelectionFont = new Font(textBox.Font, FontStyle.Bold);
        textBox.AppendText("" + username + " - ");
        textBox.SelectionFont = new Font(textBox.Font, FontStyle.Regular);
        textBox.AppendText(msg + Environment.NewLine);
    }

}

public class Tab
{
    public Boolean status; // online/offline
    String title;

    public Tab(String title)
    {
        this.title = title;
        this.status = true;
    }

    public String AddReceiverText1(String msg, String username)
    {
        return FormatMessage1(msg, username, HorizontalAlignment.Left);
    }

    public String AddSenderText1(String msg, String username)
    {
        return FormatMessage1(msg, username, HorizontalAlignment.Right);
    }

    public String ConversationRequest1()
    {
        return "Conversation request sent." + Environment.NewLine;
    }

    public String AcceptConversation1(String username)
    {
        return username + " accepted your request." + Environment.NewLine + Environment.NewLine;
    }

    public String RefuseConversation1(String username)
    {
        return username + " refused your request." + Environment.NewLine + Environment.NewLine;
    }

    public void ChangeMsgStatus(String username)
    {
        this.status = true;
    }

    private String FormatMessage1(String msg, String username, HorizontalAlignment alignment)
    {
        String st;
        st = "" + username + " - " + msg + Environment.NewLine;
        return st;
    }
}



public interface ISingleServer {
    event AlterDelegate alterEvent;
    int RegisterAddress(String username, string address);
    int Login(String username, string address);
    int Register(String username, string address);
    int Logout(string username);
    List<string> getUsers();
    void InviteToConversation(String sender, String receiver);
    void AcceptConversation(String sender, String receiver);
    void RefuseConversation(String sender, String receiver);
}

public interface IClientRem {
    void ReceiveRequest(String username);
    void SomeMessage(Message message);
    void AcceptConversation(String username, String address);
    void RefuseConversation(String username);
    void ReceiveAdress(String username, String Address);

}

public class AlterEventRepeater : MarshalByRefObject
{
    public event AlterDelegate alterEvent;

    public override object InitializeLifetimeService()
    {
        return null;
    }

    public void Repeater(Operation op, String username)
    {
        if (alterEvent != null)
            alterEvent(op, username);
    }
}