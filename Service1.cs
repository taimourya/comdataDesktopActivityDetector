using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using WebSocketSharp.Server;

namespace comdata_activiterDetector
{
    partial class Service1 : ServiceBase
    {
        UserActivityHook actHook;
        WebSocketServer ws;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ws = new WebSocketServer(8085);

            ws.Start();

            ws.AddWebSocketService<SendMove>("/Move");
            this.startListen();
        }

        protected override void OnStop()
        {
            // TODO: ajoutez ici le code pour effectuer les destructions nécessaires à l'arrêt de votre service.
        }

        private void startListen()
        {
            actHook = new UserActivityHook();

            actHook.OnMouseActivity += new System.Windows.Forms.MouseEventHandler(onMoved);
            actHook.KeyDown += new System.Windows.Forms.KeyEventHandler(onMoved);
            actHook.KeyPress += new System.Windows.Forms.KeyPressEventHandler(onMoved);
            actHook.KeyUp += new System.Windows.Forms.KeyEventHandler(onMoved);
        }

        //si le collaborateur utilise son clavier ou sa souris cette fonction est executé
        public void onMoved(object sender, EventArgs e)
        {
            ws.WebSocketServices["/Move"].Sessions.Broadcast("move");
        }

    }
}
