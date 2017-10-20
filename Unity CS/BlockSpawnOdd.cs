using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnOdd : MonoBehaviour
{
    public GameObject GBlock;

    void Start()
    {
        for (int i = -8; i < 8; i++)
        {
            for (int j = (-8 + Mathf.Abs(i)); j < (8 - Mathf.Abs(i)); j++)
            {
                Instantiate(GBlock, transform.position = new Vector3((147 - 164 - (34 * j)), 27 - 36 - (18 * i), 0-i), transform.rotation);
            }
        }
    }
}

