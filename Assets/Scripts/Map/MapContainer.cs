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
        //Transform[] children = GetC
        foreach (Transform child in transform)
        {
            maps.Add(child.gameObject.name, child.GetComponentInChildren<Map>());
        }
    }
}

