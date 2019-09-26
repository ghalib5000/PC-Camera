﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;
using Camera_NET;
using System.Timers;
using System.Threading;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;


namespace PC_Camera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int min = 1;
        string current_time = DateTime.Now.ToString("dddd dd MMMM yyyy hh.mm tt");
        string  current_dir = DateTime.Now.ToString("dddd dd MMMM yyyy")+"\\";
        bool check = false;
        string path = @"C:\Users\ghali\AppData\Local\Temp\PC Camera\settings.txt";
        Logger logger;



        public MainWindow()
        {
            string location = @"D:\New folder12\PC Camera\";
            InitializeComponent();
            //  location + dir + @"PC Images\" + time + " PC.jpg"
            CameraChoice _CameraChoice = new CameraChoice();
            CameraControl cam = new CameraControl();
            _CameraChoice.UpdateDeviceList();
            var moniker = _CameraChoice.Devices[0].Mon;
            ResolutionList resolutions = Camera.GetResolutionList(moniker);
            DirCheck();
            if (!Directory.Exists(@"C:\Users\ghali\AppData\Local\Temp\PC Camera\"))
            {
                Directory.CreateDirectory(@"C:\Users\ghali\AppData\Local\Temp\PC Camera\");
            }

            logger = new Logger(location + current_dir + "log file.txt", current_time);
            //if setting file exists
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(min);
                }
            }
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                s = sr.ReadLine();
                min = Convert.ToInt32(s);

               // time_out.Text += Convert.ToString(min);
            }

            Res.Text += "Resolutions " + "\n";
            logger.Information("Resolutions", current_time);
            foreach (Resolution r in resolutions)
            {
                Res.Text += Convert.ToString(r + "\n");
                logger.Information(Convert.ToString(r), current_time);
            }
            Res.Text += "Starting Camera" + "\n";
            logger.Information("Starting Camera", current_time);
            var timer = new System.Timers.Timer();
            timer.Interval = (1000 * 60) * min;
            //this will run the ontimed function when time is elapsed
            timer.Elapsed += ontimed;
            Res.Text += "current mins are set to: " + Convert.ToString(min) + "\n";
            logger.Information("current mins are set to:" + min, current_time);
            //this starts the timer
            timer.Start();

            void ontimed(object sender, System.Timers.ElapsedEventArgs e)
            {   //the time at which the image got taken
                current_time = DateTime.Now.ToString("dddd dd MMMM yyyy hh.mm tt");
                //the location at which the mage will be stored along with the generic location
                //it is used to store images according to each day
                current_dir = DateTime.Now.ToString("dddd dd MMMM yyyy")+"\\";

                
                this.Dispatcher.Invoke(() =>
                {
                    try
                {

                        user_image(current_time, current_dir);
                         pc_img(current_time, current_dir);
                        Log_info("Image captured at time ",current_time);
                        if(check==true)
                        {
                            Log_mins("minutes updated to ",current_time,min);
                            Res.Text += "current mins are set to: " + Convert.ToString(min) + "\n";
                            logger.Information("current mins are set to:" + min, current_time);
                            check = false;
                        }
                        timer.Interval = (1000 * 60) * min;
                }
                catch(Exception ex)
                {
                        time_out.Text += ex.ToString() + "\n";
                        logger.Error(ex, current_time);
                    }
                });
            }
            void pc_img(string time,string dir)
            {

                var image = ScreenCapture.CaptureDesktop();
                image.Save(location + dir + @"PC Images\" + time + " PC.jpg", ImageFormat.Jpeg);

                {
                    /*
                    System.Drawing.Rectangle bounds = Screen.GetBounds(System.Drawing.Point.Empty);
                    using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                    {
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.CopyFromScreen(System.Drawing.Point.Empty, System.Drawing.Point.Empty, bounds.Size);
                        }
                        bitmap.Save("D:\\test.jpg", ImageFormat.Jpeg);
                    }
                    */
                }
            }
            void user_image(string time,string dir)
            {
                this.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        image_taker(time,dir);
                    }
                      catch (Exception ex)
                    {
                             time_out.Text = ex.ToString();
                    }
                });
            }
            void image_taker(string time,string dir)
            {

                try
                {
                //image taker
                cam.SetCamera(moniker, resolutions[0]);
                cam.SnapshotSourceImage().Save(location+ dir + @"User Images\" + time + " User.png");
                // cam.Dispose();
                }
                catch (Exception ex)
                {
                    time_out.Text += ex.ToString() + "\n";
                    logger.Error(ex, current_time);
                }

            }
           
            void Log_info(string info,string time)
            {
                time_out.Text += info + time + "\n";
                logger.Information(info, time);
            }
            void Log_mins(string info, string time,int mins)
            {

                time_out.Text += info + mins + "\n";
                logger.Information(info + mins, time);
            }
            void DirCheck()
            {
                //checks for directories
                if (!Directory.Exists(location + current_dir + @"PC Images\"))
                {
                    Directory.CreateDirectory(location + current_dir + @"PC Images\");
                }
                if (!Directory.Exists(location + current_dir + @"User Images\"))
                {
                    Directory.CreateDirectory(location + current_dir + @"User Images\");
                }
            }

        }
        private void Set_mins_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                min = Convert.ToInt32(mins_num.Text);
                time_out.Text += "waiting for minutes to update... "+ "\n";
                logger.Information("waiting for minutes to update... ", current_time);
                mins_num.Text = "";
                check = true;
            }
            catch(Exception ex)
            {
                time_out.Text += ex.ToString() + "\n";
                logger.Error(ex, current_time);
            }
        }

        private void Set_Def_Click(object sender, RoutedEventArgs e)
        {

            try
            {
              
                min = Convert.ToInt32(Def_Mins.Text);
                time_out.Text += "waiting for minutes to update... " + "\n";
                logger.Information("waiting for minutes to update... ", current_time);
                Def_Mins.Text = "";
                check = true;
                if (!Directory.Exists(@"C:\Users\ghali\AppData\Local\Temp\PC Camera\"))
                {
                    Directory.CreateDirectory(@"C:\Users\ghali\AppData\Local\Temp\PC Camera\");
                }
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.Write(min);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                time_out.Text += ex.ToString() + "\n";
                logger.Error(ex, current_time);
            }
        }
    }
}