using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
namespace AttendanceSystem
{
    public partial class Form1 : Form
    {
        ToolTip t1 = new ToolTip();//Tips for actions performed by button

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Button1_Click(object sender, EventArgs e)
        {

            //******************************************************************************************************************************************************
            
            new Select().Show();//Select cam source and coursename
            
        }


        public void sendMail(String courseName, String totalStudents,String totPresent,String pres) {//sends mail

            string sender = "";//enter sender email here
            string pass = "";//enter sender's email password here

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(sender);
            mail.To.Add("AddReceiver'sMail");
            mail.Subject = "Attendance Report";
            mail.Body = "CourseName:\t\t" + courseName + "\nTotal Students:\t\t" + totalStudents + "\nTotal Students Present:\t" + totPresent + "\nPresent Students:\n" + pres;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(sender, pass);
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }
        private void Button2_Click(object sender, EventArgs e)//About button
        {
            this.Hide();
            MessageBox.Show("Project Members:\nAbdul Mannan:\t\t16k-3620\nMurtaza Multanwala:\t16k-3618\n\n\n\n" +
                "HCI Project:\tAutomatic Attendance System\n\nTags:\n1)Python\n2)OpenCv\n3)" +
                "facerecognition\n4).NET\n\n\nDescription: It is an automated attendance system" +
                " that marks the attendance of the students by facial recognition. It automates the manual " +
                "task of taking attendance saving much time. We have also added a manual feature so that teacher may take attendance manually if the camera is not working" +
                "" +
                "\n\nInstructions to follow:\n1) Press the Take Attendance button\n2) Specify the course name and source of attendance\n3) Press ok to proceed\n" +
                "\nNote: The app works on a single student at a time. The students must be instructed to keep their faces in the center" +
                " of the rectangle shown on the live feed. Once the student is prompted to be as present, their attendance" +
                " is marked and they can move away from the camera", "Automatic Attendance System -> About");
            this.Show();
        }

        
       
        

        private void Form1_KeyDown_1(object sender, KeyEventArgs e){ //Key presses to simulate button press


            if (e.Control == true && e.KeyCode == Keys.W)//simulates key press of take attendance button
            {
                Button1.PerformClick();
            }
            if (e.Control == true && e.KeyCode == Keys.D)//simulates key press of about button
            {
                button2.PerformClick();
            }
            if (e.Control == true && e.KeyCode == Keys.F)//simulates key press of about button
            {
                button3.PerformClick();
            }

            if (e.Control == true && e.KeyCode == Keys.Q)//closes application
            {
                System.Windows.Forms.Application.Exit();
            }
            
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        private void Button1_MouseHover(object sender, EventArgs e)//Take attendance button
        {
            t1.Show("Press this button to take attendance\nPlease wait for few seconds after pressing the button\nMake sure the camera is attached", Button1);
        }

        private void Button2_MouseHover(object sender, EventArgs e)//About button
        {
            t1.Show("Press this button to read details about Automatic Attendance System", button2);
        }

        

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("Have a nice day","Automatic attendance system");
            System.Windows.Forms.Application.Exit();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            new ShowAttendance().Show();
        }
    }
}
