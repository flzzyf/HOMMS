﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public Player[] players;

    void Awake()
    {
        if (PlayerManager.instance != this)
        {
            print("自我摧毁");
            Destroy(this);
        }
    }
}

[System.Serializable]
public class Player
{
    public int id;
    public bool isAI;

    public Vector2Int startingPoint;

	//玩家所有的英雄和城镇
    public List<Hero> heroes;
    public List<Town> towns;

	//资源
	public HOMMResource resources;
}
