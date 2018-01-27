using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour {

    public GameObject HandA;
    public GameObject HandB;

    private Hand  handA,handB;

    void Start(){
        handA = new Hand(HandA);
        handB = new Hand(HandB);
    }

    private bool handsHided;

    Animator animator;
    public void HandsController(Animator _animator){
        animator = _animator;
		if (HandsColliding())
		{
            if(!handsHided){
                HideHands();
            }

        }else if (handsHided){
            UnhideHands();
        }
    }

    void HideHands(){
        handsHided = true;
        animator.PlayInFixedTime("HideHands");
    }
    void UnhideHands(){
        handsHided = false;
        animator.PlayInFixedTime("UnhideHands");
        
    }

    public bool HandsColliding(){
        return handA.trigger.triggered || handB.trigger.triggered;
    }

}


public struct Hand{
    public GameObject gameObject;
    public СubeTrigger trigger;

    public Hand(GameObject hand){
        gameObject = hand;

        GameObject copy = GameObject.Instantiate(hand);
        copy.transform.SetParent(hand.transform.parent);
        copy.transform.localPosition = hand.transform.localPosition;

        GameObject.Destroy( copy.GetComponent<MeshRenderer>());

        copy.name = copy.name + "_Copy";
        BoxCollider coll = copy.AddComponent<BoxCollider>();
        coll.isTrigger = true;
        trigger = copy.AddComponent<СubeTrigger>();
    }

}