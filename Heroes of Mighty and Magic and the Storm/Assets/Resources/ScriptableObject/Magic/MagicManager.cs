using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManager : Singleton<MagicManager>
{
	//玩家当前准备释放的魔法
	public static Magic currentMagic;
	[HideInInspector]
	public List<NodeItem> spellableNodes;

    //准备释放魔法
    public void PrepareMagic(Hero _hero, Magic _magic)
    {
        //判定魔法学派，根据英雄的相应学派等级释放不同效果
		int magicLevel = _magic.GetMagicLevel(_hero);

        MagicType type = _magic.type;

        if (type == MagicType.Battle)
        {

        }

		currentMagic = _magic;

		//目标类型：无目标直接释放
		int targetType = Mathf.Min(_magic.targetType.Length - 1, magicLevel);
		//int effect = Mathf.Min(_magic.effects.Length - 1, magicLevel);
		if (_magic.targetType[targetType] == MagicTargetType.Null)
		{
			CastMagic();
		}
		else
		{
			MagicTargetFliter fliter = _magic.targetFliter[Mathf.Min(_magic.targetFliter.Length - 1, magicLevel)];
			List<Unit> targetUnits;
			//否则选择目标
			if (fliter == MagicTargetFliter.All)
			{
				//选择所有单位
				targetUnits = BattleManager.instance.allUnits;
			}
			else
			{
				int side = (BattleManager.currentSide + (int)fliter) % 2;

				targetUnits = BattleManager.instance.units[side];
			}

			foreach (Unit item in targetUnits)
			{
				item.nodeItem.GetComponent<NodeItem_Battle>().ChangeNodeType(BattleNodeType.spellable);

				spellableNodes.Add(item.nodeItem);
			}
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

		int effect = Mathf.Min(currentMagic.effects.Length - 1, magicLevel);
		currentMagic.effects[effect].originPlayer = BattleManager.currentSide;
		//释放魔法
		if (_nodeitem == null)
		{
			//无目标直接释放
			currentMagic.effects[effect].Invoke();
		}
		else
		{
			if (currentMagic.targetType[Mathf.Min(currentMagic.targetType.Length - 1, magicLevel)] == MagicTargetType.Node)
			{
				currentMagic.effects[effect].targetNode = _nodeitem;
			}
			else
			{
				currentMagic.effects[effect].targetUnit = _nodeitem.nodeObject.GetComponent<Unit>();
			}
			currentMagic.effects[effect].Invoke();
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
