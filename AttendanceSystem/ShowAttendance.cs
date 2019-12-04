using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AttendanceSystem
{
    public partial class ShowAttendance : Form
    {
        public ShowAttendance()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            String name = comboBox1.Text;
            string filepath = @"E:\Sem7\HCI\ProjAttendanceSystem\HCI\AttendanceSheets\"+name+".csv";
            DataTable dt = new DataTable();
            string[] lines = File.ReadAllLines(filepath);

            if (lines.Length > 0)
            {
                string firstLine = lines[0];
                string[] headerLabels = firstLine.Split(',');

                foreach (string headerWord in headerLabels)
                {
                    dt.Columns.Add(new DataColumn(headerWord));
                }

                for (int r = 1; r < lines.Length; r++)
                {
                    string[] dataWords = lines[r].Split(',');
                    DataRow dr = dt.NewRow();

                    int columnIndex = 0;

                    foreach (string headerWord in headerLabels)
                    {
                        dr[headerWord] = dataWords[columnIndex++];
                    }

                    dt.Rows.Add(dr);
                }
            }

            if (dt.Rows.Count > 0)
            {
                csvDisplay.DataSource = dt;
            }
        }

        private void ShowAttendance_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            return;
        }
    }
}
