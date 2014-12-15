using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test1
{
    class GameEngine
    {

        int numOfPlayers = 0;
        int bricksCount = 0;
        public static int clientTankIndex;
        public static int msgSplayerCount;
        
        public struct player
        {
            public int index;
            public int x;
            public int y;
            public int direction;
            public int isShot;
            public int health;
            public int coins;
            public int points;
        }

        public struct point
        {
            public int x;
            public int y;
            public int damageLevel;
        }


        public struct coinPile
        {
            public int x;
            public int y;
            public int time;
            public int value;
            public DateTime timeStamp;
        }


        /* public struct bullet
         {
             public int x;
             public int y;
             public int derection;
             public int count;

         } */

        public struct helthPack
        {
            public int x;
            public int y;
            public int time;
        }

        static int[,] MapData = new int[20,20];


        public  static List<player> gamer = new List<player>();
        public static List<point> bricks = new List<point>();
        public static List<point> water = new List<point>();
        public static List<point> stones = new List<point>();
        public static List<coinPile> coins = new List<coinPile>();
        public static List<helthPack> medipack = new List<helthPack>();
        
        // this method not used so far
        public static void setCoinPileTime(int index1)
        {
            
            coinPile newCoinPile;
            newCoinPile.x = coins[index1].x;
            newCoinPile.y = coins[index1].y;
            newCoinPile.time =0;
            newCoinPile.timeStamp = coins[index1].timeStamp;
            newCoinPile.value = coins[index1].value;
            coins.RemoveAt(index1);
            coins.Insert(index1, newCoinPile);

        }


        public void AnalyseServerResponse(String gameUpdate)
        {

            if (gameUpdate.StartsWith("G") && gameUpdate.Contains(':'))
            { // global updates
                GlobalUpdate(gameUpdate.Substring(2, gameUpdate.Length - 3));

            }
            else if (gameUpdate.StartsWith("C") && gameUpdate.Contains(':'))
            { // coin piles
                ProcessCoinData(gameUpdate.Substring(2, gameUpdate.Length - 3));
                
            }
            else if (gameUpdate.StartsWith("L") && gameUpdate.Contains(':'))
            { // life packs
                ProcessLifePacksData(gameUpdate.Substring(2, gameUpdate.Length - 3));

            }
            else if (gameUpdate.StartsWith("S") && gameUpdate.Contains(':'))
            { // server acceptance
                ProcessConnEstdData(gameUpdate.Substring(2, gameUpdate.Length - 3));

            }
            else if (gameUpdate.StartsWith("I") && gameUpdate.Contains(':'))
            { // player initialisation
                //String waterDetail = waterDetail.Substring(0, waterDetail1[waterDetail1.Length - 1].IndexOf('#'));
                  ProcessGameInitiationData(gameUpdate.Substring(2, gameUpdate.IndexOf('#')-2));
                  GameMapGenerate();
                  PrintSolution();
            }
        }

        public void GlobalUpdate(String gameUpdate)
        {
             int playerCount = 0;

            string[] players = gameUpdate.Split(':');

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].StartsWith("P"))
                {

                     playerCount++;

                    player newPlayer;
                    string[] playerDetail = players[i].Split(';');



                    for (int j = 0; j < playerDetail.Length; j++)
                    {
                        newPlayer.index = Convert.ToInt32(playerDetail[0].Substring(1));

                        newPlayer.x = Convert.ToInt32(playerDetail[1].Split(',')[0]);
                        newPlayer.y = Convert.ToInt32(playerDetail[1].Split(',')[1]);

                        newPlayer.direction = Convert.ToInt32(playerDetail[2]);

                        newPlayer.isShot = Convert.ToInt32(playerDetail[3]);

                        newPlayer.health = Convert.ToInt32(playerDetail[4]);

                        newPlayer.coins = Convert.ToInt32(playerDetail[5]);

                        newPlayer.points = Convert.ToInt32(playerDetail[6]);

                        gamer.Insert(newPlayer.index, newPlayer);


                      
                    }

                }

                else
                {
                    String[] brickDetail = players[i].Split(';');
                    point points;
                    int j;
                    for (j = 0; j < brickDetail.Length; j++)
                    {

                        points.x = Convert.ToInt32(brickDetail[j].Split(',')[0]);
                        points.y = Convert.ToInt32(brickDetail[j].Split(',')[1]);
                        points.damageLevel = Convert.ToInt32(brickDetail[j].Split(',')[2].Substring(0, 1));


                        bricks.Insert(j, points);
                    }

                    bricksCount = j;
                }




            }


            numOfPlayers = playerCount;
              /*   for(int i=0; i <bricksCount ;i++)
                  {
                      Console.WriteLine("   Player X = {0} ", bricks[i].x);
                      Console.WriteLine("   Player Y = {0}  ", bricks[i].y);
                      Console.WriteLine("   Damage Level = {0}", bricks[i].damageLevel);
                      Console.WriteLine("--------------------------------------------------------------------");
                  }
                  Console.WriteLine("---------------------------------------11-----------------------------");
                  Console.WriteLine("-----------------------------------22---------------------------------");
                  Console.WriteLine("---------------------------------33-----------------------------------"); 
              */ 
            /*
             for (int i = 0; i < numOfPlayers; i++)
                {
                    Console.WriteLine("   Player Index = {0} ", i);
                    Console.WriteLine("   Player X = {0} ", gamer[i].x);
                    Console.WriteLine("   Player Y = {0} ", gamer[i].y);
                    Console.WriteLine("   Player Points = {0} ", gamer[i].points);

                    Console.WriteLine("--------------------------------------------------------------------");
                }
                Console.WriteLine("---------------------------------------11-----------------------------");
                Console.WriteLine("-----------------------------------22---------------------------------");
                Console.WriteLine("---------------------------------33-----------------------------------"); 
            */


        }

        private void ProcessCoinData(String data)
        {
            String[] coinData = data.Split(':');
            coinPile coin;
            
            coin.x = Convert.ToInt32(coinData[0].Split(',')[0]);
            coin.y = Convert.ToInt32(coinData[0].Split(',')[1]);
            coin.time = Convert.ToInt32(coinData[1]);
            coin.value = Convert.ToInt32((coinData[2].Substring(0, coinData[2].IndexOf('#'))));
            DateTime currentDate = DateTime.Now;
            coin.timeStamp = currentDate;
            
            coins.Add(coin);


        }

        private void ProcessLifePacksData(String data)
        {
            String[] LifePackData = data.Split(':');
            helthPack life;
            life.x = Convert.ToInt32(LifePackData[0].Split(',')[0]);
            life.y = Convert.ToInt32(LifePackData[0].Split(',')[1]);
            life.time = Convert.ToInt32(LifePackData[1].Substring(0, LifePackData[1].IndexOf('#')));
           /* Console.WriteLine(life.x);
            Console.WriteLine(life.y);
            Console.WriteLine(life.time); */

            medipack.Add(life);
        }

        private void ProcessConnEstdData(String data)
        {
            string[] splitted = data.Split(':');

             msgSplayerCount = 0;

            for (int i = 0; i < splitted.Length; i++)
            {
                if (splitted[i].StartsWith("P"))
                {

                    msgSplayerCount++;

                    player newPlayer;
                    string[] playerDetail = splitted[i].Split(';');

                    for (int j = 0; j < playerDetail.Length; j++)
                    {
                        newPlayer.index = Convert.ToInt32(playerDetail[0].Substring(1));

                        newPlayer.x = Convert.ToInt32(playerDetail[1].Split(',')[0]);
                        newPlayer.y = Convert.ToInt32(playerDetail[1].Split(',')[1]);

                        newPlayer.direction = Convert.ToInt32(playerDetail[2].Substring(0, 1));

                        newPlayer.isShot = 0;

                        newPlayer.health = 100;

                        newPlayer.coins = 0;

                        newPlayer.points = 0;

                        gamer.Insert(newPlayer.index, newPlayer);


                    }


                }
                numOfPlayers = msgSplayerCount;

            }
           /* for (int i = 0; i < numOfPlayers; i++)
            {
                Console.WriteLine("   Player Index = {0} ", i);
                Console.WriteLine("   Player X = {0} ", gamer[i].x);
                Console.WriteLine("   Player Y = {0} ", gamer[i].y);
                Console.WriteLine("   Player Points = {0} ", gamer[i].direction);

                Console.WriteLine("--------------------------------------------------------------------");
            }
            Console.WriteLine("---------------------------------------11-----------------------------");
            Console.WriteLine("-----------------------------------22---------------------------------");
            Console.WriteLine("---------------------------------33-----------------------------------"); */





        }

        private void ProcessGameInitiationData(String data)
        {
             string[] splitted = data.Split(':');
            int k,m,l,j,r,br =0,st =0,wa =0;

            

            for (int i = 0; i < splitted.Length; i++)
            {
                if (splitted[i].StartsWith("P"))
                {
                    clientTankIndex = Convert.ToInt32(splitted[0].Substring(1));
                }
                else
                {
                    String[] brickDetail = splitted[1].Split(';');
                    point points;
                   // Console.WriteLine("bricks cordinates");
                   // Console.WriteLine(splitted[1]);


                    for (r = 0; r < brickDetail.Length; r++)
                    {

                        points.x = Convert.ToInt32(brickDetail[r].Split(',')[0]);
                        points.y = Convert.ToInt32(brickDetail[r].Split(',')[1]);
                        points.damageLevel = 0;


                        bricks.Insert(r, points);
                    }
                    br = r;

                    String[] stoneDetail = splitted[2].Split(';');
                    point point1;

                    for (k = 0; k < stoneDetail.Length; k++)
                    {

                        point1.x = Convert.ToInt32(stoneDetail[k].Split(',')[0]);
                        point1.y = Convert.ToInt32(stoneDetail[k].Split(',')[1]);
                        point1.damageLevel = 0;


                        stones.Insert(k, point1);
                    }
                    st = k;

                    String[] waterDetail = splitted[3].Split(';');

                    point point2;
                    //Console.WriteLine("Water cordinates");
                   // Console.WriteLine(splitted[3]);
                    for (l = 0; l < waterDetail.Length; l++)
                    {

                        point2.x = Convert.ToInt32(waterDetail[l].Split(',')[0]);
                        point2.y = Convert.ToInt32(waterDetail[l].Split(',')[1]);
                        point2.damageLevel = 0;


                        water.Insert(l, point2);
                    }
                    wa = l;

                }
                



            }
            /* TO check water Brick stone detail added corectly into the lists
             * 
             * 
            for (int v = 0; v < br; v++)
            {
                Console.WriteLine(" BRICK  Player X = {0} ", bricks[v].x);
                Console.WriteLine("   Player Y = {0}  ", bricks[v].y);
                Console.WriteLine("   Damage Level = {0}", bricks[v].damageLevel);
                Console.WriteLine("--------------------------------------------------------------------");
            }
            Console.WriteLine("---------------------------------------11-----------------------------");
            Console.WriteLine("-----------------------------------22---------------------------------");
            Console.WriteLine("---------------------------------33-----------------------------------");
            for (int n = 0; n < wa; n++)
            {
                Console.WriteLine("  WATER Player X = {0} ", water[n].x);
                Console.WriteLine("   Player Y = {0}  ", water[n].y);
                Console.WriteLine("   Damage Level = {0}", water[n].damageLevel);
                Console.WriteLine("--------------------------------------------------------------------");
            }
            Console.WriteLine("---------------------------------------11-----------------------------");
            Console.WriteLine("-----------------------------------22---------------------------------");
            Console.WriteLine("---------------------------------33-----------------------------------");
            for (int t = 0; t < st; t++)
            {
                Console.WriteLine("  STONES Player X = {0} ", stones[t].x);
                Console.WriteLine("   Player Y = {0}  ", stones[t].y);
                Console.WriteLine("   Damage Level = {0}", stones[t].damageLevel);
                Console.WriteLine("--------------------------------------------------------------------");
            }
            Console.WriteLine("---------------------------------------11-----------------------------");
            Console.WriteLine("-----------------------------------22---------------------------------");
            Console.WriteLine("---------------------------------33-----------------------------------");

            */




        }

        public void GameMapGenerate()
        {
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    Boolean check = true;
                    foreach (point p in bricks)
                    {
                        if (x == p.x && y == p.y)
                        {
                            MapData[y, x] = -1;
                            check = false;
                            break;
                        }
                    }
                    if (!check)
                        continue;
                    foreach (point p in water)
                    {
                        if (x == p.x && y == p.y)
                        {
                            MapData[y, x] = -1;
                            check = false;
                            break;
                        }
                    }
                    if (!check)
                        continue;
                    foreach (point p in stones)
                    {
                        if (x == p.x && y == p.y)
                        {
                            MapData[y, x] = -1;
                            check = false;
                            break;
                        }
                    }
                    if (!check)
                        continue;

                    if (check)
                    {
                        MapData[y, x] = 1;
                    }
                }

            }


        }
        
         // Test whether map generated succesfully
         
                public static int getMap(int x, int y)
                {
                    int yMax = 19;
                    int xMax = 19;
                    if (x < 0 || x > xMax)
                        return -1;
                    else if (y < 0 || y > yMax)
                        return -1;
                    else
                        return MapData[y, x];
                }

                static public void PrintSolution()
                {


                    for (int j = 0; j < 20; j++)
                    {
                        for (int i = 0; i < 20; i++)
                        {


                            if (getMap(i, j) == -1)
                                Console.Write("# "); //wall
                            else
                                Console.Write(". "); //road
                        }
                        Console.WriteLine("");
                    }
                }

                public int[,] passMap()
                {
                    return MapData;
                }
    } 
}


