using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace comdata_activiterDetector
{
    public partial class form_pause : Form, StateObservable
    {
        Form2 parent;
        public form_pause(Form2 parent)
        {
            InitializeComponent();

            this.parent = parent;

            StateManager.addObservable(this);

            foreach(TypeItem item in StateManager.types)
            {
                comboBox1.Items.Add(item);
            }

            comboBox1.ValueMember = "Nom";
            comboBox1.SelectedIndex = 0;
        }

        public void onTimeChange(int tactif, int tpause, int tinactif)
        {
            
        }

        public void onVisibilityChange(bool isVisible)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () {
                    this.Close(); 
                }));
            }
            else
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TypeItem selectedItem = (TypeItem)comboBox1.SelectedItem;
            this.parent.onTakingPause(selectedItem.Id);
            this.Close();
        }
    }
}
