using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {


    public GameObject from, to;

    public float width;

    public int steps;

    public GameObject body;

    public GameObject handler;

    List<GameObject> createdCubes;
	
    public void Build(){
        if(handler != null){
            DestroyImmediate(handler);
        }
        handler = new GameObject("Handler");
        handler.transform.SetParent(transform);

        createdCubes = new List<GameObject>();

		body = CreateBody(from.transform.position, to.transform.position, width);


		Vector3 direction = to.transform.position - from.transform.position;
        Vector3 t = new Vector3(-direction.z, 0, direction.x).normalized;


        CreateCubeAt(from.transform.position);
        CreateCubeAt(to.transform.position);
        Vector3 r1 = from.transform.position + t * width / 2;
        Vector3 r2 = to.transform.position + t * width / 2;
        Vector3 l1 = from.transform.position - t * width / 2;
        Vector3 l2 = to.transform.position - t * width / 2;

        CreateLineAt(r1, r2);
		CreateLineAt(l1, l2);


        for (int i = 0; i <= steps+1; i++) {
            Vector3 curP = from.transform.position + direction * (i ) / (steps + 1);
            Vector3 c1 = curP + width / 2 * t;
			Vector3 c2 = curP - width / 2 * t;

            CreateLineAt(c1, c2);
        }

        foreach(GameObject cube in createdCubes){
            
            //Vector3 sc = handler.transform.localScale;
            cube.transform.SetParent(handler.transform);
            //cube.transform.localScale = sc;
        }
        //CreateBody(from.transform.position, to.transform.position,width);
	}

    GameObject CreateCubeAt(Vector3 position){
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        createdCubes.Add(cube);

        DestroyImmediate(cube.GetComponent<Collider>());
        cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		
        cube.transform.SetParent(transform);

		cube.transform.position = position;

        return cube;
    }

    GameObject CreateBody(Vector3 from, Vector3 to, float width){
        GameObject resbody = CreateLineAt(from, to);
        resbody.AddComponent<BoxCollider>();


        Transform prnt = resbody.transform.parent;
        resbody.transform.SetParent(null);
        resbody.transform.localScale = new Vector3(width, 0.1f, resbody.transform.localScale.z);
        resbody.transform.SetParent(prnt);


        DestroyImmediate(resbody.GetComponent<Renderer>());
        Rigidbody rig =  resbody.AddComponent<Rigidbody>();
        rig.useGravity = false;
        resbody.transform.tag = "Floor";
        rig.isKinematic = true;
        return resbody;

    }

    GameObject CreateLineAt(Vector3 start,Vector3 end){
        GameObject cube = CreateCubeAt((start + end) / 2);

        Vector3 dir = end - start;

        Transform prnt = cube.transform.parent;
        cube.transform.SetParent(null);
		cube.transform.localScale = new Vector3(0.2f , 0.2f , dir.magnitude );

		cube.transform.SetParent(prnt);

        cube.transform.rotation = Quaternion.LookRotation(dir);

        return cube;

    }

	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        //Gizmos.DrawLine(from.transform.position,to.transform.position);

        Vector3 direction = to.transform.position - from.transform.position;

        Vector3 t = new Vector3(-direction.z,0,direction.x);
        t = t.normalized;

        Gizmos.DrawLine(from.transform.position, from.transform.position + t * width/2);
		Gizmos.DrawLine(from.transform.position, from.transform.position - t * width / 2);

		Gizmos.DrawLine(to.transform.position, to.transform.position + t * width / 2);
		Gizmos.DrawLine(to.transform.position, to.transform.position - t * width / 2);

		Gizmos.DrawLine(from.transform.position + t * width / 2, to.transform.position + t * width / 2);
		Gizmos.DrawLine(from.transform.position - t * width / 2, to.transform.position - t * width / 2);



	}
}
