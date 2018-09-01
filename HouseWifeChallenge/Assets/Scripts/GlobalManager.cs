using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GlobalManager : MonoBehaviour {

    public static Tilemap GetGroundTileMap()
    {
        Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
        foreach (Tilemap tm in tilemaps)
        {
            if (tm.name == "Ground")
            {
                return tm;
            }
        }
        return null;
    }
}
