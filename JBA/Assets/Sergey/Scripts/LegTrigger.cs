using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegTrigger : MonoBehaviour {

    public bool triggered;

	void OnTriggerEnter(Collider other)
	{
        triggered = true;
	}
	void OnTriggerExit(Collider other)
	{
        triggered = false;
	}
}
