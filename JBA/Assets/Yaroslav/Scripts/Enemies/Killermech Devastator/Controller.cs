using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	public Vector3 player_pos, delta_rot, beam_end;
	public GameObject player, charge;

	public float time_in_status, charging_time, firing_time;

	private bool lockRotation;

	public enum States { charging, firing, inactive, cooldown };

	public States status;
	
	public LineRenderer beam;


	[System.Serializable]
	public struct RotationSettings{
		public float rot_speed;
		public Vector3 look_dir;
		public Transform obj;
		public RotationSettings(float s, Vector3 ld, Transform o){
			rot_speed = s;
			look_dir = ld;
			obj = o;
		}
	}

	public RotationSettings head, body;

	

	void Start () {
		head.look_dir = head.obj.right*(-1);
		body.look_dir = body.obj.right*(-1);
		lockRotation = false;
	}

	void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(charge.transform.position, body.obj.forward*10000);
    }
	
	void FollowObject(Vector3 obj_pos, RotationSettings rs ){
		float step = rs.rot_speed * Time.deltaTime;
		Vector3 target_dir = (obj_pos - rs.obj.position).normalized;
		target_dir.y = 0;
		Quaternion target_rot = Quaternion.LookRotation(target_dir);
		rs.obj.rotation = Quaternion.Slerp(rs.obj.rotation, target_rot, step);
	}

	Vector3 GetPlayerPosition(){
		return player.transform.position;
	}

	void StartFiring(){
		RaycastHit fire_point;
		if(Physics.Raycast(charge.transform.position, body.obj.forward, out fire_point, 1000.0f)){
			beam.positionCount = 2;
			beam_end = fire_point.point;
			beam.SetPosition(0, charge.transform.position);
			beam.SetPosition(1, beam_end);
		}
	}

	void StopFiring(){
		beam.positionCount = 0;
	}

	void Update () {
		player_pos = GetPlayerPosition();
		FollowObject(player_pos, head);
		if(!lockRotation){
			FollowObject(player_pos, body);
		}
		switch(status){
			case States.inactive:
				status = States.charging;
				time_in_status = 0;
				break;
			case States.charging:
				charge.SetActive(true);
				if( time_in_status < charging_time ){
					if(charging_time - time_in_status < 0.5) lockRotation = true;
					time_in_status += Time.deltaTime;
					charge.transform.localScale = new Vector3(1,1,1) * (time_in_status / charging_time);
				}
				else{
					status = States.firing;
					time_in_status = 0;
				}
				break;
			case States.firing:
				if(time_in_status < firing_time){
					time_in_status += Time.deltaTime;
					StartFiring();
					charge.transform.localScale = new Vector3(1f, 1f, 1f) * (1-(time_in_status / firing_time));
				}
				else{
					status = States.inactive;
					time_in_status = 0;
					charge.SetActive(false);
					lockRotation = false;
					StopFiring();
				}
				break;
		}
	}
}
