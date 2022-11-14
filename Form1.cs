using System;
using System.Data;
using System.Windows.Forms;
using WindowsFormsNoteApp.Properties;
using System.IO;

namespace WindowsFormsNoteApp
{
    public partial class Form1 : Form
    {
        DataTable notes = new DataTable();
        
        bool editing = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            if (notes.Columns.Count < 1)
            {
                notes.Columns.Add("Title");
                notes.Columns.Add("    Time");
                notes.Columns.Add("Note").ColumnName = "Note";
            }
            NoteList.DataSource = notes;
            NoteList.Columns["Note"].Visible = false;
            NoteList.Columns["    Time"].Width = 66;
            string[] lines = File.ReadAllLines(@"notes.txt");
            string[] data;

            for (int i = 0; i < lines.Length; i++)
            {
                data = lines[i].ToString().Split('|');

                string[] row = new string[data.Length];

                for (int j = 0; j < data.Length; j++)
                {
                    row[j] = data[j].Trim();
                }
                notes.Rows.Add(row);
            }
            RedBtn.Text = Settings.Default.RedTitle;
            YellowBtn.Text = Settings.Default.YellowTitle;
            GreenBtn.Text = Settings.Default.GreenTitle;
        }
        string stupidstring = "blank";
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (notes.Rows.Count != 0)
            {
                if(stupidstring != "blank")
                {
                    if(stupidstring == "red")
                    {
                        Settings.Default.Red = "";
                        Settings.Default.RedTitle = "Title";
                        RedBtn.Text = "Title";
                        Notebox.Text = "";
                        Titlebox.Text = "";
                        panel2.Visible = false;
                    }
                    else if(stupidstring == "yellow")
                    {
                        Settings.Default.Yellow = "";
                        Settings.Default.YellowTitle = "Title";
                        YellowBtn.Text = "Title";
                        Notebox.Text = "";
                        Titlebox.Text = "";
                        panel3.Visible = false;
                    }
                    else if(stupidstring == "green")
                    {
                        Settings.Default.Green = "";
                        Settings.Default.GreenTitle = "Title";
                        GreenBtn.Text = "Title";
                        Notebox.Text = "";
                        Titlebox.Text = "";
                        panel4.Visible = false;
                    }
                }
                else
                {
                    try
                    {
                        notes.Rows[NoteList.CurrentCell.RowIndex].Delete();
                        Titlebox.Text = "";
                        Notebox.Text = "";
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Not A Valid Note");
                    }
                }
            }
            //DeleteBtn.Enabled = false;
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Titlebox.Text = notes.Rows[NoteList.CurrentCell.RowIndex].ItemArray[0].ToString();
                Notebox.Text = notes.Rows[NoteList.CurrentCell.RowIndex].ItemArray[2].ToString();
                editing = true;
            }
            catch (Exception)
            {

            }
        }

        private void NewBtn_Click(object sender, EventArgs e)
        {
            editing = false;
            stupidstring = "blank";
            Titlebox.Text = "";
            Notebox.Text = "";
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {

            if(stupidstring != "blank")
            {
                if(stupidstring == "red")
                {
                    Settings.Default.Red = Notebox.Text;
                    Settings.Default.RedTitle = Titlebox.Text;
                    RedBtn.Text = Titlebox.Text;
                }
                else if (stupidstring == "yellow")
                {
                    Settings.Default.Yellow = Notebox.Text;
                    Settings.Default.YellowTitle = Titlebox.Text;
                    YellowBtn.Text = Titlebox.Text;
                }

                else if (stupidstring == "green")
                {
                    Settings.Default.Green = Notebox.Text;
                    Settings.Default.GreenTitle = Titlebox.Text;
                    GreenBtn.Text = Titlebox.Text;
                }
            }
            else if(stupidstring == "blank")
            {
                if (editing)
                {
                    string noNewline = Notebox.Text.Replace("\n", "   ");
                    notes.Rows[NoteList.CurrentCell.RowIndex]["Title"] = Titlebox.Text;
                    notes.Rows[NoteList.CurrentCell.RowIndex]["Note"] = noNewline;
                    notes.Rows[NoteList.CurrentCell.RowIndex]["    Time"] = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                    string noNewline = Notebox.Text.Replace("\n", "    ");
                    notes.Rows.Add(Titlebox.Text, DateTime.Now.ToString("dd/MM/yyyy"), noNewline);
                }
                Titlebox.Text = "";
                Notebox.Text = "";
                stupidstring = "blank";
                editing = false;
            }
        }

        private void NoteList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Titlebox.Text = notes.Rows[NoteList.CurrentCell.RowIndex].ItemArray[0].ToString();
                Notebox.Text = notes.Rows[NoteList.CurrentCell.RowIndex].ItemArray[2].ToString();
                editing = true;
                stupidstring = "blank";
                if(notes.DefaultView.RowFilter != string.Empty)
                {
                    notes.DefaultView.RowFilter = string.Empty;
                }
            }
            catch (Exception)
            {

            }
        }

        private void NoteList_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (panel1.Visible == true)
            {
                panel1.Visible = false;
            }

            else if (panel1.Visible == false)
            {
                panel1.Visible = true;
            }
        }

        private void PanelDelBtn_Click(object sender, EventArgs e)
        {
            if (notes.Rows.Count != 0)
            {
                try
                {
                    notes.Rows[NoteList.CurrentCell.RowIndex].Delete();
                    Titlebox.Text = "";
                    Notebox.Text = "";
                    panel1.Visible = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("Not A Valid Note");
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            TextWriter Writer = new StreamWriter(@"notes.txt");
            for(int i = 0; i < NoteList.Rows.Count - 1; i++)
            {
                for(int j = 0; j < NoteList.Columns.Count; j++)
                {
                    if (j == NoteList.Columns.Count - 1)
                        Writer.Write("\t" + NoteList.Rows[i].Cells[j].Value.ToString());

                    else
                    Writer.Write("\t" + NoteList.Rows[i].Cells[j].Value.ToString() + "\t" + "|");
                }
                Writer.WriteLine("");
            }
            Writer.Close();
            Settings.Default.Save();
        }

        private void PanelEditBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Titlebox.Text = notes.Rows[NoteList.CurrentCell.RowIndex].ItemArray[0].ToString();
                Notebox.Text = notes.Rows[NoteList.CurrentCell.RowIndex].ItemArray[2].ToString();
                editing = true;
            }
            catch (Exception)
            {

            }
        }

        private void PanelCopyButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(notes.Rows[NoteList.CurrentCell.RowIndex]["Note"].ToString());
        }

        private void GreenbtnOption_Click(object sender, EventArgs e)
        {
            if (panel4.Visible == true)
            {
                panel4.Visible = false;
            }

            else if (panel4.Visible == false)
            {
                panel4.Visible = true;
            }

            if(panel2.Visible == true || panel3.Visible == true)
            {
                panel3.Visible = false;
                panel2.Visible = false;
            }
            panel4.BringToFront();
        }

        private void YellowBtnOption_Click(object sender, EventArgs e)
        {
            if (panel3.Visible == true)
            {
                panel3.Visible = false;
            }

            else if (panel3.Visible == false)
            {
                panel3.Visible = true;
            }

            if (panel2.Visible == true || panel4.Visible == true)
            {
                panel4.Visible = false;
                panel2.Visible = false;
            }
            panel3.BringToFront();
        }

        private void RedBtnOption_Click(object sender, EventArgs e)
        {
            if (panel2.Visible == true)
            {
                panel2.Visible = false;
            }

            else if (panel2.Visible == false)
            {
                panel2.Visible = true;
            }

            if (panel4.Visible == true || panel3.Visible == true)
            {
                panel3.Visible = false;
                panel4.Visible = false;
            }
            panel2.BringToFront();
        }
        
        private void RedBtn_Click(object sender, EventArgs e)
        {
            stupidstring = "red";
            Notebox.Text = Settings.Default.Red;
            Titlebox.Text = Settings.Default.RedTitle;
        }

        private void YellowBtn_Click(object sender, EventArgs e)
        {
            stupidstring = "yellow";
            Notebox.Text = Settings.Default.Yellow;
            Titlebox.Text = Settings.Default.YellowTitle;
        }

        private void Greenbtn_Click(object sender, EventArgs e)
        {
            stupidstring = "green";
            Notebox.Text = Settings.Default.Green;
            Titlebox.Text = Settings.Default.GreenTitle;
        }

        private void RedPanelBtnDel_Click(object sender, EventArgs e)
        {
            Settings.Default.Red = "";
            Settings.Default.RedTitle = "Title";
            RedBtn.Text = "Title";
            Notebox.Text = "";
            Titlebox.Text = "";
            panel2.Visible = false;
        }

        private void YellowPanelDelBtn_Click(object sender, EventArgs e)
        {
            Settings.Default.Yellow = "";
            Settings.Default.YellowTitle = "Title";
            YellowBtn.Text = "Title";
            Notebox.Text = "";
            Titlebox.Text = "";
            panel3.Visible = false;
        }

        private void GreenPanelDelBtn_Click(object sender, EventArgs e)
        {
            Settings.Default.Green = "";
            Settings.Default.GreenTitle = "Title";
            GreenBtn.Text = "Title";
            Notebox.Text = "";
            Titlebox.Text = "";
            panel4.Visible = false;
        }

        private void RedPanelCopyBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(Settings.Default.Red);
        }

        private void YellowPanelCopyBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(Settings.Default.Yellow);
        }

        private void GreenPanelCopyBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(Settings.Default.Green);
        }

        private void SearchDGV_Click(object sender, EventArgs e)
        {
            if(searchTextBox.Text != string.Empty)
            {
                notes.DefaultView.RowFilter = string.Format("Title LIKE '%{0}%'", searchTextBox.Text);
                searchTextBox.Clear();
            }
        }

        private void RedPnlCpyFleBtn(object sender, EventArgs e)
        {
            StreamWriter redsw = new StreamWriter(@"" + RedBtn.Text+".txt");
            redsw.Write(Settings.Default.Red.ToString());
            redsw.Close();
        }

        private void YellwPnlCpyFleBtn(object sender, EventArgs e)
        {
            StreamWriter yellowsw = new StreamWriter(@"" + YellowBtn.Text+".txt");
            yellowsw.Write(Settings.Default.Yellow.ToString());
            yellowsw.Close();
        }
        //            StreamWriter sw = new StreamWriter(@"FormatedText.txt"); C:\Users\ZekeR\Desktop\
        private void GreenPnlCpyFleBtn(object sender, EventArgs e)
        {
            StreamWriter greensw = new StreamWriter(@"" + GreenBtn.Text+".txt");
            greensw.Write(Settings.Default.Green.ToString());
            greensw.Close();
        }
        public static string notebox = "";
        public static string titlebox = "";
        private static int days = 0;
        private static int hours = 0;
        private static int minutes = 0;
        private void ReminderBtn_Click(object sender, EventArgs e)
        {
            panel5.Visible = true;
        }

        private void PnlSetBtn_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            label6.Text = "Reminder Set For " + days.ToString() +"D: "+ hours.ToString() +"H: "+ minutes.ToString() + "M";
            label7.Text = "Reminder Set At: " + DateTime.Now.ToString("dddd dd: hh:mm:tt");
        }

        private void PnlStopbtn_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer1.Interval = 700;
            days = 0;
            hours = 0;
            minutes = 0;
            label3.Text = "Days: ";
            label4.Text = "Hours: ";
            label5.Text = "Minutes: ";
            label6.Text = "Reminder Stoped";
            label7.Text = "";
        }

        private void PlusDays_Click(object sender, EventArgs e)
        {
            days = days + 1;
            label3.Text = "Days: " + days.ToString();
            timer1.Interval += 86400000;
        }

        private void PlusHours_Click(object sender, EventArgs e)
        {
            hours = hours + 1;
            label4.Text = "Hours: " + hours.ToString();
            timer1.Interval += 3600000;
        }

        private void PlusMinutes_Click(object sender, EventArgs e)
        {
            minutes = minutes + 1;
            label5.Text = "Minutes: " + minutes.ToString();
            timer1.Interval += 60000;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Notification noti = new Notification();
            notebox = Notebox.Text;
            titlebox = Titlebox.Text;
            noti.Show();
            timer1.Enabled = false;
        }

        private void PnlExitBtn_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            label7.Text = "";
            panel5.Hide();
        }
    }
}
