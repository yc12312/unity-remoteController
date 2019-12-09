using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class BlockAttack : MonoBehaviour {

    public Animator at;
    public AudioSource block;

    public static bool blocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "SWORD")
        {
            blocked = true;

            block.Play();

            if (NetworkServer.connections.Count > 0)
            {
                NetworkConnection connection = NetworkServer.connections[1];
                NetworkServerUI.OnServerConnect(connection);
            }

            at.SetBool("Blocked", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        blocked = false;
    }
}
