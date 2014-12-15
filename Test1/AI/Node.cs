using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Test1.AI
{
    public class Node : IComparable
    {

        public int totalCost
        {
            get
            {
                return g + h;
            }
            set
            {
                totalCost = value;
            }
        }
        public int g;
        public int h;

        public Map newMap = new Map();

        public int x;
        public int y;


        private Node _goalNode;
        public Node parentNode;
        public Node grandParentNode;
        private int gCost;


        public Node(Node parentNode, Node goalNode, int gCost, int x, int y)
        {

            this.parentNode = parentNode;
            this._goalNode = goalNode;
            this.gCost = gCost;
            this.x = x;
            this.y = y;
            if (parentNode != null)
            {
                this.grandParentNode = parentNode.parentNode;
            }
            
            InitNode();
        }

        private void InitNode()
        {

            if (parentNode == null)
            {
                this.g = (parentNode != null) ? this.parentNode.g + gCost : gCost;
                this.h = (_goalNode != null) ? (int)Huristic_H() : 0;
               //this method is okay this is for the start node
            }
            if (grandParentNode == null)
            {
                this.g = (parentNode != null) ? this.parentNode.g + gCost : gCost;
                this.h = (_goalNode != null) ? (int)Huristic_H() : 0;
                //change this to add cost for first turn of the tank
                //this for the second node in the path
            }
            else if (grandParentNode != null)
            {

                if (parentNode.x == this.x && this.x == grandParentNode.x)
                {
                    this.g = (parentNode != null) ? this.parentNode.g + gCost : gCost;
                    this.h = (_goalNode != null) ? (int)Huristic_H() : 0;
                    // Console.WriteLine("{0}  {1}   {2} ",grandParentNode.x,this.x,parentNode.x);
                    // Console.WriteLine(this.x);
                }
                else if (parentNode.y == this.y && this.y == grandParentNode.y)
                {
                    this.g = (parentNode != null) ? this.parentNode.g + gCost : gCost;
                    this.h = (_goalNode != null) ? (int)Huristic_H() : 0;
                }
                else
                {
                    this.g = (parentNode != null) ? this.parentNode.g + gCost + 3 : gCost;
                    this.h = (_goalNode != null) ? (int)Huristic_H() : 0;
                }
            }
            //later gcost can be changed to tanks direction when travelling
        }

        private double Huristic_H()
        {

            // here euclidean distance
           /* double xd = this.x - this._goalNode.x;
            double yd = this.y - this._goalNode.y;
            return Math.Sqrt((xd * xd) + (yd * yd));  */
            //change this to man hatten distance
            int xd =Math.Abs( this.x - this._goalNode.x);
            int yd = Math.Abs(this.y - this._goalNode.y);
            return (xd+yd); 

        }

        public int CompareTo(object obj)
        {

            Node n = (Node)obj;
           // if(Command.getStringDirection(GameEngine.gamer[GameEngine.clientTankIndex].direction) == Command.getDirection(
            int cFactor = this.gCost - n.gCost;
            return cFactor;
        }

        public bool isMatch(Node n)
        {
            if (n != null)
                return (x == n.x && y == n.y);
            else
                return false;
        }

        public ArrayList GetSuccessors()
        {
            ArrayList successors = new ArrayList();
            
            for (int xd = -1; xd <= 1; xd++)
            {
                for (int yd = -1; yd <= 1; yd++)
                {
                    //Boolean check = false;
                    if (xd == 1 || xd == -1)
                    {
                        if (yd == 0)
                        {
                            if (Map.getMap(x + xd, y + yd) != -1)
                            {
                                Node n = new Node(this, this._goalNode, Map.getMap(x + xd, y + yd), x + xd, y + yd);
                                if (!n.isMatch(this.parentNode) && !n.isMatch(this) && !(this.x == n.x && this.y == n.y))
                                    successors.Add(n);

                            }

                        }

                    }
                    else if (xd == 0)
                    {
                        if (Map.getMap(x + xd, y + yd) != -1)
                        {
                            Node n = new Node(this, this._goalNode, Map.getMap(x + xd, y + yd), x + xd, y + yd);
                            if (!n.isMatch(this.parentNode) && !n.isMatch(this) && !(this.x == n.x && this.y == n.y))
                                successors.Add(n);

                        }

                    }

                }
            }
            return successors;
        }
    }
}
