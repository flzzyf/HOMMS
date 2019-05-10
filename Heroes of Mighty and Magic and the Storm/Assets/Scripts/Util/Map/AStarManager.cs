using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarManager
{
    public static MapManager map;

    public static List<Node> FindPath(Vector2Int _startPos, Vector2Int _endPos, bool _ignoreObstacle = false)
    {
        Node startNode = map.GetNode(_startPos);
        Node endNode = map.GetNode(_endPos);

        //开集和闭集
        List<Node> openSet = new List<Node>();
        List<Node> closeSet = new List<Node>();
        //将开始节点介入开集
        openSet.Add(startNode);
        //开始搜索
        while (openSet.Count > 0)
        {
            //当前所在节点
            Node curNode = openSet[0];
            //从开集中选出f和h最小的
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].f <= curNode.f && openSet[i].h <= curNode.h)
                {
                    curNode = openSet[i];
                }
            }
            //把当前所在节点加入闭集
            openSet.Remove(curNode);
            closeSet.Add(curNode);
            //如果就是终点
            // if (curNode == endNode)
            // {
            //     //可通行
            //     return GeneratePath(startNode, endNode);
            // }
            //判断周围节点
            foreach (var item in map.GetNearbyNodes(curNode))
            {
                //如果是终点，结束
                if (item == endNode)
                {
                    //可通行
                    item.parentNode = curNode;

                    return GeneratePath(startNode, endNode);
                }
                //如果不可通行或在闭集中，则跳过。加入特殊规则判定
                if ((!_ignoreObstacle && !item.walkable) || closeSet.Contains(item))
                {
                    continue;
                }
                //判断新节点耗费
                int newH = GetNodeDistance(curNode, item);
                int newCost = curNode.g + newH;
                //耗费更低或不在开集中，则加入开集
                if (newCost < item.g || !openSet.Contains(item))
                {
                    item.g = newCost;
                    item.h = newH;
                    item.parentNode = curNode;
                    if (!openSet.Contains(item))
                    {
                        openSet.Add(item);
                    }
                }
            }
        }
        //无法通行
        return null;
    }

	//生成路径
    static List<Node> GeneratePath(Node _startNode, Node _endNode)
    {
        Node curNode = _endNode;

        List<Node> path = new List<Node>();

        while (curNode != _startNode)
        {
            path.Add(curNode);

            curNode = curNode.parentNode;
        }

        path.Add(_startNode);
        //反转路径然后生成显示路径
        path.Reverse();

        return path;
    }

    //节点间路径距离估计算法（只考虑XY轴）
    static int GetNodeDistance(Node a, Node b)
    {
        //先斜着走然后直走
        int x = Mathf.Abs(a.x - b.x);
        int y = Mathf.Abs(a.y - b.y);

        if (x > y)
            return 14 * y + 10 * (x - y);
        else
            return 14 * x + 10 * (y - x);
    }

    static List<NodeItem> FindPath(NodeItem _start, NodeItem _end, bool _ignoreObstacle = false)
    {
        List<NodeItem> list = new List<NodeItem>();

        Vector2Int startPos = _start.pos;
        Vector2Int endPos = _end.pos;

        List<Node> path = FindPath(startPos, endPos, _ignoreObstacle);
        if (path == null)
            return null;

        foreach (var item in path)
        {
            list.Add(map.GetNodeItem(item.pos));
        }

        return list;
    }

    public static List<NodeItem> FindPath(MapManager _map, NodeItem _start, NodeItem _end, bool _ignoreObstacle = false)
    {
        map = _map;
        return FindPath(_start, _end, _ignoreObstacle);
    }

    //获取节点间距离
    public static int GetNodeItemDistance(NodeItem _origin, NodeItem _target, bool _ignoreObstacle = false)
    {
        List<NodeItem> path = FindPath(BattleManager.instance.map, _origin, _target, _ignoreObstacle);

        return path.Count - 1;
    }

	//特殊规则，该节点双格单位不可通行则True
	static bool SpecialRule(Node _node)
	{
		//是双格单位，则判定前方节点也可通行
		if(BattleManager.currentActionUnit.type.isTwoHexsUnit)
		{
			Vector2Int pos = _node.pos;
			pos.x += BattleManager.currentActionUnit.sideFacing;
			if (map.isNodeAvailable(pos) && !map.GetNode(pos).walkable)
				return false;
		}

		return true;
	}

	//双格单位寻路
	public static List<Node> FindPath_TwoHex(MapManager _map, Node _node1, Node _node2, Node _endNode, bool _ignoreObstacle = false)
	{
		map = _map;

		Node startNode = _node1;

		//单位前方点的偏移
		int offset = _node2.pos.x - _node1.pos.x;

		//开集和闭集
		List<Node> openSet = new List<Node>();
		List<Node> closeSet = new List<Node>();
		//将开始节点介入开集
		openSet.Add(startNode);
		//开始搜索
		while (openSet.Count > 0)
		{
			//当前所在节点
			Node curNode = openSet[0];
			//从开集中选出f和h最小的
			for (int i = 0; i < openSet.Count; i++)
			{
				if (openSet[i].f <= curNode.f && openSet[i].h <= curNode.h)
				{
					curNode = openSet[i];
				}
			}
			//把当前所在节点加入闭集
			openSet.Remove(curNode);
			closeSet.Add(curNode);
			//如果就是终点
			// if (curNode == endNode)
			// {
			//     //可通行
			//     return GeneratePath(startNode, endNode);
			// }
			//判断周围节点
			foreach (var item in map.GetNearbyNodes(curNode))
			{
				//如果是终点，结束
				if (item == _endNode)
				{
					//可通行
					item.parentNode = curNode;

					return GeneratePath(startNode, _endNode);
				}
				//如果不可通行或在闭集中，则跳过
				if ((!_ignoreObstacle && !(item.walkable || item == _node2)) || closeSet.Contains(item))
				{
					continue;
				}


				//判断偏移点是否可通行
				Vector2Int pos = item.pos;
				pos.x += offset;
				//如果不存在、或者不可通行（而且不是单位前方点
				if (!(map.isNodeAvailable(pos) && (map.GetNode(pos).walkable || map.GetNode(pos) == _node2)))
				{
					continue;
				}

				//判断新节点耗费
				int newH = GetNodeDistance(curNode, item);
				int newCost = curNode.g + newH;
				//耗费更低或不在开集中，则加入开集
				if (newCost < item.g || !openSet.Contains(item))
				{
					item.g = newCost;
					item.h = newH;
					item.parentNode = curNode;

					openSet.Add(item);
				}
			}
		}
		//无法通行
		return null;
	}

	public static List<Node> FindPath(MapManager _map, Node _node1, Node _node2, Node _endNode, bool _ignoreObstacle = false)
	{
		map = _map;

		//起始节点，选中最近的
		Node startNode = _endNode.x > _node2.x ? _node2 : _node1;
		//int offset = _endNode.x > _node2.x ? -1 : 1;

		//开集和闭集
		List<Node> openSet = new List<Node>();
		List<Node> closeSet = new List<Node>();
		//将开始节点介入开集
		openSet.Add(startNode);
		//开始搜索
		while (openSet.Count > 0)
		{
			//当前所在节点
			Node curNode = openSet[0];
			//从开集中选出f和h最小的
			for (int i = 0; i < openSet.Count; i++)
			{
				if (openSet[i].f <= curNode.f && openSet[i].h <= curNode.h)
				{
					curNode = openSet[i];
				}
			}
			//把当前所在节点加入闭集
			openSet.Remove(curNode);
			closeSet.Add(curNode);
			//如果就是终点
			// if (curNode == endNode)
			// {
			//     //可通行
			//     return GeneratePath(startNode, endNode);
			// }
			//判断周围节点
			foreach (var item in map.GetNearbyNodes(curNode))
			{
				//如果是终点，结束
				if (item == _endNode)
				{
					//可通行
					item.parentNode = curNode;

					return GeneratePath(startNode, _endNode);
				}
				//如果不可通行或在闭集中，则跳过
				if ((!_ignoreObstacle && !item.walkable) || closeSet.Contains(item))
				{
					continue;
				}


				Vector2Int pos = item.pos;
				int offset = item.pos.x > curNode.x ? -1 : 1;
				pos.x -= offset;

				if (!(map.isNodeAvailable(pos) && (item.walkable && (map.GetNode(pos).walkable || map.GetNode(pos) == startNode))))
				{
					continue;
				}

				//判断新节点耗费
				int newH = GetNodeDistance(curNode, item);
				int newCost = curNode.g + newH;
				//耗费更低或不在开集中，则加入开集
				if (newCost < item.g || !openSet.Contains(item))
				{
					item.g = newCost;
					item.h = newH;
					item.parentNode = curNode;

					openSet.Add(item);
				}
			}
		}
		//无法通行
		return null;
	}
}
