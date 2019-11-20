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
    public partial class Attendance : Form
    {
        public Attendance()
        {
            InitializeComponent();
        }

        int[] a = {0,0,0,0 };

        private void Button1_Click(object sender, EventArgs e)
        {
            List<string> PresentStudents = new List<string>();
            String show = "CourseName:\t\t"+temp.courseName+"\nTotal Students:\t\t4"+"\nStudents Present:\n";
            if (a[0]==1)
            {
                PresentStudents.Add("Manan");
                show += "Manan\n";
            }
            if (a[1] == 1)
            {
                PresentStudents.Add("Murtaza");
                show += "Murtaza\n";
            }
            if (a[2] == 1)
            {
                PresentStudents.Add("Imtiaz");
                show += "Imtiaz\n";
            }
            if (a[3] == 1)
            {
                PresentStudents.Add("Yasir");
                show += "Yasir\n";
            }


            //MessageBox.Show(PresentStudents.Count.ToString(), "Count");

            String filePath = @"E:\Sem7\HCI\ProjAttendanceSystem\HCI\AttendanceSheets\"+temp.courseName+".csv";
            List<string> lines = File.ReadAllLines(filePath).ToList();

            //MessageBox.Show(lines.Count.ToString(),"Count");
            //add new column to the header row
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            string time = DateTime.Now.ToString("hh tt");

            lines[0] += ","+date+" ("+time+")";
            int index = 1;
            //add new column value for each row.
            lines.Skip(1).ToList().ForEach(line =>
            {
               // MessageBox.Show(lines[index++].ToString().Split(',')[0], "Names");
                if (PresentStudents.Contains(lines[index].Split(',')[0]))
                {
                    lines[index++] += "," + "P";
                }
                else
                {
                    lines[index++] += "," + "A";
                }
            });
                //write the new content
            File.WriteAllLines(filePath, lines);

            //************************************
            this.Hide();
            MessageBox.Show(show,"Attendance");
            new Form1().Show();//showing main screen again

        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            a[0] = 1;//Manan
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            a[1] = 1;//Murtaza

        }

        private void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            a[2] = 1;//imtiaz

        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            a[3] = 1;//yasir

        }


        private void Attendance_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            new Form1().Show();//Show main screen again
        }
    }
}
