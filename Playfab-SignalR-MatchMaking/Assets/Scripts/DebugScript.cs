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
            if (GUI.Button(new Rect(20, 0, 150, 30), "Login as Daxx"))
            {
              //  PlayFabManager.instance.SignUp("dhakaddaksh123@gmail.com", "Daksh123/","Daxx");
                 PlayFabManager.instance.Login("dhakaddaksh123@gmail.com", "Daksh123/");
            }

            if (GUI.Button(new Rect(20, 50, 150, 30), "Login as MyBoy"))
            {
               // PlayFabManager.instance.SignUp("MyBoy69@gmail.com", "MyBoy123","MyBoy");
                 PlayFabManager.instance.Login("MyBoy69@gmail.com", "MyBoy123");
            }

            if (GUI.Button(new Rect(20, 100, 150, 30), "Login as JP00"))
            {
                //PlayFabManager.instance.SignUp("jpGord00@gmail.com", "JP123/","JP00");
                PlayFabManager.instance.Login("jpGord00@gmail.com", "JP123/");
            }

          /*  if (GUI.Button(new Rect(20, 150, 150, 30), "Login as Other"))
            {
            }*/
        }
    }
}
