using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace UDPAsiakas
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            int port = 9999;

            IPEndPoint iep = new IPEndPoint(IPAddress.Loopback, port);
            byte[] rec = new byte[256];

            EndPoint ep = (EndPoint)iep;
            s.ReceiveTimeout = 1000;
            String msg;
            Boolean on = true;

            do
            {
                Console.Write(">");
                msg = Console.ReadLine();
                if (msg.Equals("Q"))
                {
                    on = false;
                }
                else
                {
                    s.SendTo(Encoding.ASCII.GetBytes(msg), ep);

                    while (!Console.KeyAvailable)
                    {
                        IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                        EndPoint Palvelinep = (EndPoint)remote;
                        int paljon = 0;

                        try
                        {
                            paljon = s.ReceiveFrom(rec, ref Palvelinep);
                            // splitataan string, onko pituus 2
                            String rec_string = Encoding.ASCII.GetString(rec, 0, paljon);
                            char[] delim = { ';' };
                            String[] palat = rec_string.Split(delim, 2);

                            if (palat.Length < 2)
                            {
                                // lähetä virheviesti
                            }
                            else
                            {
                                Console.WriteLine("[{0}:{1}]", palat[0], palat[1]);
                            }

                        }
                        catch
                        {

                        }
                    }
                }

            } while (on);

            s.Close();
        }
    }
}
