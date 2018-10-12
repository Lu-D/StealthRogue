using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Message
{
    public GameObject sender;
    public State newState;

    public Message(GameObject _sender, State _newState)
    {
        sender = _sender;
        newState = _newState;
    }
}
