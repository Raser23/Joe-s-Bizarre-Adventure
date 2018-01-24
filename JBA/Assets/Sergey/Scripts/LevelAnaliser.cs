using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAnaliser : MonoBehaviour {

    public LayerMask mask;

    void OnDrawGizmos(){
        Plane cast = Cast();

        Gizmos.color = Color.magenta;

        //Gizmos.DrawLine(transform.position,cast.pnt);
        Gizmos.DrawRay(cast.pnt, cast.Image(transform.forward));


	}

    public Plane Cast(){
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray.origin,ray.direction,out hit,1000,mask)){

            return new Plane(hit);

        }
        return new Plane(Vector3.zero, Vector3.zero);

    }



}

public struct Plane{
    public Vector3 pnt;
    public Vector3 normal;

    public Plane(RaycastHit hit){
        pnt = hit.point;
        normal = hit.normal;
    }
    public Plane(Vector3 _pnt, Vector3 _normal)
	{
        pnt = _pnt;
        normal = _normal    ;
	}
	Vector3 f(Vector3 v1, Vector3 v2)
	{
		return new Vector3(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v2.z * v1.x, v1.x * v2.y - v1.y * v2.x);
	}
    public Vector3 Image(Vector3 v){
        return f(normal,f(v, normal));
    }
}
