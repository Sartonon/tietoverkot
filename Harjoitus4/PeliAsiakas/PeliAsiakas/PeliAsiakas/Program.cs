using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace PeliAsiakas
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket palvelin = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 9999);
            
            EndPoint Pep = (IPEndPoint)ep;

            Laheta(palvelin, Pep, "JOIN Santeri");

            Boolean on = true;
            String TILA = "JOIN";

            while(on)
            {
                String[] palat = Vastaanota(palvelin);
                switch (TILA)
                {
                    case "JOIN":
                        switch (palat[0])
                        {
                            case "ACK":
                                switch (palat[1])
                                {
                                    case "201":
                                        Console.WriteLine("Odotetaan toista pelaajaa");
                                        break;
                                    case "202":
                                        Console.WriteLine("Vastustajasi {0} on" + palat[2]);
                                        Console.WriteLine("Anna numero väliltä 0-10 ");
                                        String luku = Console.ReadLine();
                                        Laheta(palvelin, Pep, "DATA " + luku);
                                        TILA = "GAME";
                                        break;
                                    case "203":
                                        Console.WriteLine("Vastustaja {0} saa aloittaa" + palat[2]);
                                        TILA = "GAME";
                                        break;
                                    default:
                                        Console.WriteLine("Virhe " + palat[1]);
                                        break;

                                } // switch palat[1]
                                break;
                            default:
                                Console.WriteLine("Virhe " + palat[0]);
                                break;
                        }
                        break;
                    case "GAME":
                        switch (palat[0])
                        {
                            case "ACK":
                                switch (palat[1].Substring(0, 1))
                                {
                                    case "3":
                                        Console.WriteLine(palat[0] + " " + palat[1] +
                                            " DATA OK!");
                                        break;
                                    case "4":
                                        switch (palat[1])
                                        {
                                            case "402":
                                                Console.WriteLine(palat[0] + " " + palat[1] +
                                                    " Ei sinun vuorosi!");
                                                break;
                                            default:
                                                Console.WriteLine(palat[0] + " " + palat[1] +
                                                    " Tapahtui jokin virhe...");
                                                String joku = Console.ReadLine();
                                                Laheta(palvelin, Pep, joku);
                                                break;
                                        }
                                        break;

                                    default:
                                        Console.WriteLine(palat[0] + " " + palat[1] +
                                            " Tapahtui jokin virhe...");
                                        break;
                                }
                                break;
                            case "DATA":
                                Console.WriteLine("Vastustajan arvaus: " + palat[1]);
                                Console.WriteLine("Vahvista");
                                String ack = Console.ReadLine();
                                Laheta(palvelin, Pep, ack);
                                Console.WriteLine("Anna numero > ");
                                String arvattu = Console.ReadLine();
                                Laheta(palvelin, Pep, arvattu);
                                break;
                            case "QUIT":
                                switch (palat[1])
                                {
                                    case "501":
                                        Console.WriteLine(palat[0] + " " + palat[1] +
                                            " Voitit pelin!");
                                        String poisV = Console.ReadLine();
                                        Laheta(palvelin, Pep, poisV);
                                        break;
                                    case "502":
                                        Console.WriteLine(palat[0] + " " + palat[1] +
                                            " Vastustajasi voitti pelin...");
                                        String poisH = Console.ReadLine();
                                        Laheta(palvelin, Pep, poisH);
                                        break;
                                    default:
                                        Console.WriteLine(palat[0] + " " + palat[1] +
                                            " Jotain määrittelemätöntä tapahtui.");
                                        break;
                                }
                                break;
                            default:
                                String arvaus = Console.ReadLine();
                                Laheta(palvelin, Pep, arvaus);
                                Console.WriteLine(palat[0] + " " + palat[1] +
                                    " Tapahtui virhe...");
                                break;
                        }
                        break;
                } // tila
            } // while
        }

        private static string[] Vastaanota(Socket palvelin)
        {
            byte[] rec = new byte[256];
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Palvelinep = (EndPoint)remote;
            int paljon = 0;
            paljon = palvelin.ReceiveFrom(rec, ref Palvelinep);
            String rec_string = Encoding.ASCII.GetString(rec, 0, paljon);
            String[] palat = rec_string.Split(' ');
            return palat;

        }

        private static void Laheta(Socket palvelin, EndPoint Pep, string p)
        {
            palvelin.SendTo(Encoding.ASCII.GetBytes(p), Pep);
        }
    }
}
