using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]

public class Player : MonoBehaviour {

    bool seatUp;

    PlayerController controller;
    Animator animator;
    public Hands hands;

    //public bool isGrounded;

    private bool needToStand;
    private bool seating;



    private Vector3 Movement;

	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = gameObject.GetComponent<PlayerController>();
        animator = gameObject.GetComponent<Animator>();
	}
	
	void Update () {
        Seating();

        hands.HandsController(animator);



	}
    void Seating(){
		if (Input.GetKeyDown(KeyCode.C))
		{
			Seat();
			needToStand = false;
			seating = true;
		}
		if (Input.GetKeyUp(KeyCode.C))
		{

			needToStand = true;
		}

		if (needToStand)
		{
			if (!seatUp)
			{//SeatDown

				bool canUp = controller.CanStand(0, 1);
				bool canDown = controller.CanStand(1, 2);

				if (canUp)
				{
					//print("here");
					animator.PlayInFixedTime("SeatDownStandUp");
					needToStand = false;
					seating = false;
				}
				else if (canDown)
				{
					animator.PlayInFixedTime("SeatDownStandDown");
					needToStand = false;
					seating = false;
				}
			}
			else
			{
				bool canUp = controller.CanStand(1, 2);
				bool canDown = controller.CanStand(2, 3);

				if (canUp)
				{

					animator.PlayInFixedTime("SeatUpStandUp");
					needToStand = false;
					seating = false;
				}
				else if (canDown)
				{
					//print("here");
					animator.PlayInFixedTime("SeatUpStandDown");
					needToStand = false;
					seating = false;
				}

			}


		}
    }

    void Seat(){

        if(controller.isgrounded){
            animator.PlayInFixedTime("SeatDown");
            seatUp = false;
        }else{
            animator.PlayInFixedTime("SeatUp");
            seatUp = true;
        }
    }


	void FixedUpdate()
	{
        controller.Move(makeInput());
        	
	}


    PlayerInput makeInput(){
		float Right = Input.GetAxisRaw("Horizontal");
		float Forward = Input.GetAxisRaw("Vertical");
        PlayerInput input = new PlayerInput(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"),
                                            Right, Forward,Input.GetKey(KeyCode.LeftShift));

        return input;
    }
}

public struct PlayerInput{
    public float mouseX, mouseY;
    public float right, forward;

    public bool Shift;

    public PlayerInput(float mX, float mY, float _right, float _forward, bool _shift){
        mouseX = mX;
        mouseY = mY;
        right = _right;
        forward = _forward;
        Shift = _shift;
        
    }
    
}
