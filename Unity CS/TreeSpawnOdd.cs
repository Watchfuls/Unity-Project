using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawnOdd : MonoBehaviour
{
    public GameObject Tree;
    int x = 0;
    int y = 0;
    MapGenerator Mapref;
    void Start()
    {
        Mapref = GetComponent<MapGenerator>();
        for (int i = -8; i < 8; i++)
        {
            for (int j = (-8 + Mathf.Abs(i)); j < (8 - Mathf.Abs(i)); j++)
            {
                y = (i - j) + 7;
                x = 7 - (i + j);
                
                if (Mapref.map[y,x] == 1)
                {
                    Instantiate(Tree, transform.position = new Vector3((147 - 164 - (34 * j)), 27 - 18 - (18 * i), 0 - i), transform.rotation);
                }
            }
        }
    }
}

