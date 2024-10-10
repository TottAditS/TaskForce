using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [Header("Music Setting")]
    private int temp;

    private void Start()
    {
        getmusic();
    }

    private void Update()
    {
        getmusic();
    }

    private void getmusic()
    {
        if (!AudioManager.instance.IsPlaying("Main-Menu-Theme"))
        {
            //Debug.Log("musik");
            AudioManager.instance.playM("Main-Menu-Theme");
        }

        if (!AudioManager.instance.IsPlaying("Ocean-SFX"))
        {
            //Debug.Log("air laut");
            AudioManager.instance.playS("Ocean-SFX");
        }
    }
}
