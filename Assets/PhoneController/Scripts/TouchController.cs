using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {
    public bool isOn = false;
	// Use this for initialization
	void Start () {
        isOn = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isOn)
        {
            string returnMessage = "Touches in Last frames: \n";
            if (Input.touchCount > 0)
            {
                int fingerCount = 0;
                foreach (Touch touch in Input.touches)
                {
                    fingerCount++;
                    returnMessage += fingerCount;
                    returnMessage += "position: ";
                    returnMessage += touch.position;
                    returnMessage += ", statues: ";
                    returnMessage += touch.phase;
                    returnMessage += "\n";
                }
                GUILayout.Label(returnMessage);
            }
        }
	}
}
