using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace HTTPAsiakas
{
    class HTTPAsiakas
    {
        static void Main(string[] args)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            s.Connect("localhost", 25000);
            String snd = "GET / HTTP/1.1\r\nHost: localhost\r\n\r\n";
            byte[] msg = Encoding.ASCII.GetBytes(snd);
            s.Send(msg);
            byte[] rec = new byte[256];
            Boolean on = true;
            while (on)
            {
                int count = s.Receive(rec);
                if (count == 0) on = false;
                // jos count == 0 -----> palvelin sulki yhteyden
                String rec_string = Encoding.ASCII.GetString(rec, 0, count);
                Console.Write(rec_string);
            }
            Console.ReadKey();

            s.Close();
        }
    }
}
