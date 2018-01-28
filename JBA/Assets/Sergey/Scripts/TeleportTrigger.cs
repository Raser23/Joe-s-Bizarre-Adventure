using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour {

	public bool triggered
	{
		get
		{
			return ins.Count > 0;
		}
	}

    public Types currentType;
    public LayerMask mask;

	
	public List<Collider> ins;

	void Start()
	{
		ins = new List<Collider>();
	}

	void OnTriggerEnter(Collider other)
	{
        if (Matches(other))
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
        if (ins.Contains(other))
        {
            ins.Remove(other);
        }
    }

    bool Matches(Collider col){
        bool result = false;
        switch (currentType)
        {
            case Types.OnlyPlayer: result = col.name == "Player"; break;
            case Types.ByLayer:result = (mask == (mask | (1 << col.gameObject.layer))); break;
            case Types.ListOfTags: result = false;break;
        }

        return result;
    }

   
}
public enum Types { OnlyPlayer, ByLayer, ListOfTags };