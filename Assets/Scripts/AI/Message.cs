using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageReceiver {
    public string message;
};

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
