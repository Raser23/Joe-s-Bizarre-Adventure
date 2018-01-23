using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]

public class Player : MonoBehaviour {

    PlayerController controller;

    public bool isGrounded;

    private Vector3 Movement;

	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = gameObject.GetComponent<PlayerController>();

	}
	
	void Update () {
        
	}

	void FixedUpdate()
	{
        controller.Move(makeInput());
        	
	}


    PlayerInput makeInput(){
		float Right = Input.GetAxisRaw("Horizontal");
		float Forward = Input.GetAxisRaw("Vertical");
        PlayerInput input = new PlayerInput(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"),
                                            Right, Forward);

        return input;
    }
}

public struct PlayerInput{
    public float mouseX, mouseY;
    public float right, forward;
    public PlayerInput(float mX, float mY, float _right, float _forward){
        mouseX = mX;
        mouseY = mY;
        right = _right;
        forward = _forward;
        
    }
    
}
