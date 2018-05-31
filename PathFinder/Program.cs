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
            //int[] array = new int[10] { 2, 3, 6, 1, 9, 4, 0, 8, 7, 5 };
            //for(int i = 0; i < array.Length; i++) {
            //    for(int ii = i + 1; ii < array.Length; ii++) {
            //        if(array[i] > array[ii]) {
            //            int tmp = array[i];
            //            array[i] = array[ii];
            //            array[ii] = tmp;
            //        }
            //        string str = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9]);
            //        Console.WriteLine(str);
            //        Thread.Sleep(500);
            //    }
            //}

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
            {'0','0','0','0','0','0','0','0'},
            {'0','0','x','x','x','x','0','0'},
            {'0','0','0','0','0','x','0','0'},
            {'0','0','0','0','0','x','0','x'},
            {'0','0','0','0','0','x','0','0'},
            {'0','0','x','x','x','x','0','0'},
            {'0','0','0','0','0','0','0','0'},
            {'0','0','0','0','0','0','0','0'}
        };
        PathNode startPoint = new PathNode(3, 0);
        PathNode endPoint = new PathNode(6, 6);
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closeList = new List<PathNode>();
        PathNode[] dir = new PathNode[4]
        {
            new PathNode(-1,0),
            //new P(-1, 1),
            new PathNode(0,1),
            //new P(1,1),
            new PathNode(1,0),
            //new P(1,-1),
            new PathNode(0, -1),
            //new P(-1,-1)
        };

        public void FindPath() {
            openList.Clear();
            closeList.Clear();
            matrix[startPoint.x, startPoint.y] = 's';
            matrix[endPoint.x, endPoint.y] = 'e';
            openList.Add(startPoint);
            while(openList.Count > 0) {
                var checkingPoint = openList[0];
                openList.Remove(checkingPoint);
                closeList.Add(checkingPoint);
                for(int i = 0; i < dir.Length; i++) {
                    var tmpPoint = checkingPoint + dir[i];
                    if(!closeList.Contains(tmpPoint) && matrix[tmpPoint.x, tmpPoint.y] != 'x') {
                        if(openList.Contains(tmpPoint)) {

                        }
                        else {
                            openList.Add(tmpPoint);
                            tmpPoint.parent = checkingPoint;
                            tmpPoint.G = 10;
                            tmpPoint.H = tmpPoint.Distance(endPoint);
                        }
                    }
                }
                SortUp(ref openList);
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

        public static bool operator ==(PathNode _a, PathNode _b) {
            return _a.x == _b.x && _a.y == _b.y;
        }
        public static bool operator !=(PathNode _a, PathNode _b) {
            return _a.x != _b.x || _a.y != _b.y;
        }
        public override bool Equals(object obj) {
            return base.Equals(obj);
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public double Distance(PathNode _a) {
            return Math.Sqrt(Math.Pow(Math.Abs(_a.x - x), 2) + Math.Pow(Math.Abs(_a.y - y), 2));
        }
    }
}
