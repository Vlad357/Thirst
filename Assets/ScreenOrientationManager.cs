using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOrientationManager : MonoBehaviour
{
    private void Awake()
    {
        
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    private void Update()
    {
        
        if (Screen.orientation != ScreenOrientation.LandscapeLeft)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }
}
