using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace TCPPalvelin
{
    /// <summary>
    /// 
    /// </summary>
    class TCPPalvelin
    {
        static void Main(string[] args)
        {
            Socket p = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 25000);

            p.Bind(iep);
            p.Listen(2);
            Boolean auki = true;

            while (auki)
            {

                Socket asiakas = p.Accept();

                Boolean on = true;

                while (on)
                {
                    byte[] rec = new byte[256];
                    int count = asiakas.Receive(rec);
                    String rec_string = Encoding.ASCII.GetString(rec, 0, count);
                    if (rec_string.Equals("q"))
                    {
                        on = false;
                    }
                    else if (rec_string.Equals("sulje"))
                    {
                        auki = false;
                    }
                    Console.Write(rec_string);

                    String erotin = ";";


                    String snd = "Santerin palvelin" + erotin + rec_string;
                    byte[] msg = Encoding.ASCII.GetBytes(snd);
                    asiakas.Send(msg);
                }
                asiakas.Close();
            }

            Console.ReadKey();
            p.Close();
        }
    }
}
