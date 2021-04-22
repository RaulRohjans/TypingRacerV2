using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Typing_Racer_V2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private readonly int SECONDS = 60;

        int wordCnt = 0;
        string path = "";
        string contentOG = "";
        List<char> contentTemp = new List<char>();

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Welcome to the typing racer game, made by RAUL ROHJANS!\n\nEnjoy :)", "Typing Racer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        private void btn_start_Click(object sender, EventArgs e)
        {
            btn_start.Enabled = false;
            btn_stop.Enabled = true;
            txt_word.ReadOnly = false;
            wordCnt = 0;
            lbl_time.Text = SECONDS.ToString();

            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text|*.txt|All|*.*";
            open.RestoreDirectory = true;

            if (open.ShowDialog() == DialogResult.OK)
            {
                path = open.FileName;
                StreamReader reader = new StreamReader(open.OpenFile());
                contentOG = reader.ReadToEnd();

                string tempStr = contentOG.Replace('\r', ' ');
                char[] Temp = tempStr.Replace('\n', ' ').ToCharArray();
                
                //Fill list
                for(int i = 0; i < Temp.Length; i++)
                {
                    if (Temp[i] == ' ' && Temp[i + 1] == ' ')
                        continue;

                    contentTemp.Add(Temp[i]);
                }

                txt_text.Text = new string(contentTemp.ToArray());
                timer.Enabled = true;
            }
            else
            {
                btn_start.Enabled = true;
                btn_stop.Enabled = false;
                txt_word.ReadOnly = true;
            }

        }

        private void btn_leave_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure you want to leave?", "Leave Game?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
                Application.Exit();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_word.BackColor = Color.White;
            if (e.KeyChar == ' ')
            {                
                int idx = 0;
                bool correct = true;
                char[] tempchar = txt_word.Text.ToCharArray();
                while(contentTemp[idx] != ' ')
                {
                    if(contentTemp[idx] != tempchar[idx])
                    {
                        txt_word.BackColor = Color.Red;
                        correct = false;
                        break;
                    }
                    idx++;
                }

                if(correct)
                {
                    txt_word.Text = "";

                    //Remove word from List
                    for(int i = 0; i <= idx; i++)
                    {
                        contentTemp.RemoveAt(0);
                    }

                    txt_text.Text = new string(contentTemp.ToArray());
                    wordCnt++;
                }

                e.Handled = true;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (lbl_time.Text != "0" && lbl_time.Text != "00")
            {
                lbl_time.Text = Convert.ToString(Convert.ToInt32(lbl_time.Text) - 1);
            }
            else
            {
                timer.Enabled = false;
                lbl_time.Text = "0";

                MessageBox.Show("The time is up!\n\nYou scored " + wordCnt / (SECONDS / 60.0) + " WPM (Words per Minute)", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btn_start.Enabled = true;
                btn_stop.Enabled = false;
                txt_word.ReadOnly = true;
            }
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            btn_start.Enabled = true;
            btn_stop.Enabled = false;
            txt_word.ReadOnly = true;
            MessageBox.Show("Aww, you canceled the game :(\n\nYou scored " + wordCnt / ((SECONDS - Convert.ToDouble(lbl_time.Text)) / 60.0) + " WPM (Words per Minute)", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            timer.Enabled = false;
        }
    }
}
