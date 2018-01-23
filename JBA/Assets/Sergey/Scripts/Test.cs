using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    CharacterController controller;

    float height, radius,delta;
    Vector3 center;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        height = controller.height;
        radius = controller.radius;

        delta = height - 2 * radius;
        center = controller.center;
    }

    private void OnDrawGizmos()
    {



    }
}
