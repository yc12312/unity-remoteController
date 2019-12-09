using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleHealthBar_SpaceshipExample;

public class Enemy : MonoBehaviour {

    private int hpVal;
    public SimpleHealthBar hp;

    public Animator at;
    public SphereCollider inRange;
    public CapsuleCollider Body;
    public AudioSource hit;

    // Use this for initialization
    void Start () {
        hpVal = 100;
	}

    private void OnTriggerEnter(Collider other)
    {
        //player is in range
        if (other.tag == "Player" && (other.gameObject.name != "Arrow_Small(Clone)"))
        {
            at.SetBool("Attack",true);
            inRange.enabled = false;
            Body.isTrigger = true;
        }

        //attacked by dagger
        if(other.gameObject.name == "Dagger" || other.gameObject.name == "Arrow_Small(Clone)")
        {
            if (!at.GetBool("Hit"))
            {
                hit.Play();

                at.SetBool("Hit", true);

                hpVal -= 10;
                hp.UpdateBar(hpVal, 100);
            }   
        }
        
        if(hpVal == 0)
        {
            //Dead
            at.SetBool("Dead", true);
        }
    }

}
