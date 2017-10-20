using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StradyBG : MonoBehaviour {
    public GameObject BGS;

	// Update is called once per frame
	void Update () {
        if (BGS.transform.position.x != 0) {
            BGS.transform.position = new Vector3(0, 0, 0);
        }


    }
}
