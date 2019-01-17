using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelector
{
	static List<NodeItem> targetNodes;

	//获取目标节点组
	public static List<NodeItem> SelectTarget(int _player, TargetFilter _filter, NodeItem _origin = null, int _range = 0)
    {
        targetNodes = new List<NodeItem>();

		if(_origin != null)
			targetNodes = BattleManager.instance.map.GetNodeItemsWithinRange(_origin, _range, false);

		//目标类型为单位
		if (_filter.targetType == TargetType.Unit)
        {
			List<Unit> targetUnits;

			if (_filter.allyOrEnemy == AllyOrEnemy.All)
				targetUnits = BattleManager.instance.allUnits;
			else
			{
				//目标为玩家单位
				int side = (_player + (int)_filter.allyOrEnemy) % 2;

				targetUnits = BattleManager.instance.units[side];
			}
			//如果源点存在
			if (_origin != null)
			{
				//移除不在目标单位组的目标节点
				NodeObject obj;
				for (int i = targetNodes.Count - 1; i > 0; i--)
				{
					obj = targetNodes[i].nodeObject;
					if (obj == null || obj.GetComponent<NodeObject>().nodeObjectType == NodeObjectType.unit ||
						!targetUnits.Contains(obj.GetComponent<Unit>()))
						targetNodes.RemoveAt(i);
				}
			}
			else
			{
				//否则直接返回这些单位的所在点
				foreach (Unit item in targetUnits)
				{
					targetNodes.Add(item.nodeItem);
				}
			}
        }
		else
		{
			//目标类型为节点
		}

		return targetNodes;
    }
	//选出可到达节点（飞行单位walkable为true）
	public static List<NodeItem> SelectWalkableNodes(NodeItem _origin, int _range, bool _walkable = false)
	{
		targetNodes = BattleManager.instance.map.GetNodeItemsWithinRange(_origin, _range, _walkable);
		return targetNodes;
	}
}

public enum AllyOrEnemy { All, Ally = 0, Enemy = 1 }
public enum TargetType { Null, Unit, Node }
public class TargetFilter
{
	//public TargetFilter(AllyOrEnemy _allyOrEnemy, )

    public AllyOrEnemy allyOrEnemy;
    public TargetType targetType;
}