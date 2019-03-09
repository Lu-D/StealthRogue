using System;
using UnityEngine;

class SensoryObject : MonoBehaviour
{
    public string messageType;
    private message_type message;
    private messageTypeLookup dictionary;

    private void  Awake(){
        message = dictionary[messageType];
    }

    public message_type senseObject(){
        return message;
    }
}

