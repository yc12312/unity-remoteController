using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

public class ServerConnectionManager : MonoBehaviour {

    public TMPro.TMP_Text ip, stat;

    public GameObject Contents;
    public GameObject Canvas;

	// Use this for initialization
	void Start () {
        setup();
    }
	
	// Update is called once per frame
	void Update () {
        ConnectionCheck();
    }

    public void setup()
    {
        ip.text = Network.player.ipAddress;
    }

    private void ConnectionCheck()
    {
        //status setup
        bool status = NetworkServer.active;
        if (status)
        {
            stat.text = "Alive";
        }
        else
        {
            stat.text = "Dead";
        }

        if (NetworkServer.connections.Count == 2)
        {
            //Disable canvas
            Contents.SetActive(true);
            Canvas.SetActive(false);
        }
    }
}
