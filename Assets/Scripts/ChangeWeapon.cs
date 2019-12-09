//Use swipe on mobile to change weapon
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ChangeWeapon : MonoBehaviour
{

    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = true;

    public float SWIPE_THRESHOLD = 20f;

    public GameObject sword, sheild, arrow;

    private static List<GameObject> tools;

    public static int index;

    public TMPro.TMP_Text wpStat;

    private void Start()
    {
        //Creating list of tools
        tools = new List<GameObject>();
        index = 0;
        tools.Add(sword);
        tools.Add(sheild);
        tools.Add(arrow);

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                checkSwipe();
            }

        }

    }

    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            if (fingerDown.y - fingerUp.y > 2 )
            {
                swapWeapon(false);
            }
            else if (fingerDown.y - fingerUp.y < -2)
            {
                swapWeapon(true);
            }
            fingerUp = fingerDown;
        }

        //Check if Horizontal swipe
        else if(index == 2)
        {
            if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
            {
                if (fingerDown.x - fingerUp.x > 0 || fingerDown.x - fingerUp.x < 0)
                {
                    NetworkClinetUI.SendFireInfo();

                }
                fingerUp = fingerDown;
            }
                
        }
        
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }
    
    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }
    

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////
    private void swapWeapon(bool up)
    {
        int prevIndex = index;

        if (up)
        {
            index++;
            if (index > 2)
            {
                index = 0;
            }
            tools[prevIndex].SetActive(false);
            tools[index].SetActive(true);
            NetworkClinetUI.SendUpSwapInfo();
        }
        else
        {
            index--;
            if (index <0)
            {
                index = 2;
            }
            tools[prevIndex].SetActive(false);
            tools[index].SetActive(true);
            NetworkClinetUI.SendDownSwapInfo();
        }

        if (index == 0)
        {
            wpStat.text = "Sword";
        }
        else if (ChangeWeapon.index == 1)
        {
            wpStat.text = "Sheild";
        }
        else
        {
            wpStat.text = "CrossBow";
        }
    }
}
