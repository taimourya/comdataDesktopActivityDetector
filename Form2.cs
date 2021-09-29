using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebSocketSharp.Server;

namespace comdata_activiterDetector
{
    public partial class Form2 : Form
    {
        UserActivityHook actHook;
        WebSocketServer ws;
        bool cansend = false;
        public Form2()
        {
            InitializeComponent();

            ws = new WebSocketServer(8085);

            ws.Start();

            ws.AddWebSocketService<SendMove>("/Move");


            timer1.Start();

            this.Opacity = 0;
            this.ShowInTaskbar = false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            actHook = new UserActivityHook();

            actHook.OnMouseActivity += new MouseEventHandler(onMoved);
            actHook.KeyDown += new KeyEventHandler(onMoved);
            actHook.KeyPress += new KeyPressEventHandler(onMoved);
            actHook.KeyUp += new KeyEventHandler(onMoved);
        }

        //si le collaborateur utilise son clavier ou sa souris cette fonction est executé
        public void onMoved(object sender, EventArgs e)
        {
            //cansend va nous faire evité de spamer les clients websocket
            if(ws.IsListening && cansend)
            {
                ws.WebSocketServices["/Move"].Sessions.Broadcast("move");
                cansend = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cansend = true;
        }
    }
}
