using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {


    public TeleportTrigger trigger;

    public GameObject teleportTo;

    public float timePerTeleport;

    bool teleporting;
    private float passedTime;

    private void Start()
    {
        passedTime = 0;
        teleporting = false;
    }

    private void Update()
    {
        if (trigger.triggered)
        {
            teleporting = true;
            passedTime += Time.deltaTime;

            if (passedTime >= timePerTeleport){
                trigger.ins[0].transform.position = teleportTo.transform.position;
                passedTime -= timePerTeleport;
            }

        }else{
            teleporting = false;
            passedTime = 0;
        }
    }
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position,teleportTo.transform.position);
	}
}
