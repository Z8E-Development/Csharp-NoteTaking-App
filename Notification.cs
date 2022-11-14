using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsNoteApp
{
    public partial class Notification : Form
    {
        public Notification()
        {
            InitializeComponent();
        }

        private void Notification_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width + 0, Screen.PrimaryScreen.WorkingArea.Height - this.Height + 0);
            label1.Text = Form1.notebox;
            label2.Text = Form1.titlebox;
            this.TopMost = true;
            //add a thing in the reminder panel that has a label = the current time when you click set so that you know when you set it
            //for the test im going to run on the four hour thing inside this form load thing make to so that the textbox = datetime.now
            //so when the form first loads it will tell me the time and when i wake up i can check the time it loaded
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Notification_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
