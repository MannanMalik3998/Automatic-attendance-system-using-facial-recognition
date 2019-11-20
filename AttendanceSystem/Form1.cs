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

            
            string input = Interaction.InputBox("Select CourseName\n1)HCI\n2)IPT\n3)PIT\n4)IS", "Automatic Attendance System -> Course Name", "HCI", -1, -1);

            if (input == "") {//cancel pressed i.e user changes his/her mind
                return;    
            }

            string course = input.ToUpper();
            temp.courseName = input.ToUpper();

            
            while (!(course.Contains("HCI") || course.Contains("IPT") || course.Contains("PIT") || course.Contains("IS")) ){//user enters wrong name
                input = Interaction.InputBox("Select correct CourseName\n1)HCI\n2)IPT\n3)PIT\n4)IS", "Automatic Attendance System -> Course Name", "HCI", -1, -1);

                if (input == "")
                {//cancel pressed i.e user changes his/her mind
                    return;
                }

                course = input.ToUpper();
                temp.courseName = course;


                //return;
            }



            string cam = Interaction.InputBox("Select attendance source:\n0 - webcam\n1 - external webcam\n2 - Manual attendance", "Automatic Attendance System -> Course Name -> Specify Camera", "0", -1, -1);

            if (cam == ""){//cancel pressed i.e user changes his/her mind
                return;
            }

            while (!(cam.Contains("0")|| cam.Contains("1") || cam.Contains("2") )){//wrong input

                

                cam = Interaction.InputBox("Select correct source:\n0 - webcam\n1 - external webcam\n2 - Manual attendance", "Automatic Attendance System -> Course Name -> Specify Camera", "0", -1, -1);
                if (cam == "")
                {//cancel pressed i.e user changes his/her mind
                    return;
                }
            }

            int camSource = int.Parse(cam);      //input to determine camera source


            if (camSource==2) {//Taking attendance Manually
                this.Hide();
                new Attendance().Show();
                return;
            }

            string fileName = @"E:\Sem7\HCI\ProjAttendanceSystem\HCI\recognize_faces_video.py";    //python script to run    

            #region Execute python script

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(@"C:\Python37\python.exe", fileName) {
                Arguments = string.Format("{0} {1} {2}", fileName, course,camSource),//command line arguments
                RedirectStandardOutput = true,//passing of parameters
                UseShellExecute = false,
                CreateNoWindow = false,//for opening facial recognition window, set it to false
                
            };

            p.Start();//runs the script
            this.Hide();//hides the main screen

            string output = p.StandardOutput.ReadToEnd();//returns output of script
            p.WaitForExit();

            int ErrorResult = checkError(output);
            #endregion

            if (ErrorResult == -1){//error
                this.Show();
            }
            else{//no error
            
                string[] a = output.Split('\n');

                //initializing variables
                string courseName = a[0];
                string totalStudents = a[1];
                string totPresent = a[2];
                string[] students = { "Manan", "Murtaza", "Yasir", "Imtiaz" };
                string pres = "";
                List<string> presStudents = new List<string>();

                for (int i = 3; i < a.Length - 1; i++){
                    presStudents.Add(a[i]);//adding present students in list

                }

                
                foreach (string i in presStudents){
                    pres += i;
                    pres += "\n";
                }

                //Displaying current attendance
                MessageBox.Show("CourseName:\t\t" + courseName + "\nTotal Students:\t\t" + totalStudents + "\nTotal Students Present:\t" + totPresent + "\nPresent Students:\n" + pres, "Automatic Attendance System -> Facial Recognition -> Attendance");

                //Saving current attendance in text file
                System.IO.File.WriteAllText(@"E:\Sem7\HCI\ProjAttendanceSystem\HCI\log.txt", "CourseName:\t\t" + courseName + "\nTotal Students:\t\t" + totalStudents + "\nTotal Students Present:\t" + totPresent + "\nPresent Students:\n" + pres);
                
                //Relaunching main screen
                this.Show();

            }
        }

        public int checkError(string output)
        {
            if (output.Contains("-1")){//if an error occured during execution such as camera malfunction
                MessageBox.Show("An error occurred during execution\nKindly check whether the camera is working properly and then start the application again", "Automatic Attendance System -> Error");

                return -1;
            }
            return 0;//if everything runs smoothly without any error
        }

        public void sendMail(String courseName, String totalStudents,String totPresent,String pres) {//sends mail

            string sender = "";//enter sender email here
            string pass = "";//enter sender's email password here

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(sender);
            mail.To.Add("manan.hameed47@gmail.com");
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
                "task of taking attendance saving much time.\n\nInstructions to follow:\nPress the Take Attendance button" +
                " to start the attendance app. The app works on a single student at a time. The students must be instructed to keep their faces in the center" +
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

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Attendance().Show();
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            System.Windows.Forms.Application.Exit();
        }
        
    }
}
