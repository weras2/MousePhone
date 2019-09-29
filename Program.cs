using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KentHackMouse
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)] public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;


        public static double[] StringToArray(String stringArr)
        {
            int index = stringArr.IndexOf("a");

            String firstNumb = stringArr.Substring(0, index);
            String secondNumb = stringArr.Substring(index + 1);

            double[] tmpArr = { Convert.ToDouble(firstNumb), Convert.ToDouble(secondNumb) };
            return tmpArr;
        }


        public static void Main()
        {

            Cursor.Position = new Point(0, 0);



            try
            {
                IPAddress ipAd = IPAddress.Parse("10.7.102.148");
                // use local m/c IP address, and 

                // use the same in the client


                /* Initializes the Listener */
                TcpListener myList = new TcpListener(ipAd, 11000);

                /* Start Listeneting at the specified port */
                myList.Start();

                //Console.WriteLine("The server is running at port 1100...");
                //Console.WriteLine("The local End point is  :" + myList.LocalEndpoint);
                //Console.WriteLine("Waiting for a connection.....");
            m:
                Socket s = myList.AcceptSocket();
                //Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

                byte[] b = new byte[100];
                int k = s.Receive(b);
                //Console.WriteLine("Recieved: " + k);
                char cc = ' ';
                string test = null;
                //Console.WriteLine("Recieved...");
                for (int i = 0; i < k - 1; i++)
                {
                    //Console.Write(Convert.ToChar(b[i]));
                    cc = Convert.ToChar(b[i]);
                    test += cc.ToString();
                }


                if (test.Equals("1"))
                {

                    int X = Cursor.Position.X;
                    int Y = Cursor.Position.Y;
                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);

                }
                else
                {
                    double[] myArr = StringToArray(test);
                    Console.WriteLine(myArr[0]);
                    Console.WriteLine(myArr[1]);

                    Cursor.Position = new Point((int)myArr[0], (int)myArr[1]);
                }

                









                ASCIIEncoding asen = new ASCIIEncoding();
                s.Send(asen.GetBytes("The string was recieved by the server."));
                //Console.WriteLine("\nSent Acknowledgement");


                /* clean up */
                goto m;
                s.Close();
                myList.Stop();
                Console.ReadLine();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
    }
}
