using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace comdata_activiterDetector
{
    public class MessageListener : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            StateManager.changeVisibility(true);
        }
        protected override void OnClose(CloseEventArgs e)
        {
            if(this.Sessions.Sessions.Count() == 0)
                StateManager.changeVisibility(false);
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            //Send(e.Data);
            if (e.Data.StartsWith("types"))
            {
                //types = nom:1,nom2:2,nom3:3
                String value = e.Data.Split('=')[1].Trim();
                String[] types = value.Split(',');
                StateManager.types = new List<TypeItem>();
                foreach(String type in types)
                {
                    String[] item = type.Split(':');
                    TypeItem typeItem = new TypeItem(int.Parse(item[1]), item[0]);
                    StateManager.types.Add(typeItem);
                }
            }
            else if(e.Data.StartsWith("count"))
            {
                //count : actif,pause,inactif
                String value = e.Data.Split(':')[1].Trim();
                String[] splitedValue = value.Split(',');
                int tActif = int.Parse(splitedValue[0].Trim());
                int tPause = int.Parse(splitedValue[1].Trim());
                int tInactif = int.Parse(splitedValue[2].Trim());
                StateManager.changeTime(tActif, tPause, tInactif);
            }
        }
    }
}
