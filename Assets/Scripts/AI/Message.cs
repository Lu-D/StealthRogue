using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageReceiver {
    public string message;

    public MessageReceiver()
    {
        message = null;
    }

    public T getMessageContent<T>()
    {
        return ((Message<T>)this).messageContent;   
    }
}

public class Message<T> : MessageReceiver
{
    public T messageContent;

    public Message(string _message)
    {
        message = _message;
        messageContent = default(T);
    }

    public Message(string _message, T _messageContent)
    {
        messageContent = _messageContent;
        message = _message;
    }
}
