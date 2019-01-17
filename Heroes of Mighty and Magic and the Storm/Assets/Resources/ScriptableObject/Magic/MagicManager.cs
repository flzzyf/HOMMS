using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManager : Singleton<MagicManager>
{
	//玩家当前准备释放的魔法
	public static Magic currentMagic;

    //准备释放魔法
    public void PrepareMagic(Hero _hero, Magic _magic)
    {
        //判定魔法学派，根据英雄的相应学派等级释放不同效果
		int magicLevel = _magic.GetMagicLevel(_hero);

        MagicType type = _magic.type;

        if (type == MagicType.Battle)
        {

        }

        //print(SchoolToSkill(school));


		//目标类型：无目标直接释放
		int targetType = Mathf.Min(_magic.targetType.Length - 1, magicLevel);
		int effect = Mathf.Min(_magic.effects.Length - 1, magicLevel);
		if (_magic.targetType[targetType] == MagicTargetType.Null)
		{
			CastMagic();
		}

	}

	//释放魔法
	public void CastMagic(NodeItem _nodeitem = null)
	{
		int magicLevel = currentMagic.GetMagicLevel(BattleManager.currentHero);
		int mana = currentMagic.GetManaCost(BattleManager.currentHero);
		//扣除魔法值
		BattleManager.currentHero.mana -= mana;

		//界面暂停

		//释放魔法
		if(currentMagic.targetType[magicLevel] == MagicTargetType.Null)
		{
			//无目标直接释放
			int effect = Mathf.Min(currentMagic.effects.Length - 1, magicLevel);
			currentMagic.effects[effect].originPlayer = BattleManager.currentSide;
			currentMagic.effects[effect].Invoke();
		}
		else
		{
			//有目标，则选择目标

		}
	}

	//给英雄添加魔法
	public static void AddMagic(Hero _hero, Magic _magic)
    {
        //如果没有该魔法才添加
        if (!_hero.magics.Contains(_magic))
            _hero.magics.Add(_magic);
    }

    //给英雄添加所有魔法
    public static void AddAllMagic(Hero _hero)
    {
        Magic[] magic = Resources.LoadAll<Magic>("ScriptableObject/Magic/Instance");
        foreach (Magic item in magic)
        {
            _hero.magics.Add(item);
        }
    }

    public static Magic GetMagic(string _name)
    {
        Magic[] magic = Resources.LoadAll<Magic>("ScriptableObject/Magic/Instance");
        foreach (Magic item in magic)
        {
            if (item.name == _name)
                return item;
        }

        return null;
    }

}
