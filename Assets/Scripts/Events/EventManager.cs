using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

/*
 * Event manager queues 
 * */
namespace Events
{
    public class EventManager : MonoBehaviour
    {
        private static EventManager instance;
        public static EventManager Instance{
            get{
                if (instance == null)
                {
                    GameObject go = new GameObject("TaskManager");
                    instance = go.AddComponent<EventManager>();
                }
                return instance;
            }
        }

        private Queue<Event> eventQueue;

        private EventManager()
        {
            eventQueue = new Queue<Event>();
        }

        public void addEvent(Event newEvent)
        {
            
            eventQueue.Enqueue(newEvent);
        }

        private void Dispatch()
        {
            eventQueue.First().execute();
        }

        private void Update()
        {
            if (eventQueue.Count() > 0)
            {
                Dispatch();
                eventQueue.Dequeue();
            }
        }
    }
}
