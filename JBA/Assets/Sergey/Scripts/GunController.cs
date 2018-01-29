using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public Hands hands;


    public GameObject currentGun;
    public GameObject backpackGun;



    bool hasGunInHands = false;

    private void Start()
    {
        if(currentGun)
            currentGun.SetActive(false);
        if(currentGun)
            backpackGun.SetActive(false);
        PutCurrentGun();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)){
            SwapGuns();
        }
    }

    void PutCurrentGun()
	{
        if(currentGun){
            currentGun.SetActive(true);
            print(hands);
            currentGun.transform.SetParent(hands.HandR.transform);
            currentGun.transform.localPosition = Vector3.zero;
            currentGun.transform.localEulerAngles = Vector3.zero;

            hasGunInHands = true;
        }

	}

    void SwapGuns(){
        GameObject tmp = backpackGun;
        backpackGun = currentGun;
        currentGun = tmp;

        backpackGun.SetActive(false);

        PutCurrentGun();
    }
	
}
