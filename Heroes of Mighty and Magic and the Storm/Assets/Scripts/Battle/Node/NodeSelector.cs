using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSelector : MonoBehaviour
{
	//单位可到达的节点
	public static List<NodeItem> reachableNodes;
	//单位可攻击节点
	public static List<NodeItem> attackableNodes;
	//可施法节点
	public static List<NodeItem> spellableNodes;

	//获取单位可行动的节点
	public static void GetUnitActionNodes(Unit _unit)
	{
		reachableNodes = new List<NodeItem>();
		attackableNodes = new List<NodeItem>();

		int speed = _unit.GetComponent<Unit>().speed;

		//选出可到达节点，（判断是否是步行单位，是否是双格单位
		if(!_unit.type.isTwoHexsUnit)
		{
			foreach (NodeItem item in BattleManager.instance.map.GetNodeItemsWithinRange(_unit.nodeItem, speed, _unit.isWalker))
			{
				if (item.isEmpty)
					reachableNodes.Add(item);
			}
		}
		else
		{
			//是双格单位。先从源点选出行动范围，然后是前方的点
			foreach (NodeItem item in BattleManager.instance.map.GetNodeItemsWithinRange(_unit.nodeItem, speed, _unit.isWalker))
			{
				if (item.isEmpty)
					reachableNodes.Add(item);
			}
			//前方的点
			int offsetX = _unit.facingRight ? 1 : -1;
			Vector2Int pos = _unit.nodeItem.pos;
			pos.x += offsetX;
			foreach (NodeItem item in BattleManager.instance.map.GetNodeItemsWithinRange(BattleManager.instance.map.GetNodeItem(pos), speed, _unit.isWalker))
			{
				if (item.isEmpty && !reachableNodes.Contains(item))
					reachableNodes.Add(item);
			}
		}

		//选出可攻击节点
		//是近战，或者被近身的远程单位
		if (!_unit.IsRangeAttack)
		{
			//可攻击节点
			foreach (NodeItem item in BattleManager.instance.map.GetNodeItemsWithinRange(_unit.nodeItem, speed + 1, _unit.isWalker))
			{
				//是单位而且是敌对
				if (!item.isEmpty &&
					item.nodeObject.GetComponent<NodeObject>().nodeObjectType == NodeObjectType.unit &&
					!BattleManager.instance.isSamePlayer(item.nodeObject.GetComponent<Unit>(), _unit))
				{
					attackableNodes.Add(item);
				}
			}
		}
		else
		{
			//远程攻击，直接选中所有敌人，将节点类型设为可攻击
			int enemyHero = (_unit.side + 1) % 2;

			foreach (Unit item in BattleManager.instance.units[enemyHero])
			{
				attackableNodes.Add(item.nodeItem);
			}
		}
	}
	//高亮单位可行动节点
	public static void HighlightUnitActionNodes()
	{
		foreach (var item in reachableNodes)
		{
			item.GetComponent<NodeItem_Battle>().ChangeNodeType(BattleNodeType.reachable);
		}
		foreach (var item in attackableNodes)
		{
			item.GetComponent<NodeItem_Battle>().ChangeNodeType(BattleNodeType.attackable);
		}
	}
	//隐藏可行动节点
	public static void HideActionNodes()
	{
		foreach (var item in reachableNodes)
		{
			item.GetComponent<NodeItem_Battle>().ChangeNodeType(BattleNodeType.empty);
		}
		foreach (var item in attackableNodes)
		{
			item.GetComponent<NodeItem_Battle>().ChangeNodeType(BattleNodeType.empty);
		}
	}
	//获取并高亮可施法节点
	public static void GetSpellableNodes(MagicTargetFliter _fliter)
	{
		spellableNodes = new List<NodeItem>();

		List<Unit> targetUnits;
		//否则选择目标
		if (_fliter == MagicTargetFliter.All)
		{
			//选择所有单位
			targetUnits = BattleManager.instance.allUnits;
		}
		else
		{
			int side = (BattleManager.currentSide + (int)_fliter) % 2;

			targetUnits = BattleManager.instance.units[side];
		}

		foreach (Unit item in targetUnits)
		{
			//设置为可施法节点
			item.nodeItem.GetComponent<NodeItem_Battle>().ChangeNodeType(BattleNodeType.spellable);

			spellableNodes.Add(item.nodeItem);
		}
	}
	//隐藏可施法节点
	public static void HideSpellableNodes()
	{
		foreach (var item in spellableNodes)
		{
			item.GetComponent<NodeItem_Battle>().ChangeNodeType(BattleNodeType.empty);
		}
	}
}
