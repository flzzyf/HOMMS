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
		List<Node> nodes = new List<Node>();
		//加入初始节点
		nodes.Add(_node1);
		nodes.Add(_node2);

		//遍历range次
		while (_range > 0)
		{
			List<Node> temp = new List<Node>();

			//挑选每个节点，判断相邻节点
			foreach (var item in nodes)
			{
				foreach (var item2 in GetNearbyNodes(item))
				{
					//如果存在，不在表内，而且符合行走条件
					if (item2 != null && !nodes.Contains(item2) && !temp.Contains(item2) && (!_walkable || (_walkable && item2.walkable)))
						temp.Add(item2);
				}
			}

			foreach (var item in temp)
			{
				nodes.Add(item);
			}

			_range--;
		}

		return nodes;
	}
	public List<NodeItem> GetTwoHexUnitReachableNodeItemsWithinRange(NodeItem _node1, NodeItem _node2, int _range, bool _walkable)
	{
		List<NodeItem> list = new List<NodeItem>();
		foreach (var item in GetTwoHexUnitReachableNodesWithinRange(GetNode(_node1.pos), GetNode(_node2.pos), _range, _walkable))
		{
			list.Add(GetNodeItem(item.pos));
		}

		list.Remove(_node1);
		list.Remove(_node2);

		return list;
	}
}