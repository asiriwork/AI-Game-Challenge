using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Test1.AI
{
    class Command
    {
        
        public GameEngine enginePath = new GameEngine();
        static int coinX  ;
        static int coinY  ;
        static int time;
        static int starX;
        static int startY;
        static int index3;
       // static int previousCoinPile;
       // int direction;
        public  int nodeCount=0, nodeCount2=0;
        ArrayList solutionList;
        String commandToSend;
        static Boolean coinCheck = true;

        

        public void getStartPosition()
        {
            starX = GameEngine.gamer[GameEngine.clientTankIndex].x;
            startY = GameEngine.gamer[GameEngine.clientTankIndex].y;
           // direction = GameEngine.gamer[GameEngine.clientTankIndex].direction;
           // Console.WriteLine("start postion {0} {1} ", starX, startY);
        }

     

        public String getPath()
        {
            //int index =0;
            if (coinCheck)
            {
                coinX = 5;
                coinY = 5;
                coinCheck = false;
            }
           // getStartPosition();

            Astar pathFinder = new Astar();
            solutionList = pathFinder.AIsolution(coinX, coinY, starX, startY);
            nodeCount = solutionList.Count;
            //to check whether nodcount works
            Console.WriteLine("start postion {0} {1} ", starX, startY);
            Console.WriteLine("NODE COUNT = {0}  Coin : {1} , {2}", nodeCount,coinX,coinY);
           
             Node currentNode = (Node)solutionList[0];
             
             int nextX;
             int nextY;
             if ((coinX == starX && coinY == startY) )
             {
                 nextX = 5;
                 nextY = 5;
                 
             }
             else 
             {

                 Node nextNode = (Node)solutionList[1];
                 nextX = nextNode.x;
                 nextY = nextNode.y;
             }
             commandToSend = getDirection(nextX, nextY, currentNode.x, currentNode.y);
             int playercount = 0;
             for (int k = 0; k < GameEngine.msgSplayerCount; k++)
             {
                 //remove already achieved coinpiles set time to zero by other tanks. (includese this tank too)
                 playercount++;
                 if (k!= GameEngine.clientTankIndex &&  GameEngine.gamer[k].x == nextX && GameEngine.gamer[k].y == nextY && GameEngine.gamer[GameEngine.clientTankIndex].direction == oppositeDir(GameEngine.gamer[k].direction))
                 {
                     if(GameEngine.gamer[k].health != 0 )
                        commandToSend = "SHOOT#";
                 }
             }
           
             
             
             Map.PrintSolution(solutionList);
             solutionList.Clear();

            return commandToSend;
        }

        public int oppositeDir(int dir)
        {
            if (dir == 0)
                return 2;
            if (dir == 1)
                return 3;
            if (dir == 2)
                return 0;
            if (dir == 3)
                return 1;
            else
                return -1;
        }

        public void selectCoinPile()
        {
            Boolean getCurrentCoin = false;
            Boolean onCoin = false;
            Boolean removeCurrentCoin = false;
            int oneNodeIndex = 0;
            int curentCoinCount =0;
            int goalx =0;
            int goaly=0;
            
           // Astar pathFinder = new Astar();
            getStartPosition();
            int[] bestCoinPile = new int[GameEngine.coins.Count];
            //List<int> indexOfCoinPlie = new List<int>();
           // int i = 0;
            int counter = 0;
            //everything is working outruling achieved coin piles
            /*if (!coinCheck)
            {
                int playercount=0;
                for(int k=0; k<GameEngine.msgSplayerCount ;k++ )
                {
                    //remove already achieved coinpiles set time to zero by other tanks. (includese this tank too)
                    playercount++;
                    if (GameEngine.gamer[k].x == coinX && GameEngine.gamer[k].y == coinY)
                    {
                        GameEngine.setCoinPileTime(index3);
                    }
                }
                Console.WriteLine("+_+_+@##*@*@#&@&#(!*@&#(*($ Plyaer COUnt    " + playercount);
            } */
            
            int d =0;
            int removeCoinIndex = 0;
            
            while( d < GameEngine.coins.Count && !coinCheck)
            {
                d = 0;
                foreach (GameEngine.coinPile coinPile in GameEngine.coins)
                {
                    d++;
                    DateTime timeStamp = coinPile.timeStamp;
                    DateTime currentDate = DateTime.Now;
                    long elapsedTicks = currentDate.Ticks - timeStamp.Ticks;
                    TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                    if (elapsedSpan.TotalMilliseconds < coinPile.time)
                    {
                         Astar pathFinder1 = new Astar();
                         solutionList = pathFinder1.AIsolution(coinPile.x, coinPile.y, starX, startY);
                         nodeCount2 = solutionList.Count;
                        if ((coinPile.time - elapsedSpan.TotalMilliseconds) / 1000 >= nodeCount2)
                        {
                            for (int k = 0; k < GameEngine.msgSplayerCount; k++)
                            {
                                //remove already achieved coinpiles set time to zero by other tanks. (includese this tank too)
                                
                                if (GameEngine.gamer[k].x == coinPile.x && GameEngine.gamer[k].y == coinPile.y)
                                {
                                    Console.WriteLine("^^^^^^^^^^^^   ^^^^^   ");
                                    Console.WriteLine("^^^^^^^^^^^^   ^^^^^   ");
                                    Console.WriteLine("^^^^^^^^^^^^   ^^^^^   ");
                                    onCoin = true;
                                    removeCoinIndex = d - 1;
                                    break;
                                    
                                }
                            }

                            if (onCoin)
                            {
                                onCoin = false;
                                break;
                            }
                         }

                    }
                    solutionList.Clear();
                }


                GameEngine.setCoinPileTime(removeCoinIndex);
            }
           
            foreach (GameEngine.coinPile coinPile in GameEngine.coins)
            {

                DateTime timeStamp = coinPile.timeStamp;
                DateTime currentDate = DateTime.Now;
                long elapsedTicks = currentDate.Ticks - timeStamp.Ticks;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                bestCoinPile[counter] = 1000;
                //Console.WriteLine("Elapesed time" + elapsedSpan.TotalMilliseconds  + "conpile time" + coinPile.time);
                if(elapsedSpan.TotalMilliseconds < coinPile.time)
                {
                    
                    Astar pathFinder1 = new Astar();
                    solutionList = pathFinder1.AIsolution(coinPile.x, coinPile.y, starX, startY);
                    nodeCount2 = solutionList.Count ;
                    if (nodeCount2 == 1)
                        continue;
                    Console.WriteLine("___________________ " + (coinPile.time - elapsedSpan.TotalMilliseconds) / 1000 + "______ Node Count VS time __ " + nodeCount2);
                    if ((coinPile.time - elapsedSpan.TotalMilliseconds)/1000 >= nodeCount2)
                    {

                        
                        bestCoinPile[counter] = nodeCount2;
                        if (!coinCheck)
                        {
                            if (coinX == coinPile.x && coinY == coinPile.y)
                            {
                                getCurrentCoin = true;
                                curentCoinCount =nodeCount2;
                            }
                        }
                        //to remove coin pile already achived This doesn't need any more
                     /*   if (nodeCount2 == 2)
                        {
                            //if two coin piles distance is two in nodes removes currently travelling one.
                            if (coinX == coinPile.x && coinY == coinPile.y)
                            {
                                oneNodeCount = true;
                                oneNodeIndex = counter;
                                goalx = coinPile.x;
                                goaly = coinPile.y;
                                removeCurrentCoin = true;
                            }
                            else if (!removeCurrentCoin)
                            {
                                oneNodeCount = true;
                                oneNodeIndex = counter;
                                goalx = coinPile.x;
                                goaly = coinPile.y;
                            }
                           
                        }
                       */
                        Console.WriteLine("Elapesed time" + elapsedSpan.TotalMilliseconds + "conpile time" + coinPile.time);
                        Console.WriteLine("COMES to the Wlleyadf;kjjjjjjjjj This is the Number(NOde Count)     ###" + bestCoinPile[counter]);
                        Console.WriteLine("Coin list " + coinPile.x + coinPile.y);
                        //Console.WriteLine("Elapesed time" + elapsedSpan.TotalMilliseconds + "conpile time" + coinPile.time);
                        //Console.WriteLine("OH yeah beby");
                       
                    }
                    solutionList.Clear();
                    //Console.WriteLine("Coin list " + coinPile.x, coinPile.y);


                }
               // Console.WriteLine("Coin list ALL list " + coinPile.x + coinPile.y);
                counter++;
             
            }
            //does't need this if cz player loob above the foreach covers the functianlity
          /*  if (oneNodeCount && counter == -1)
            {
                Console.WriteLine("Coin Tank Direction ::::::::::::: " + getStringDirection(GameEngine.gamer[GameEngine.clientTankIndex].direction) + "Travel Direction" + getDirection(goalx, goaly, starX, startY));
                if (getStringDirection(GameEngine.gamer[GameEngine.clientTankIndex].direction) == getDirection(goalx, goaly, starX, startY))
                {
                    GameEngine.setCoinPileTime(oneNodeIndex);
                    oneNodeCount = false;
                    Console.WriteLine("Coin pile time ::::::::::::: " + GameEngine.coins[oneNodeIndex].time);
                }
            } 
           */


           
            
            if (getCurrentCoin)
            {
                
                nodeCount = curentCoinCount;
            }
            else
            {
                nodeCount = 500;
            }
            if (GameEngine.coins.Count != 0)
            {
                if (bestCoinPile.Min() < nodeCount)
                {
                    index3 = Array.IndexOf(bestCoinPile, bestCoinPile.Min());
                    coinX = GameEngine.coins[index3].x;
                    coinY = GameEngine.coins[index3].y;



                    coinCheck = false;
                    Console.WriteLine("MIN MIN MIN COIN XY " + coinX + coinY + "  GameEngine.coins index " + Array.IndexOf(bestCoinPile, bestCoinPile.Min()));
                    Console.WriteLine("MIN MIN mIN MIN Value (NODE COUNT)  #####  " + bestCoinPile.Min());
                }
            }
            else if (nodeCount != 500)
            {
                coinCheck = false;
            }

            else
            {
                coinCheck = true;
            }
            Array.Clear(bestCoinPile, 0, GameEngine.coins.Count);
          


        }

        // this method can be used with life pack anything with time
        public Boolean evaluateTime()
        {
            return true;

        }

        public static String getStringDirection(int dir)
        {
            if (dir == 0)
            {
                return "UP#";
            }
            if (dir == 1)
            {
                return "RIGHT#";
            }
            if (dir == 2)
            {
                return "DOWN#";
            }
            if (dir == 3)
            {
                return "LEFT#";
            }
            return "UP#";
        }

        public static String getDirection(int nextX,int nextY,int x,int y)
        {
            if(nextX==x && nextY < y )
            {
                return "UP#";
                
            }
            else if(nextX == x && nextY > y)
            {
                return "DOWN#";
            }
            else if (nextY == y && nextX < x)
            {
                return "LEFT#";
            }
            else if (nextY == y && nextX > x)
            {
                return "RIGHT#";
            }
            else
            {
                return "RIGHT#";
            }
        }


    }
}
