using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleHealthBar_SpaceshipExample;

public class Player : MonoBehaviour {

    private int hpVal;
    public SimpleHealthBar hp;
    public AudioSource hurt;

    public Shake sk;

    public GameObject bloodEffect;
    private GameObject temp;

    public GameObject Dead;

    void Start()
    {
        hpVal = 100;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SWORD" && BlockAttack.blocked == false)
        {
            //hurt sound play
            hurt.Play();

            //effect
            temp = Instantiate(bloodEffect.gameObject, bloodEffect.transform.position, Quaternion.identity);
            Destroy(temp, 1.0f);
            //Update hp bar
            hpVal -= 10;
            hp.UpdateBar(hpVal, 100);

            //Do camera shake
            StartCoroutine(sk.CameraShake(0.15f, 0.1f));
        }

        if (hpVal == 0)
        {
            //Dead
            Dead.SetActive(true);
        }
    }

}
