using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : NodeObject
{
    public Vector2Int interactPoint;

	public Race race;

	//城镇内外的英雄
	public Hero hero_inside, hero_outside;
	//城内驻军（只有城内没英雄时才有
	public PocketUnit[] townUnits;

	//获取城内单位组，英雄或者守城单位
	public PocketUnit[] inTownUnits
	{
		get { return hero_inside != null ? hero_inside.pocketUnits : townUnits;  }
	}

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(interactPoint.x, 0, interactPoint.y), Vector3.one * 3);
    }
}
