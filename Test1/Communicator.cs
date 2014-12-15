using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;


namespace Test1
{
   public class Communicator
    {
        
        String serverIP;
        int port;
        NetworkStream netStream;
        public String serverResponse;
        TcpListener listenConection;
        TcpClient serverListener;

        public int loop;

        public Communicator()
        {

        }
      


       public void SendMsg(String msg)
        {

            TcpClient client = new TcpClient("127.0.0.1", 6000);

            netStream = client.GetStream();
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
            netStream.Write(data, 0, data.Length);
            netStream.Close();
            client.Close();
        }

       public void ServerLisnter(String serverIP, int port)
       {
           this.serverIP = serverIP;
           this.port = port;
           

           serverListener = new TcpClient();

           listenConection = new System.Net.Sockets.TcpListener(IPAddress.Parse(serverIP),port);
           listenConection.Start();


           

       }
       public void getServerResponse()
       {
           loop = 0;
               if (listenConection.Pending())
               {
                 
                   serverListener = listenConection.AcceptTcpClient();
                   netStream = serverListener.GetStream();

                   byte[] bytes = new byte[serverListener.ReceiveBufferSize];


                   netStream.Read(bytes, 0, (int)serverListener.ReceiveBufferSize);

                   serverResponse = Encoding.UTF8.GetString(bytes);

                  
                  loop =1;
                   
                  
                                  

              }
           
       }
            
    }
}

