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
    public partial class Form2 : Form, StateObservable
    {
        UserActivityHook actHook;
        WebSocketServer ws;
        bool cansend = false;
        public Form2()
        {
            InitializeComponent();

            ws = new WebSocketServer(8085);

            ws.Start();

            ws.AddWebSocketService<MessageListener>("/");
            
            timer1.Start();

            this.Opacity = 0;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Left = Screen.PrimaryScreen.WorkingArea.Right - ((Screen.PrimaryScreen.WorkingArea.Size.Width + this.Width)/ 2); 
            this.Top = 0;
            this.TopMost = true;

            StateManager.addObservable(this);
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
                ws.WebSocketServices["/"].Sessions.Broadcast("move");
                cansend = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cansend = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form_pause form = new form_pause(this);
            form.ShowDialog();
        }

        public void changeVisibility(bool isVisible)
        {
            if (isVisible)
            {
                this.Opacity = 0.5;
            }
            else
            {
                this.Opacity = 0;
            }
        }
        public void onVisibilityChange(bool isVisible)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { changeVisibility(isVisible); }));
            }
            else
            {
                changeVisibility(isVisible);
            }
        }

        public void onTakingPause(int idPause)
        {
            ws.WebSocketServices["/"].Sessions.Broadcast("pause:"+idPause);
        }

        public void onTimeChange(int tactif, int tpause, int tinactif)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () {
                    if(tpause > 0)
                    {
                        button1.Visible = false;
                        button2.Visible = true;
                    }
                    else
                    {
                        button1.Visible = true;
                        button2.Visible = false;
                    }
                    label1.Text = tactif.ToString();
                    label2.Text = tpause.ToString();
                    label3.Text = tinactif.ToString();
                }));
            }
            else
            {
                label1.Text = tactif.ToString();
                label2.Text = tpause.ToString();
                label3.Text = tinactif.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ws.WebSocketServices["/"].Sessions.Broadcast("stopPause");
        }
    }
}
