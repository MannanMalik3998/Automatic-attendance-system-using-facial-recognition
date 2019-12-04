using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AttendanceSystem
{
    public partial class Select : Form
    {
        public Select()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            /*
            //Taking inputs from dropdown menu
            if(comboBox1.Text==""){//empty
                this.Hide();
                
                new Select().Show();
            }

            if (comboBox2.Text == "") {//empty
                this.Hide();
                new Select().Show();
            }
            */
            temp.courseName = comboBox1.Text;
            String camSrc = comboBox2.Text;
            //MessageBox.Show("", "");
            string course = temp.courseName;
            int camSource = 0;
            if (camSrc.Contains("Laptop"))
            {
                camSource = 0;
            }
            else if (camSrc.Contains("External"))
            {
                camSource = 1;
            }
            else camSource = 2;
            
            //******************************************************************************************************************************************************

            if (camSource == 2)
            {//Taking attendance Manually
                this.Close();
                new Attendance().Show();
                return;
            }

            string fileName = @"E:\Sem7\HCI\ProjAttendanceSystem\HCI\recognize_faces_video.py";    //python script to run    

            #region Execute python script

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(@"C:\Python37\python.exe", fileName)
            {
                Arguments = string.Format("{0} {1} {2}", fileName, course, camSource),//command line arguments
                RedirectStandardOutput = true,//passing of parameters
                UseShellExecute = false,
                CreateNoWindow = false,//for opening facial recognition window, set it to false

            };

            p.Start();//runs the script
            this.Close();//hides the main screen

            string output = p.StandardOutput.ReadToEnd();//returns output of script
            p.WaitForExit();

            int ErrorResult = checkError(output);
            #endregion

            if (ErrorResult == -1)
            {//error
                return;
            }
            else
            {//no error

                string[] a = output.Split('\n');

                //initializing variables
                string courseName = a[0];
                string totalStudents = a[1];
                string totPresent = a[2];
                string[] students = { "Manan", "Murtaza", "Yasir", "Imtiaz" };
                string pres = "";
                List<string> presStudents = new List<string>();

                for (int i = 3; i < a.Length - 1; i++)
                {
                    presStudents.Add(a[i]);//adding present students in list

                }


                foreach (string i in presStudents)
                {
                    pres += i;
                    pres += "\n";
                }

                //Displaying current attendance
                MessageBox.Show("CourseName:\t\t" + courseName + "\nTotal Students:\t\t" + totalStudents + "\nTotal Students Present:\t" + totPresent + "\nPresent Students:\n" + pres, "Automatic Attendance System -> Select -> Attendance");

                //Saving current attendance in text file
                System.IO.File.WriteAllText(@"E:\Sem7\HCI\ProjAttendanceSystem\HCI\log.txt", "CourseName:\t\t" + courseName + "\nTotal Students:\t\t" + totalStudents + "\nTotal Students Present:\t" + totPresent + "\nPresent Students:\n" + pres);

                //Relaunching main screen
                this.Close();
                

            }
            this.Hide();
        }
        public int checkError(string output)
        {
            if (output.Contains("-1"))
            {//if an error occured during execution such as camera malfunction
                MessageBox.Show("An error occurred during execution\nKindly check whether the camera is working properly", "Automatic Attendance System -> Error");

                return -1;
            }
            return 0;//if everything runs smoothly without any error
        }

        private void Select_FormClosing(object sender, FormClosingEventArgs e)
        {
            return;
        }


        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
