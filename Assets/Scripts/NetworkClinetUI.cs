using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkClinetUI : MonoBehaviour {

    static NetworkClient client;

    public TMPro.TMP_InputField input;

    public GameObject Y, N;

    public GameObject ConnectCanvas, ControlCanvas;

    // Use this for initialization
    void Start () {
        client = new NetworkClient();

        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        isConnect();
        if (!ConnectCanvas.activeSelf)
        {
            //Sending Gyro Info
            SendGyroInfo();
            //Read data comes from server
            OnClientConnect(client.connection);
        }

    }

    public void isConnect()
    {
        if (client.isConnected)
        {
            Y.SetActive(true);
            N.SetActive(false);
            Forward();
        }
        else
        {
            Back();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        //Change Canvas
        ConnectCanvas.SetActive(true);
        ControlCanvas.SetActive(false);
    }

    public void BackDisConnect()
    {
        SendQuitInfo();

        //Change Canvas
        ConnectCanvas.SetActive(true);
        ControlCanvas.SetActive(false);
        
        
    }


    private void Forward()
    {
        //Change Canvas
        ConnectCanvas.SetActive(false);
        ControlCanvas.SetActive(true);
    }

    public void Connect()
    {
        if (!client.isConnected)
        {
            
            client.Connect(input.text, 25000);

        }

    }

    public void initWrapper()
    {
        SendInitInfo();
    }

    //Init gyro
    static public void SendInitInfo()
    {
        if (client.isConnected)
        {
            IntegerMessage msg = new IntegerMessage(0);
     
            client.Send(889, msg);

        }
    }

    //Notify swap
    static public void SendDownSwapInfo()
    {
        if (client.isConnected)
        {
            IntegerMessage msg = new IntegerMessage(1);

            client.Send(889, msg);

        }
    }

    static public void SendUpSwapInfo()
    {
        if (client.isConnected)
        {
            IntegerMessage msg = new IntegerMessage(2);

            client.Send(889, msg);

        }
    }

    //Notify fire
    static public void SendFireInfo()
    {
        if (client.isConnected)
        {
            IntegerMessage msg = new IntegerMessage(3);
            client.Send(889, msg);

        }
    }

    //Notify swap
    static public void SendQuitInfo()
    {
        if (client.isConnected)
        {
            IntegerMessage msg = new IntegerMessage(4);

            client.Send(889, msg);

        }
    }

    //Send Gyro Input
    static public void SendGyroInfo()
    {
        Vector3 vec = Input.gyro.rotationRateUnbiased;

        if (client.isConnected)
        {
            StringMessage msg = new StringMessage();
            msg.value = vec.x + "|" + vec.y + "|" + vec.z;
            client.Send(888, msg);

        }
    }

    //[Receive from server]
    public class RegisterHostMessage : MessageBase
    {
        public bool vib;
    }

    public bool received;

    public const short m_MessageType = MsgType.Highest + 1;

    public void OnClientConnect(NetworkConnection connection)
    {
        //Register and receive the message on the Client's side
        client.RegisterHandler(m_MessageType, ReceiveMessage);
    }

    public void ReceiveMessage(NetworkMessage networkMessage)
    {
        //Read the message that comes in
        RegisterHostMessage hostMessage = networkMessage.ReadMessage<RegisterHostMessage>();
        //Store the name and comment as variables
        received = hostMessage.vib;

        if (received)
        {
            Handheld.Vibrate();
        }
    }

    
}
