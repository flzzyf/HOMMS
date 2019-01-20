using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager_Battle : MapManager
{
    public float nodeRadius = 1;

    //相邻节点偏移，顺序为从右上开始的顺时针
    static Vector2Int[,] nearbyNodeOffset = {
        {   new Vector2Int(1, -1),
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1)
        },

        {   new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, -1)
        }
    };

    public override void GenerateMap()
    {
        nodeSize.x = nodeRadius * 2;
        nodeSize.y = nodeRadius / 1.73f * 3;

        base.GenerateMap();
    }

    public override Vector3 NodeInit(int _x, int _y)
    {
        base.NodeInit(_x, _y);

        //偶数行，偏移一点
        if (_y % 2 == 0)
        {
            pos.x += nodeSize.x / 2;
        }

        pos.y *= -1;

        return pos;
    }

    //获取相邻的某个节点
    public Node GetNearbyNode(Node _node, int _index)
    {
        //奇偶行特殊处理
        int sign = _node.pos.y % 2 == 0 ? 0 : 1;

        Vector2Int offset = _node.pos + nearbyNodeOffset[sign, _index];

        if (isNodeAvailable(offset))
            return GetNode(offset);
        return null;
    }

    public NodeItem GetNearbyNodeItem(NodeItem _go, int _index)
    {
        Node node = GetNearbyNode(GetNode(_go.pos), _index);
        if (node != null)
        {
            return GetNodeItem(node.pos);
        }
        return null;
    }

    //获取周围节点
    public override List<Node> GetNearbyNodes(Node _node)
    {
        List<Node> list = new List<Node>();

        for (int i = 0; i < 6; i++)
        {
            if (GetNearbyNode(_node, i) != null)
                list.Add(GetNearbyNode(_node, i));
        }

        return list;
    }

    //鼠标进入节点
    public override void OnNodeHovered(NodeItem _node)
    {
        BattleNodeMgr.instance.OnNodeHovered(_node.GetComponent<NodeItem_Battle>());
    }

    public override void OnNodeUnhovered(NodeItem _node)
    {
        BattleNodeMgr.instance.OnNodeUnhovered(_node.GetComponent<NodeItem_Battle>());
    }

    //鼠标在节点内移动
    public void OnMouseMoved(NodeItem _node)
    {
        BattleNodeMgr.instance.OnMouseMoved(_node.GetComponent<NodeItem_Battle>());
    }

    //点击节点
    public override void OnNodePressed(NodeItem _node)
    {
        BattleNodeMgr.instance.OnNodePressed(_node.GetComponent<NodeItem_Battle>());
    }

	//获取双格单位可到达节点（效率并不高
	public List<Node> GetTwoHexUnitReachableNodesWithinRange(Node _node1, Node _node2, int _range, bool _walkable)
	{
		Node[] nodes = { _node1, _node2 };

		List<Node> nodeList = new List<Node>();

		//头尾各搜索一次
		for (int i = 0; i < nodes.Length; i++)
		{
			List<Node> open = new List<Node>();
			List<Node> close = new List<Node>();
			open.Add(nodes[i]);

			//搜索方向
			int dir = i == 0 ? 1 : -1;

			//遍历range次
			for (int j = 0; j < _range; j++)
			{
				//挑选每个节点，判断相邻节点
				int length = open.Count;
				while(length > 0)
				{
					length--;

					Node curNode = open[0];
					open.Remove(curNode);
					close.Add(curNode);

					foreach (var item in GetNearbyNodes(curNode))
					{
						if (item == null)
							continue;

						//如果已经在集合中
						if (open.Contains(item) || close.Contains(item))
							continue;

						//walkable指的是碰到障碍物也将其作为搜索节点
						if (!_walkable)
							open.Add(item);

						Vector2Int pos = item.pos;
						pos.x += BattleManager.currentActionUnit.sideFacing * dir;

						//加入最终输出节点
						if (isNodeAvailable(pos) &&
							(item.walkable && (GetNode(pos).walkable || GetNode(pos) == nodes[i])))
						{
							nodeList.Add(item);
							open.Add(item);
						}
					}
				}
			}
		}

		return nodeList;
	}
	public List<NodeItem> GetTwoHexUnitReachableNodeItemsWithinRange(NodeItem _node1, NodeItem _node2, int _range, bool _walkable)
	{
		List<NodeItem> list = new List<NodeItem>();
		//获取可移动范围
		foreach (var item in GetTwoHexUnitReachableNodesWithinRange(GetNode(_node1.pos), GetNode(_node2.pos), _range, _walkable))
		{
			list.Add(GetNodeItem(item.pos));
		}

		//去除其中前方点不存在，或不可通行的节点
		/*for (int i = list.Count - 1; i > 0; i--)
		{
			Vector2Int pos = list[i].pos;
			pos.x += BattleManager.currentActionUnit.facing;
			if (!isNodeAvailable(pos) || !GetNode(pos).walkable)
				list.RemoveAt(i);
		}*/

		list.Remove(_node1);
		list.Remove(_node2);

		return list;
	}

	//链接单位和节点
	public void LinkNodeWithUnit(Unit _unit, NodeItem _nodeItem)
	{
		//单位占据的所有节点
		List<NodeItem> nodes = new List<NodeItem>();
		nodes.Add(_nodeItem);

		//如果是双格单位，同时修改前方节点
		if (_unit.type.isTwoHexsUnit)
		{
			Vector2Int pos = _nodeItem.pos;
			pos.x += _unit.sideFacing;
			nodes.Add(GetNodeItem(pos));
		}

		//如果已经和节点链接，取消链接
		if (_unit.GetComponent<Unit>().nodeItem != null)
		{
			UnlinkNodeWithUnit(_unit);
		}

		_unit.nodeItem = _nodeItem;

		foreach (var item in nodes)
		{
			item.nodeObject = _unit;

			GetNode(item.pos).walkable = false;
		}
	}
	//取消链接单位和节点
	public void UnlinkNodeWithUnit(Unit _unit)
	{
		//单位占据的所有节点
		List<NodeItem> nodes = new List<NodeItem>();
		nodes.Add(_unit.nodeItem);

		//如果是双格单位，同时修改前方节点
		if (_unit.type.isTwoHexsUnit)
		{
			Vector2Int pos = _unit.nodeItem.pos;
			pos.x += _unit.sideFacing;
			nodes.Add(GetNodeItem(pos));
		}
		//取消链接
		foreach (var item in nodes)
		{
			item.nodeObject = null;
			_unit.nodeItem = null;

			GetNode(item.pos).walkable = true;
		}
	}
}