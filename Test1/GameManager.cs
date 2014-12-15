using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using Test1.AI;
using System.Diagnostics;

namespace Test1
{
    public class GameManager
    {
        Communicator join;
        Command newCommand = new Command();
        public String serverResponse;
        int count = 0;
        GameEngine newGame = new GameEngine();
        Boolean coinCheck = false;


        public void GameInitiation()
        {
            join = new Communicator();
            join.SendMsg("JOIN#");
            join.ServerLisnter("127.0.0.1", 7000);
            while (true)
            {
               // Console.WriteLine(count);
                join.getServerResponse();
                if (join.loop == 1)
                {
                    serverResponse = join.serverResponse;
                   // Console.Write(serverResponse);
                    newGame.AnalyseServerResponse(serverResponse);
                   // if (count > 2)
                    {
                        if (serverResponse.StartsWith("C"))
                        {
                           // if (!coinCheck)
                            {
                                coinCheck = true;
                               // newCommand.selectCoinPile();
                            }
                        }
                        if (serverResponse.StartsWith("G"))
                        {
                            //get the return from a method and then
                            //if (coinCheck)
                            {
                                Stopwatch stopwatch = new Stopwatch();
                                stopwatch.Start();
                                newCommand.selectCoinPile();
                                String msg =newCommand.getPath();
                                join.SendMsg(msg);
                                stopwatch.Stop();
                                Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
                            }
                            
                        }
                    }
                    count++;
                }

            }
           

        }
    }
}
