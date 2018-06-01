using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PathFinder
{
    class Program
    {
        static void Main(string[] args) {
            PathFinder p = new PathFinder();
            p.PrintMatrix();
            p.FindPath();
            Console.ReadKey();
        }
    }

    public class PathFinder
    {
        char[,] matrix = new char[8, 8]
        {
            {'0','0','0','0','0','x','0','0'},
            {'x','x','x','x','0','x','0','0'},
            {'0','0','0','0','0','x','0','0'},
            {'0','0','0','x','x','x','0','0'},
            {'0','0','0','0','0','x','0','0'},
            {'0','0','0','0','0','x','0','0'},
            {'0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0'}
        };
        PathNode startPoint = new PathNode(0, 0);
        PathNode endPoint = new PathNode(4, 7);
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closeList = new List<PathNode>();
        PathNode[] dir = new PathNode[8]
        {
            new PathNode(-1,0),
            new PathNode(-1, 1),
            new PathNode(0,1),
            new PathNode(1,1),
            new PathNode(1,0),
            new PathNode(1,-1),
            new PathNode(0, -1),
            new PathNode(-1,-1)
        };

        public void FindPath() {
            openList.Clear();
            closeList.Clear();
            matrix[startPoint.x, startPoint.y] = 's';
            matrix[endPoint.x, endPoint.y] = 'e';
            openList.Add(startPoint);
            while(openList.Count > 0) {
                SortUp(ref openList);
                var checkingPoint = openList[0];
                openList.Remove(checkingPoint);
                closeList.Add(checkingPoint);
                for(int i = 0; i < dir.Length; i++) {
                    var tmpPoint = checkingPoint + dir[i];
                    //出界
                    if(tmpPoint.x < 0 || tmpPoint.x > matrix.GetUpperBound(0) || tmpPoint.y < 0 || tmpPoint.y > matrix.GetUpperBound(1)) {
                        continue;
                    }
                    //已经在关闭列表或者是障碍物
                    if(closeList.Contains(tmpPoint) || matrix[tmpPoint.x, tmpPoint.y] == 'x') {
                        continue;
                    }
                    //是终点
                    if(tmpPoint.x == endPoint.x && tmpPoint.y == endPoint.y) {
                        endPoint.parent = checkingPoint;
                        openList.Add(endPoint);
                        Console.WriteLine("Find Path");
                        var parent = endPoint.parent;
                        while(parent != null) {
                            matrix[parent.x, parent.y] = 'p';
                            parent = parent.parent;
                        }
                        PrintMatrix();
                        return;
                    }
                    if(openList.Exists(x => x.x == tmpPoint.x && x.y == tmpPoint.y)) {
                        var tmpG = ((Math.Abs(checkingPoint.x - tmpPoint.x) + Math.Abs(checkingPoint.y - tmpPoint.y)) == 2 ? 14 : 10) + (tmpPoint.parent == null ? 0 : tmpPoint.parent.G);
                        if(tmpG < tmpPoint.G) {
                            tmpPoint.parent = checkingPoint;
                            tmpPoint.G = tmpG;
                        }
                    }
                    else {
                        openList.Add(tmpPoint);
                        //matrix[tmpPoint.x, tmpPoint.y] = 'p';
                        tmpPoint.parent = checkingPoint;
                        tmpPoint.G = ((Math.Abs(checkingPoint.x - tmpPoint.x) + Math.Abs(checkingPoint.y - tmpPoint.y)) == 2 ? 14 : 10) + (tmpPoint.parent == null ? 0 : tmpPoint.parent.G);
                        tmpPoint.H = (Math.Abs(tmpPoint.x - endPoint.x) + Math.Abs(tmpPoint.y - endPoint.y)) * 10;
                    }

                }
            }
        }

        public void PrintMatrix() {
            string str = "";
            for(int i = 0; i <= matrix.GetUpperBound(0); i++) {
                for(int ii = 0; ii <= matrix.GetUpperBound(1); ii++) {
                    str = str + matrix[i, ii].ToString() + (ii == matrix.GetUpperBound(1) ? "" : ",");
                }
                str += "\n";
            }
            Console.WriteLine(str);
        }

        /// <summary>
        /// 对传入的list按照F值进行升序排序
        /// </summary>
        /// <param name="_list"></param>
        public void SortUp(ref List<PathNode> _list) {
            if(_list.Count < 2) {
                return;
            }
            for(int i = 0; i < _list.Count; i++) {
                for(int j = i + 1; j < _list.Count; j++) {
                    if(_list[i].F > _list[j].F) {
                        var tmp = _list[i];
                        _list[i] = _list[j];
                        _list[j] = tmp;
                    }
                }
            }
        }

    }
    public class PathNode
    {
        public PathNode parent = null;
        public double F {
            get { return G + H; }
        }

        /// <summary>
        /// 从起点移动到指定点的消耗
        /// </summary>
        public double G;

        /// <summary>
        /// 从指定点移动到终点的消耗
        /// </summary>
        public double H;
        public int x;
        public int y;
        public PathNode(int _x, int _y) {
            x = _x;
            y = _y;
        }

        public static PathNode operator +(PathNode _a, PathNode _b) {
            return new PathNode(_a.x + _b.x, _a.y + _b.y);
        }
    }
}
