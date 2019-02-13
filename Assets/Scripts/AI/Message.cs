using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Purpose: Add class to objects that want to receive messages. To send a message, 
 * set messageReceiver = new Message<T>(message_type). State machine will automatically 
 * clear messages at every frame.
 * */
public class MessageReceiver {
    public message_type message;

    public MessageReceiver()
    {
        message = 0;
    }

    public T getMessageContent<T>()
    {
        Message<T> tempCast = this as Message<T>;
        if (tempCast != null)
        {
            return tempCast.messageContent;
        }
        else
        {
            Debug.LogError("Incorrect Cast Type of MessageReceiver");
            return default(T);
        }
    }
}

/*
 * Purpose:  
 * */
public class Message<T> : MessageReceiver
{
    public T messageContent;

    public Message(message_type _message)
    {
        message = _message;
        messageContent = default(T);
    }

    public Message(message_type _message, T _messageContent)
    {
        messageContent = _messageContent;
        message = _message;
    }
}

/*
 * Add to enum if new message type is desired 
 * */
public enum message_type{
    nextWaypoint = 1, //0 is empty message
    lookAtMe
};
