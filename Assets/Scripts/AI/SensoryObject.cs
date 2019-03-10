using System;
using UnityEngine;

class SensoryObject : MonoBehaviour
{

    public string messageType = "empty";
    private message_type message;

    private void  Awake(){
        message = messageTypeLookup.Instance[messageType];
    }

    public message_type senseObject(){
        return message;
    }
}

