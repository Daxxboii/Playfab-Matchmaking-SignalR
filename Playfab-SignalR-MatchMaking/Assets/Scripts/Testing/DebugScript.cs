using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{

    public bool isDebug;
   void OnGUI()
    {
        if (isDebug)
        {
            if (GUI.Button(new Rect(20, 0, 150, 30), "Login as TestUser 1"))
            {
                //PlayFabManager.instance.SignUp("UserOne@gmail.com", "User123/","UserOne");
                PlayFabManager.instance.Login("UserOne@gmail.com", "User123/");
            }
            if (GUI.Button(new Rect(20, 50, 150, 30), "Login as TestUser 2"))
            {
               // PlayFabManager.instance.SignUp("UserTwo@gmail.com", "User123/","UserTwo");
                PlayFabManager.instance.Login("UserTwo@gmail.com", "User123/");
            }
            if (GUI.Button(new Rect(20, 100, 150, 30), "Login as TestUser 3"))
            {
                //PlayFabManager.instance.SignUp("UserThree@gmail.com", "User123/","UserThree");
                PlayFabManager.instance.Login("UserThree@gmail.com", "User123/");
            }

           

          /*  if (GUI.Button(new Rect(20, 150, 150, 30), "Login as Other"))
            {
            }*/
        }
    }
}
