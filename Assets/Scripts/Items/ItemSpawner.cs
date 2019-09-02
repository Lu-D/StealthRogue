using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PriorityQueueDemo;

namespace Items
{
    public class ItemSpawner : MonoBehaviour
    {
        private static ItemSpawner instance;
        public static ItemSpawner Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("EventManager");
                    instance = go.AddComponent<ItemSpawner>();
                }
                return instance;
            }
        }

        private PriorityQueue<float, EquipmentController> itemQueue;
    }
}

