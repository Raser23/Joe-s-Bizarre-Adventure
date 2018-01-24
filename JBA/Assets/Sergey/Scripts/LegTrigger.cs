using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegTrigger : MonoBehaviour {

    public bool triggered
    {
        get { return ins.Count > 0; }
    }

    public List<Collider> ins;

    void Start(){
        ins = new List<Collider>();
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Player")
		//{
            ins.Add(other);
		//}
           

	}
	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Floor")
		{
            
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.tag != "Player")
		//{
            ins.Remove(other);
		//}

	}
}
