using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MapContainer : MonoBehaviour
{
    public Dictionary<string, Map> maps;

    private void Awake()
    {
        maps = new Dictionary<string, Map>();
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform transform in children)
        {
            maps.Add(transform.gameObject.name, transform.GetComponentInChildren<Map>());
        }
    }
}

