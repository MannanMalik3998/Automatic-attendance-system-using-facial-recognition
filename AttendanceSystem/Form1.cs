using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
namespace AttendanceSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ToolTip t1 = new ToolTip();
        private void Button1_Click(object sender, EventArgs e)
        {

            
            string input = Interaction.InputBox("Add CourseName\n1)HCI\n2)IPT\n3)PIT\n4)IS", "Automatic Attendance System -> Course Name", "HCI", -1, -1);

            if (input == "") {
                //MessageBox.Show("", "Automatic Attendance System -> Error");
                //System.Windows.Forms.Application.Exit();
                input = Interaction.InputBox("Add CourseName\n1)HCI\n2)IPT\n3)PIT\n4)IS", "Must specify course name", "HCI", -1, -1);
            }
            string cam = Interaction.InputBox("Enter 0 to use webcam\nEnter 1 to use external webcam", "Automatic Attendance System -> Specify Camera", "0", -1, -1);

            if (cam == "")
            {
                //System.Windows.Forms.Application.Exit();

                cam = Interaction.InputBox("Enter 0 to use webcam\nEnter 1 to use external webcam", "Must specify camera", "0", -1, -1);
            }

            int camSource = int.Parse(cam);
            //string fileName = @"E:\Sem7\HCI\lol.py";            
            string fileName = @"E:\Sem7\HCI\ProjAttendanceSystem\HCI\recognize_faces_video.py";        
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(@"C:\Python37\python.exe", fileName) {
                Arguments = string.Format("{0} {1} {2}", fileName, input,camSource),
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                
            };
            p.Start();
            this.Hide();
            string output = p.StandardOutput.ReadToEnd();

            
            p.WaitForExit();
            if (output.Contains("-1"))
            {
                MessageBox.Show("An error occurred during execution\nKindly check whether the camera is working properly and then start the application again", "Automatic Attendance System -> Error");
                System.Windows.Forms.Application.Exit();
            }
            string []a = output.Split('\n');

            string courseName = a[0];
            string totalStudents = a[1];
            string totPresent = a[2];

            string[] students = { "Manan", "Murtaza", "Yasir", "Imtiaz" };


            //int totalStudents = int.Parse(a[0]);
            //int totPresent = int.Parse(a[1]);
            List<string> presStudents = new List<string>();

            for (int i= 3; i < a.Length - 1; i++) {
                presStudents.Add(a[i]);

            }

            /*
            foreach (string i in a) {
                if(string.Compare(i, " ")!=0)
                MessageBox.Show(i, "Facial Recognition -> Attendance");
                //Console.WriteLine(i);
            }
            */

            string pres="";
            foreach(string i in presStudents) {
                pres += i;
                pres += "\n";
            }
            //MessageBox.Show(output, "Facial Recognition -> Attendance");
           MessageBox.Show("CourseName:\t\t"+courseName+ "\nTotal Students:\t\t" + totalStudents+ "\nTotal Students Present:\t" + totPresent+"\nPresent Students:\n"+pres, "Automatic Attendance System -> Facial Recognition -> Attendance");
            // Console.WriteLine(output);
            //          Console.ReadKey();
            this.Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            MessageBox.Show("Project Members:\nAbdul Mannan:\t\t16k-3620\nMurtaza Multanwala:\t16k-3618\n\n\n\n" +
                "HCI Project:\tAutomatic Attendance System\n\nTags:\n1)Python\n2)OpenCv\n3)" +
                "facerecognition\n4).NET\n\n\nDescription: It is an automated attendance system" +
                " that marks the attendance of the students by facial recognition. It automates the manual " +
                "task of taking attendance saving much time.\n\nInstructions to follow:\nPress the Take Attendance button" +
                " to start the attendance app. The app works on a single student at a time. The students must be instructed to keep their faces in the center" +
                " of the rectangle shown on the live feed. Once the student is prompted to be as present, their attendance" +
                " is marked and they can move away from the camera", "Automatic Attendance System -> About");
            this.Show();
        }

        
       
        

        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {


            if (e.Control == true && e.KeyCode == Keys.W)
            {
                Button1.PerformClick();
            }
            if (e.Control == true && e.KeyCode == Keys.D)
            {
                button2.PerformClick();
            }
            if (e.Control == true && e.KeyCode == Keys.Q)
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        private void Button1_MouseHover(object sender, EventArgs e)
        {
            t1.Show("Press this button to take attendance\nPlease wait for few seconds after pressing the button\nMake sure the camera is attached", Button1);
        }

        private void Button2_MouseHover(object sender, EventArgs e)
        {
            t1.Show("Press this button to read details about Automatic Attendance System", button2);
        }
    }
}
