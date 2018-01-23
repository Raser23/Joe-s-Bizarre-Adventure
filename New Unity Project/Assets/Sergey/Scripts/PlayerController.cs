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

    CharacterController controller;
    PlayerInfo info;

    Vector3 Movement;
	
	void Start () {
        controller = gameObject.GetComponent<CharacterController>();
        info = gameObject.GetComponent<PlayerInfo>();
        Movement = Vector3.zero;
	}

    float passedTime = 0F;
	
    public void Move (PlayerInput input) {
        passedTime += Time.deltaTime;
        if (passedTime < info.notWorkingTime)
            return;
        //Head
        controller.Move(Vector3.zero);
        isgrounded = trigger.triggered;

		//Movement.y = Mathf.Min(isgrounded, Movement.y);
		//
		if(!isgrounded){
            Movement.y -= Gravity;
        }else{
            Movement.y = 0;
        }


        rotationX += input.mouseX * info.sensitivityX;
        rotationY += input.mouseY * info.sensitivityY;
		rotationY = Mathf.Clamp(rotationY, info.minimumY, info.maximumY);

		transform.localEulerAngles = new Vector3(0, rotationX, 0);
		CameraY.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);


		
        Movement.Set(input.forward, Movement.y, input.right);
        controller.Move(transform.up * Movement.y * Time.fixedDeltaTime );
        controller.Move(transform.forward * input.forward * info.walkSpeed * Time.fixedDeltaTime);
		controller.Move(transform.right * input.right * info.walkSpeed * Time.fixedDeltaTime);


	}
	void OnCollisionEnter(Collision theCollision)
	{
		if (theCollision.gameObject.name == "floor")
		{
			isgrounded = true;
		}
	}

	//consider when character is jumping .. it will exit collision.
	void OnCollisionExit(Collision theCollision)
	{
		if (theCollision.gameObject.name == "floor")
		{
			isgrounded = false;
		}
	}

    public float LengthToGround(){
        Collider col = legCollider.GetComponent<Collider>();
        Bounds bounds = col.bounds;
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        List<Vector3> points = new List<Vector3>();

        points.Add(min);
        points.Add(new Vector3(min.x,min.y,max.z));
		points.Add(new Vector3(max.x, min.y, min.z));
        points.Add(new Vector3(max.x, min.y, max.z));


        float minLength = 1000;
        foreach(Vector3 pnt in points){
            Vector3 newPnt = pnt + Vector3.up * controller.skinWidth;
            Ray ray = new Ray(newPnt, newPnt + Vector3.down * 1);
            RaycastHit hit;
            if(Physics.Raycast(ray.origin,ray.direction,out hit)){
                minLength = Mathf.Min(minLength, hit.distance - controller.skinWidth );
                //print(hit.transform.name);
            }
        }

        return minLength;
    }

}
