using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrow : MonoBehaviour {

    private GameObject prefab;
    private Quaternion result;

    public AudioSource fire;

    // Use this for initialization
    void Start () {
        prefab = Resources.Load("Arrow_Small") as GameObject;
    }
    
    private Vector3 SetRotation(Vector3 org)
    {
        org.x += 90;
        org.y -= 90;

        return org;
    }

    public void Fire()
    {
        fire.Play();

        GameObject bullet = Instantiate(prefab) as GameObject;

        bullet.transform.position = transform.position + transform.forward * 2;

        result.eulerAngles = SetRotation(transform.rotation.eulerAngles);

        bullet.transform.rotation = result;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = transform.forward *40;

        Destroy(bullet.gameObject, 2);
    }

}
