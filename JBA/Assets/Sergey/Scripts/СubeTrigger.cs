using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class СubeTrigger : MonoBehaviour {

	public bool triggered
	{
		get
		{
			return ins.Count > 0;
		}
	}

    //ee
	public List<Collider> ins;

	void Start()
	{
		ins = new List<Collider>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Player" && !other.isTrigger)
			ins.Add(other);


	}
	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Floor")
		{

		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.tag != "Player"&& !other.isTrigger)
			//{
			ins.Remove(other);
		//}

	}
}
