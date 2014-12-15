using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Test1.AI
{

    public class Map
    {
        int k = -1;
        static int[,] Mapdata = new int[20, 20];
        GameEngine engine = new GameEngine();
        

        public Map()
        {
            Mapdata = engine.passMap();
        }
        // originally there was static changed it below
        public  static int getMap(int x, int y)
        {
            int yMax = Mapdata.GetUpperBound(0);
            int xMax = Mapdata.GetUpperBound(1);
            if (x < 0 || x > xMax)
                return -1;
            else if (y < 0 || y > yMax)
                return -1;
            else
                return Mapdata[y, x];
        }


        static public void PrintSolution(ArrayList solutionPathList)
        {
            int yMax = Mapdata.GetUpperBound(0);
            int xMax = Mapdata.GetUpperBound(1);

            for (int j = 0; j <= yMax; j++)
            {
                for (int i = 0; i <= xMax; i++)
                {
                    bool solutionNode = false;
                    foreach (Node n in solutionPathList)
                    {
                        Node tmp = new Node(null, null, 0, i, j);

                        if (n.isMatch(tmp))
                        {
                            solutionNode = true;
                            break;
                        }
                    }
                    if (solutionNode)
                        Console.Write("o "); //solution path
                    else if (Map.getMap(i, j) == -1)
                        Console.Write("# "); //wall
                    else
                        Console.Write(". "); //road
                }
                Console.WriteLine("");
            }
        }
    }
}
