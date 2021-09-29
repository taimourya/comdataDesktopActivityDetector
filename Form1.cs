using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace comdata_activiterDetector
{
    public partial class Form1 : Form
    {

        UserActivityHook actHook;
        TcpListener server;
        TcpClient client = null;
        WebClient webClient = null;
        private StreamWriter clientStreamWriter = null;

        bool canSend = false;

        public Form1()
        {
            InitializeComponent();
            this.Opacity = 1;

            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8085);


            server.Start();

            client = server.AcceptTcpClient();

            /*Stream s = client.GetStream();
            s.Write(h, 0, h.Length);*/

            while(true)
            {
                //while (client.Available < 3) ;

                NetworkStream stream = client.GetStream();
                Byte[] bytes = new Byte[client.Available];

                stream.Read(bytes, 0, bytes.Length);
                String data = Encoding.UTF8.GetString(bytes);
                MessageBox.Show(data);
                if (Regex.IsMatch(data, "^GET"))
                {
                    const string eol = "\r\n";
                    Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + eol
                        + "Connection: Upgrade" + eol
                        + "Upgrade: websocket" + eol
                        + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                            System.Security.Cryptography.SHA1.Create().ComputeHash(
                                Encoding.UTF8.GetBytes(
                                    new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                                )
                            )
                        ) + eol
                        + eol);
                    stream.Write(response, 0, response.Length);

                }
                else
                {

                }

                Byte[] toSend = Encoding.UTF8.GetBytes("move");
                //stream.Write(toSend, 0, toSend.Length);
                Stream s = client.GetStream();
                s.Write(toSend, 0, toSend.Length); 
            }


        }

        public void sendOnMove()
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            actHook = new UserActivityHook(); 
                                              
            actHook.OnMouseActivity += new MouseEventHandler(onMoved);
            actHook.KeyDown += new KeyEventHandler(onMoved);
            actHook.KeyPress += new KeyPressEventHandler(onMoved);
            actHook.KeyUp += new KeyEventHandler(onMoved);
            timer1.Start();
        }

		public void onMoved(object sender, EventArgs e)
        {
            if (client != null)
            {
                
                clientStreamWriter = new StreamWriter(client.GetStream());
                clientStreamWriter.WriteLine("move");
                canSend = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            canSend = true;
        }
    }
}
