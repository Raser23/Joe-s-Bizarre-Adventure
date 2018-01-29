using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour {

    public GameObject HandA;
    public GameObject HandB;

    private Hand  handA,handB;

    private List<Hand> hands;

    public List<GameObject> guns;
    void Awake(){
        handA = new Hand(HandA,"1");
        handB = new Hand(HandB,"2");
        hands = new List<Hand>(){handA,handB};

        PutCurrentGun();
    }

    private bool handsHided;

    Animator animator;

    void PutCurrentGun(){
        GameObject currentGun = guns[0];
        currentGun.transform.SetParent(handB.gameObject.transform);
        currentGun.transform.localPosition = Vector3.zero;
        currentGun.transform.localEulerAngles = Vector3.zero;



	}

    public void HandsController(Animator _animator){
        animator = _animator;

        for (int i = 0; i < hands.Count;i++)
        {
            Hand hand = hands[i];
            if (hand.trigger.triggered)
            {
                //print(hand.hided);
                if (!hand.hided)
                {
                    HideHand(hand);
                }
            }else if (hand.hided){
                UnhideHand(hand);
            }
        }
		/*if (HandsColliding())
		{
            if(!handsHided){
                HideHands();
            }

        }else if (handsHided){
            UnhideHands();
        }*/
    }

    void HideHand(Hand hand){
        hand.Hide();
        animator.PlayInFixedTime("HideHand"+hand.postfix);
    }
    void UnhideHand( Hand hand){
        hand.Unhide();
        animator.PlayInFixedTime("UnhideHand"+ hand.postfix);
        
    }

    public bool HandsColliding(){
        return handA.trigger.triggered || handB.trigger.triggered;
    }

}


public class Hand{
    public GameObject gameObject;
    public СubeTrigger trigger;

    public bool hided;
    public string postfix;

    public void Hide(){
        hided = true;
    }
	public void Unhide()
	{
        hided = false;
	}


	public Hand(GameObject hand,string st){
        postfix = st;
        Unhide();
        this.gameObject = hand;

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