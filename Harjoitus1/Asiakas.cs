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
            Boolean on = true;
            while (on)
            {
                String snd = Console.ReadLine();
                byte[] msg = Encoding.ASCII.GetBytes(snd);
                s.Send(msg);
                byte[] rec = new byte[256];
                int count = s.Receive(rec);
                if (count == 0) on = false;
                // jos count == 0 -----> palvelin sulki yhteyden
                String rec_string = Encoding.ASCII.GetString(rec, 0, count);
                int puolipisteenIndeksi = rec_string.IndexOf(";");
                Console.Write("Palvelin: " + rec_string.Substring(0,puolipisteenIndeksi) + "\nteksti: " + snd + "\n");
            }
            Console.ReadKey();

            s.Close();
        }
    }
}
