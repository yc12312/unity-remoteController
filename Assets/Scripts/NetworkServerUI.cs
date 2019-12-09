using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;



public class NetworkServerUI : MonoBehaviour
{

    public GameObject Object;

    private Vector3 sword_init = new Vector3(-90, 90, 0);
    private Vector3 sheild_init = new Vector3(-90, 0, 0);
    private Vector3 arrow_init = new Vector3(0, 0, 0);

    private StringMessage Gyromsg;
    private IntegerMessage Othermsg;

    public GameObject sword,sheild,arrow, arrowrapper;

    private static List<GameObject> tools;
    public static int index;

    public FireArrow fa;

    public GameObject Contents;
    public GameObject Canvas;

    /*
    void OnGUI()
    {
        string ipaddress = Network.player.ipAddress;
        GUI.Box(new Rect(10, Screen.height - 50, 100, 50), ipaddress);
        GUI.Label(new Rect(20, Screen.height - 35, 100, 20), "Status: " + NetworkServer.active);
        GUI.Label(new Rect(20, Screen.height - 20, 100, 20), "Status: " + NetworkServer.connections.Count);
    }
    */

    private void Start()
    {
        //Creating list of tools
        tools = new List<GameObject>();
        index = 0;
        tools.Add(sword);
        tools.Add(sheild);
        tools.Add(arrowrapper);

        Object = tools[index];

        Application.targetFrameRate = 60;

        NetworkServer.Listen(25000);
        NetworkServer.RegisterHandler(888, ServerGyroReceiveMessage);
        NetworkServer.RegisterHandler(889, ServerOtherReceiveMessage);
    }

    private void Update()
    {
        if(NetworkServer.connections.Equals(null))
        {
            //Enable canvas
            Contents.SetActive(false);
            Canvas.SetActive(true);
        }
    }


    private void ServerOtherReceiveMessage(NetworkMessage mesage)
    {
        Othermsg = new IntegerMessage();
        Othermsg.value = mesage.ReadMessage<IntegerMessage>().value;

        //Do init
        if (Othermsg.value == 0)
        {
            initWeapon();
            return;
        }

        //Do swap
        if (Othermsg.value == 1 || Othermsg.value == 2)
        {
            swapWeapon(Othermsg.value);
            return;
        }

        //Do fire arrow
        if (Othermsg.value == 3)
        {
            fa.Fire();
            return;
        }
        if (Othermsg.value == 4)
        {
            Exit();
        }
    }

    public void Exit()
    {
        NetworkServer.Reset();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        return;
    }

    private void ServerGyroReceiveMessage(NetworkMessage mesage)
    {

        Gyromsg = new StringMessage();
        Gyromsg.value = mesage.ReadMessage<StringMessage>().value;

        //move accord to gyro info
        string[] gyros = Gyromsg.value.Split('|');

        if(index == 2)
        {
            //arrow
            Object.gameObject.transform.Rotate(float.Parse(gyros[2]), -float.Parse(gyros[1]), float.Parse(gyros[0]));
        }
        else if(index == 1)
        {
            //sheild
            Object.gameObject.transform.Rotate(float.Parse(gyros[2]), -float.Parse(gyros[0]), -float.Parse(gyros[1]));
        }
        else
        {
            //sword
            Object.gameObject.transform.Rotate(-float.Parse(gyros[0]), -float.Parse(gyros[2]), -float.Parse(gyros[1]));
        }

    }

    private void swapWeapon(int type)
    {

        int prevIndex = index;

        if (type == 2)
        {
            index++;
            if (index > 2)
            {
                index = 0;
            }
            tools[prevIndex].SetActive(false);
            tools[index].SetActive(true);
        }
        else
        {
            index--;
            if (index < 0)
            {
                index = 2;
            }
            tools[prevIndex].SetActive(false);
            tools[index].SetActive(true);
        }

        Object = tools[index];

        initWeapon();
    }

    private void initWeapon()
    {
        if(index == 2)
        {
            Object.gameObject.transform.eulerAngles = arrow_init;
        }
        else if(index == 1)
        {
            Object.gameObject.transform.eulerAngles = sheild_init;
        }
        else
        {
            Object.gameObject.transform.eulerAngles = sword_init;
        }
    }

    //[Send data from server to client]
    static RegisterHostMessage m_Message;
    public const short m_MessageType = MsgType.Highest + 1;

    //Creating message format
    public class RegisterHostMessage : MessageBase
    {
        public bool vib;
    }

    private static void EditMessage()
    {
        m_Message = new RegisterHostMessage();
        //Change the message name and comment to be the ones you set

        m_Message.vib = true;
    }

    public static void OnServerConnect(NetworkConnection connection)
    {
        //Change the message to read the Player's connection ID and a comment
        EditMessage();
        //Send the new message to the Client using the Server
        NetworkServer.SendToClient(connection.connectionId, m_MessageType, m_Message);
    }

}

