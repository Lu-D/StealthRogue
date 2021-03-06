﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using PriorityQueueDemo;

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
                    GameObject go = new GameObject("EventManager");
                    instance = go.AddComponent<EventManager>();
                }
                return instance;
            }
        }

        private PriorityQueue<float, Event> eventQueue;

        private EventManager()
        {
            eventQueue = new PriorityQueue<float, Event>();
        }

        public void addEvent(Event newEvent, float delay = 0)
        {
            eventQueue.Enqueue(delay + Time.time, newEvent);
        }

        private void Dispatch()
        {
            eventQueue.First().Value.execute();
        }

        private void Update()
        {
            float currentTime = Time.time;    

            while (eventQueue.Count > 0 &&
            eventQueue.First().Key < currentTime)
            {
                Dispatch();
                eventQueue.Dequeue();
            }
        }
    }
}
