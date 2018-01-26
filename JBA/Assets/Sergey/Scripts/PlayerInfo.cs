using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	public float notWorkingTime = 0.4f;

    public float walkSpeed;

    public float runSpeed;

    public float jumpHeight;

    public float acceleration;
	
}
