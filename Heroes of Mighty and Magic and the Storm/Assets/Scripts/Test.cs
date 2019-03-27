using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	public List<Hero> heroes;
	public List<Hero> heroes2;
	public List<Town> towns;

    void Start()
    {
		//给测试英雄加上所有魔法
		MagicManager.AddAllMagic(heroes[0]);

		//AdvantureObjectMgr.CreateAdvantureObject("Chest", new Vector2Int(23, 19));
		//AdvantureObjectMgr.CreateAdvantureObject("Leorics", new Vector2Int(19, 17));
		//AdvantureObjectMgr.CreateAdvantureObject("Chest", new Vector2Int(23, 14));
		//AdvantureObjectMgr.CreateAdvantureObject("Gold", new Vector2Int(19, 14));
		//AdvantureObjectMgr.CreateAdvantureObject("Gold", new Vector2Int(17, 14));
		//AdvantureObjectMgr.CreateAdvantureObject("Gold", new Vector2Int(15, 14));

		SkillManager.AddSkill(heroes[0], "Magic_Air", 2);
		SkillManager.AddSkill(heroes[0], "Magic_Fire", 2);
		SkillManager.AddSkill(heroes[0], "Magic_Earth", 2);
		SkillManager.AddSkill(heroes[0], "Magic_Water", 2);

		StartCoroutine(DelayStart());
    }

    void Update()
    {

		if (Input.GetKeyDown(KeyCode.F))
		{
			Hero attacker = heroes2[0];
			Hero defender = heroes2[1];
			TravelManager.instance.BattleBegin(attacker, defender);
		}
	}

	IEnumerator DelayStart()
	{
		yield return new WaitForSeconds(.1f);

		RealStart();
	}

	void RealStart()
	{
		//添加测试英雄和城镇，然后刷新
		foreach (var item in heroes)
		{
			GameManager.instance.localPlayer.heroes.Add(item);
		}

		TravelManager.instance.sliderItemManager_hero.MoveToPage(0);


		foreach (var item in towns)
		{
			GameManager.instance.localPlayer.towns.Add(item);
		}

		TravelManager.instance.sliderItemManager_town.MoveToPage(0);
	}
}
