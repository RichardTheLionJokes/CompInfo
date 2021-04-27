using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompInfo.Controllers;
using System.IO; //для работы с файлами

namespace CompInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            StreamReader objReader = new StreamReader(@"name_diapazon.txt", Encoding.Default);
            string sLine = "";
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null) AllCompNames.Items.Add(sLine);
            }
            objReader.Close();
        }

        private void GetAllNames_Click(object sender, EventArgs e)
        {
            List<string> compNameList = MainController.GetNamesFromAD();
            foreach (string compName in compNameList)
            {
                AllCompNames.Items.Add(compName);
            }
        }

        private void Scan_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.AppendText(MainController.hardwareScan(AllCompNames.SelectedItem.ToString()));
        }

        private void Ping_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.AppendText(MainController.PingComp(AllCompNames.SelectedItem.ToString()));
        }

        private void View_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.AppendText(MainController.GetHardwareFromDB(AllCompNames.SelectedItem.ToString()));
        }
    }
}
