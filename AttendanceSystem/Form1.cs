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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            //string fileName = @"E:\Sem7\HCI\lol.py";            
            string fileName = @"E:\Sem7\HCI\ProjAttendanceSystem\HCI\recognize_faces_video.py";        
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(@"C:\Python37\python.exe", fileName){
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false
            };
            p.Start();
            this.Hide();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            MessageBox.Show(output, "Facial Recognition -> Attendance");
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
    }
}
