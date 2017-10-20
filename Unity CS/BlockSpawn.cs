using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawn : MonoBehaviour
{
    public GameObject GBlock;

    void Start()
    {
        var k = 1;
        for (int i = -8; i < 8; i++)
        {
            if (i > -1){k = 0;}
            for (int j = (-7 - k + Mathf.Abs(i)); j < (8 + k - Mathf.Abs(i)); j++)
                {
                    Instantiate(GBlock, transform.position = new Vector3((164 - 164 - (34 * j)), 18 - 36 - (18 * i), -1 - i), transform.rotation);
                }
        }
    }
}

 