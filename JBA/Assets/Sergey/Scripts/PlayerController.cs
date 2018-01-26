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
    [SerializeField] private bool m_Jumping;
    [SerializeField] private bool m_Falling;
    [SerializeField] private bool m_Running;
    private bool m_PreviouslyGrounded;

    private float startHeight, startRadius;
    private Vector3 chControllerCenter;

    [SerializeField] private float prevSpeed;

    public float jumpStartVelocity;

    private CollisionFlags m_CollisionFlags;

    Vector3 Movement;
	
	void Start () {
        controller = gameObject.GetComponent<CharacterController>();
        info = gameObject.GetComponent<PlayerInfo>();
        analiser = gameObject.GetComponent<LevelAnaliser>(); 
        Movement = Vector3.zero;
        m_Jumping = false;

        jumpStartVelocity = Mathf.Sqrt(2 * Gravity*Physics.gravity.magnitude * info.jumpHeight);

        startHeight = controller.height;
        startRadius = controller.radius;
        chControllerCenter = controller.center;
        prevSpeed = 0;

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

		Vector3 desiredMove = transform.forward * input.forward +
							   transform.right * input.right;

		RaycastHit hitInfo;
		Physics.SphereCast(transform.position, controller.radius, Vector3.down, out hitInfo,
						   controller.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
		desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        isgrounded = trigger.triggered;
        float speed = CurrentSpeed(input.Shift, new Vector3(desiredMove.x,0,desiredMove.z).magnitude != 0);

        passedTime += Time.deltaTime;
        if (passedTime < info.notWorkingTime)
            return;
        


		

        Movement.x = desiredMove.x * speed;
        Movement.z = desiredMove.z * speed;
        //Movement.y += desiredMove.y * info.walkSpeed;

        if(isgrounded ){
            m_Jumping = false;
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


        m_Falling = !isgrounded && !m_Jumping;
	}


    [SerializeField]private bool prevShift;
  
    public float CurrentSpeed(bool shift,bool moving){

        float targetSpeed = info.walkSpeed;

        if(!moving)
        {
            targetSpeed = 0;    
        }
        else
            if(shift)
        {
            if (isgrounded)
            {
                m_Running = true;
                targetSpeed = info.runSpeed;
            }else if(m_Running){
                targetSpeed = info.runSpeed;
            }else{
                m_Running = false;
            }
            }else{
                m_Running = false;
            }

        float currentSpeed = prevSpeed;

        float delta = targetSpeed - currentSpeed;
        float deltaSpeed = info.acceleration * Time.fixedDeltaTime;

        if (Mathf.Abs(delta) < Mathf.Abs(deltaSpeed))
        {
            currentSpeed = targetSpeed;
        }
        else
        {
            currentSpeed = currentSpeed + (Mathf.Sign(targetSpeed - currentSpeed)) * deltaSpeed;
        }
        if(delta > 0){
            currentSpeed = Mathf.Clamp(currentSpeed, 0, targetSpeed);
        }else{
			currentSpeed = Mathf.Clamp(currentSpeed, targetSpeed, currentSpeed);
		}

        prevSpeed = currentSpeed;
        prevShift = shift;
        return currentSpeed;
    }


	void OnCollisionEnter(Collision theCollision)
	{
        if (theCollision.transform.tag != "Player")
		{
			isgrounded = true;
		}
	}

	
	void OnCollisionExit(Collision theCollision)
	{
		if (theCollision.transform.tag != "Player")
		{
			isgrounded = false;
		}
	}

    public bool CanStand(int a,int b){
        List<Vector3> pnts = new List<Vector3>();
		Vector3 pnt1 = transform.position + chControllerCenter + Vector3.down * (startHeight / 2 - startRadius);
		Vector3 pnt0 = pnt1 - Vector3.up * (startHeight - 2 * startRadius);
		Vector3 pnt2 = transform.position + chControllerCenter + Vector3.up * (startHeight / 2 - startRadius);
		Vector3 pnt3 = pnt2 + Vector3.up * (startHeight - 2 * startRadius);

        pnts.Add(pnt0);
        pnts.Add(pnt1);
        pnts.Add(pnt2);
        pnts.Add(pnt3);

        Vector3 pntA = pnts[a];
        Vector3 pntB = pnts[b];

		Collider[] cols = Physics.OverlapCapsule(pntA, pntB, startRadius);
		//print(cols.Length);
		foreach (Collider col in cols)
		{
			if (col.tag != "Player")
			{
				return false;
			}
		}
		return true;
	}

    private void OnDrawGizmos()
    {
        return;
        Gizmos.color = Color.green;
		Vector3 pnt1 = transform.position + chControllerCenter + Vector3.down * (startHeight / 2 - startRadius);
		Vector3 pnt0 = pnt1 - Vector3.up * (startHeight - 2 * startRadius);
		Vector3 pnt2 = transform.position + chControllerCenter + Vector3.up * (startHeight / 2 - startRadius);
        Vector3 pnt3 = pnt2 + Vector3.up * (startHeight - 2 * startRadius);

        Gizmos.DrawSphere(pnt0,startRadius);
		Gizmos.DrawSphere(pnt1,startRadius);
		Gizmos.DrawSphere(pnt2, startRadius);
		Gizmos.DrawSphere(pnt3, startRadius);


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
