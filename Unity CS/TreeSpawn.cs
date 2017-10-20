using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawn : MonoBehaviour
{
    public GameObject Tree;
    MapGenerator playerInstance;
    int x = 0;
    int y = 0;
    MapGenerator Mapref;
    void Start()
    {
        Mapref = GetComponent<MapGenerator>();
        var k = 1;
        for (int i = -8; i < 8; i++)
        {
            if (i > -1) { k = 0; }
            for (int j = (-7 - k + Mathf.Abs(i)); j < (8 + k - Mathf.Abs(i)); j++)
            {
                x = 7 - (i + j);
                y = (i - j) + 8;
                if (Mapref.map[y,x] == 1)
                {
                  Instantiate(Tree, transform.position = new Vector3((164 - 164 - (34 * j)), 18 - 18 - (18 * i), -1 - i), transform.rotation);
                }
            }
        }
    }
}

