using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

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

        private struct EventInfo
        {
            public Event mainEvent;
            public float delay;

            public EventInfo(Event _event, float _delay)
            {
                mainEvent = _event;
                delay = _delay;
            }
        }

        private Queue<EventInfo> eventQueue;
        private Task currTask = null;

        private EventManager()
        {
            eventQueue = new Queue<EventInfo>();
        }

        public void addEvent(Event newEvent, float delay = 0f)
        {
            
            eventQueue.Enqueue(new EventInfo(newEvent, delay));
        }

        private IEnumerator Execute()
        {
            EventInfo firstEvent = eventQueue.First();
            if (firstEvent.mainEvent != null)
            {
                firstEvent.mainEvent.execute();
                yield return new WaitForSeconds(firstEvent.delay);
            }
        }

        private void Update()
        {
            if (currTask == null && eventQueue.Count() > 0)
            {
                currTask = new Task(Execute());
            }
            else if (currTask != null)
            {
                if(!currTask.Running)
                {
                    eventQueue.Dequeue();
                    currTask = null;
                }
                    
            }
        }
    }
}
