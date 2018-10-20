using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Message
{
    public Vector3 senderPosition;
    public string message;

    public Message(string _message)
    {
        message = _message;
        senderPosition = Vector3.zero;
    }

    public Message(Vector3 _sender, string _message)
    {
        senderPosition = _sender;
        message = _message;
    }
}
