using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Message
{
    public Vector3 senderPosition;
    public State newState;

    public Message(State _newState)
    {
        newState = _newState;
        senderPosition = Vector3.zero;
    }

    public Message(Vector3 _sender, State _newState)
    {
        senderPosition = _sender;
        newState = _newState;
    }
}
