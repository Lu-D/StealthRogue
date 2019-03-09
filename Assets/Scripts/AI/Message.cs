using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
Purpose: Add class to objects that want to receive messages. 

//To send a message 
messageReceiver = new Message<T>(message_type).

//Get contents of message 
messageReceiver.getMessageContent<Type>();
Note: If current message is not the message content type you casted to,
then error will be thrown and T() will be returned instead;

Note: State machine will automatically clear messageReceiver for
BEnemy at every state change. If a message results in staying the same
state, make sure to call StateMachine.clearMessages() before proceeding
to the next call.

Note: Use enums to send message, add a new enum if you want a new type of message.
0 is an empty message.
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

    public void clearMessages()
    {
        message = message_type.empty;
    }
}

/*
 * Purpose: Message class that will be instantiated and set to the MessageReceiver class
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
    empty = 0,
    nextWaypoint, 
    lookAtMe
};

/*
 * Use this to convert strings to message_type (more efficeint than Systems.Enum.TryParse algorithms)
 * */
public class messageTypeLookup
{
    public Dictionary<string, message_type> messageDictionary;

    public messageTypeLookup()
    {
        foreach (message_type value in Enum.GetValues(typeof(message_type)))
        {
            messageDictionary.Add(Enum.GetName(typeof(message_type), value), value);
        }
    }

    public message_type this[string key]
    {
        get
        {
            return getMessage(key);
        }
    }

    private message_type getMessage(string key)
    {
        if (!messageDictionary.ContainsKey(key))
        {
            Debug.LogError("Attempt to access nonexistant key in messageDic: " + key);
            return message_type.empty;
        }
        else
            return messageDictionary[key];
    }

}