using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public AudioSource audio_BGM;

    public void GameStart()
    {
        audio_BGM.Stop();

		LoadingManager.instance.LoadScene();
    }

    //退出游戏
    public void Quit()
    {
        Application.Quit();
    }
}
