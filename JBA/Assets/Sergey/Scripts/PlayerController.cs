using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInfo))]
public class PlayerController : MonoBehaviour {

    public float Gravity;

    public GameObject legCollider;

    public LegTrigger trigger;

	public GameObject CameraY;
	float rotationY = 0F;
    float rotationX = 0F;

    public bool isgrounded;
    [SerializeField] private float m_StickToGroundForce;
    private CharacterController controller;
    private PlayerInfo info;
    private LevelAnaliser analiser;
    private bool m_Jump;
    private bool m_Jumping;
    private bool m_PreviouslyGrounded;

    public float jumpStartVelocity;

    private CollisionFlags m_CollisionFlags;

    Vector3 Movement;
	
	void Start () {
        controller = gameObject.GetComponent<CharacterController>();
        info = gameObject.GetComponent<PlayerInfo>();
        analiser = gameObject.GetComponent<LevelAnaliser>(); 
        Movement = Vector3.zero;
        m_Jumping = false;

        //jumpStartVelocity = Mathf.Sqrt(2 * Gravity * info.jumpHeight);
	}

    float passedTime = 0F;
	
    void Update(){
		if (!m_Jump)
		{
            m_Jump = Input.GetKeyDown(KeyCode.Space);
		}


        m_PreviouslyGrounded = trigger.triggered;
    }

    public void Move (PlayerInput input) {
        passedTime += Time.deltaTime;
        if (passedTime < info.notWorkingTime)
            return;
        
        isgrounded = trigger.triggered;

		Vector3 desiredMove = transform.forward * input.forward * info.walkSpeed +
							   transform.right * input.right * info.walkSpeed;

		RaycastHit hitInfo;
        Physics.SphereCast(transform.position, controller.radius, Vector3.down, out hitInfo,
                           controller.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
		desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        Movement.x = desiredMove.x * info.walkSpeed;
        Movement.z = desiredMove.z * info.walkSpeed;
        //Movement.y += desiredMove.y * info.walkSpeed;

        if(isgrounded ){
            
             Movement.y = m_StickToGroundForce;
			if (m_Jump)
            {
                Movement.y = jumpStartVelocity;
				m_Jump = false;
				m_Jumping = true;
			}

        }else{
            Movement += Physics.gravity * Gravity * Time.fixedDeltaTime;
        }


        m_CollisionFlags = controller.Move(Movement * Time.fixedDeltaTime);



        //Head rotation
		rotationX += input.mouseX * info.sensitivityX;
		rotationY += input.mouseY * info.sensitivityY;
		rotationY = Mathf.Clamp(rotationY, info.minimumY, info.maximumY);

		transform.localEulerAngles = new Vector3(0, rotationX, 0);
		CameraY.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);

	}
	void OnCollisionEnter(Collision theCollision)
	{
        if (theCollision.transform.tag != "Player")
		{
			isgrounded = true;
		}
	}

	//consider when character is jumping .. it will exit collision.
	void OnCollisionExit(Collision theCollision)
	{
		if (theCollision.transform.tag != "Player")
		{
			isgrounded = false;
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;
		//dont move the rigidbody if the character is on top of it
		if (m_CollisionFlags == CollisionFlags.Below)
		{
			return;
		}

		if (body == null || body.isKinematic)
		{
			return;
		}
        body.AddForceAtPosition(controller.velocity * 0.1f, hit.point, ForceMode.Impulse);
	}

}
