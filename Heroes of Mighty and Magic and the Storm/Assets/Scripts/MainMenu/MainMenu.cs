using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void GameStart()
    {
		LoadingManager.instance.LoadScene();
    }

    //退出游戏
    public void Quit()
    {
        Application.Quit();
    }
}
