using System;
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
using JsonMaker;

namespace PC_Camera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int time_interval = 1;
        string current_time = DateTime.Now.ToString("dddd dd MMMM yyyy hh.mm tt");
        string  current_dir = DateTime.Now.ToString("dddd dd MMMM yyyy")+"\\";
        bool check = false;
        string settings_Path = @"C:\PC Camera\";

        string temp;
        string settings_file_name = "Settings.json";
        Logger logger;
        char slash = '\\';
        string img_path = @"D:\New folder12\PC Camera\";

          
           Reader.Reader read; 

        public MainWindow()
        {
            settings_Path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+"\\PC Camera"+slash;
             string[] names = { "Time_Interval", "Images_Save_Location"};
             string[] values = { time_interval.ToString(), img_path};
            
            InitializeComponent();
            //  location + dir + @"PC Images\" + time + " PC.jpg"
            CameraChoice _CameraChoice = new CameraChoice();
            CameraControl cam = new CameraControl();
            _CameraChoice.UpdateDeviceList();
            var moniker = _CameraChoice.Devices[0].Mon;
            ResolutionList resolutions = Camera.GetResolutionList(moniker);
            DirCheck();
            //settings folder maker
            if (!Directory.Exists(settings_Path))
            {
                Directory.CreateDirectory(settings_Path);
            }

            //logger = new Logger(location + current_dir + "log file.txt", current_time);
            //if setting file exists
            if (!File.Exists(settings_Path + settings_file_name))
            {
                // Create a file to write to.
                JSONMaker maker = new JSONMaker(names.Length, names, values, settings_Path + settings_file_name);
            }
            //using (StreamReader sr = File.OpenText(settings_Path + settings_file_name))
            {
                read = new Reader.Reader(settings_Path + settings_file_name);
                //string s = "";
                //s = sr.ReadLine();
                time_interval = Convert.ToInt32( read.ReturnValueOf("Time_Interval"));
                img_path = read.ReturnValueOf("Images_Save_Location");
                time_out.Text += Convert.ToString(time_interval) + "\n";
                time_out.Text += Convert.ToString(img_path) + "\n";
                LogCheck();
            }

            Res.Text += "Resolutions " + "\n";
            logger.Information("Resolutions", current_time);
            foreach (Resolution r in resolutions)
            {
                Res.Text += Convert.ToString(r + "\n");
                logger.Information(Convert.ToString(r), current_time);
            }

            cam.SetCamera(moniker, resolutions[0]);
            Res.Text += "Starting Camera" + "\n";
            logger.Information("Starting Camera", current_time);
            var timer = new System.Timers.Timer
            {
                Interval = (1000 * 60) * time_interval
            };
            //this will run the ontimed function when time is elapsed
            timer.Elapsed += ontimed;
            temp = "current mins are set to: ";
            Res.Text += temp + Convert.ToString(time_interval) + "\n";
            logger.Information(temp + time_interval, current_time);
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
                        DirCheck();
                        LogCheck();
                        user_image(current_time, current_dir);
                         pc_img(current_time, current_dir);
                        Log_info("Image captured at time ",current_time);
                        if(check==true)
                        {
                            Log_mins("minutes updated to ",current_time,time_interval);
                            temp = "current mins are set to: ";
                            Res.Text += temp + Convert.ToString(time_interval) + "\n";
                            logger.Information(temp + time_interval, current_time);
                            check = false;
                        }
                        timer.Interval = (1000 * 60) * time_interval;
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
                image.Save(img_path + dir + @"PC Images\" + time + " PC.jpg", ImageFormat.Jpeg);

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
            // cam.SetCamera(moniker, resolutions[0]);
                cam.SnapshotSourceImage().Save(img_path+ dir + @"User Images\" + time + " User.png");
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
                if (!Directory.Exists(img_path + current_dir + @"PC Images\"))
                {
                    Directory.CreateDirectory(img_path + current_dir + @"PC Images\");
                }
                if (!Directory.Exists(img_path + current_dir + @"User Images\"))
                {
                    Directory.CreateDirectory(img_path + current_dir + @"User Images\");
                }
            }
            void LogCheck()
            {
                //checks for the log file
                if(logger==null||!logger.Exsists())
                {
                    logger = new Logger(img_path + current_dir + "log file.txt", current_time);
                }
            }

        }
        private void Set_mins_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                time_interval = Convert.ToInt32(mins_num.Text);
                temp = "waiting for minutes to update... ";
                time_out.Text += temp + "\n";
                logger.Information(temp, current_time);
                mins_num.Text = "";
                check = true;
            }
            catch (Exception ex)
            {
                err(ex);
            }
        }

        private void Set_Def_Click(object sender, RoutedEventArgs e)
        {

            try
            {
              
                time_interval = Convert.ToInt32(Def_Mins.Text);
                temp = "waiting for minutes to update... ";
                time_out.Text += temp + "\n";
                logger.Information(temp, current_time);
                Def_Mins.Text = "";
                check = true;
                if (!Directory.Exists(@"C:\Users\ghali\AppData\Local\Temp\PC Camera\"))
                {
                    Directory.CreateDirectory(@"C:\Users\ghali\AppData\Local\Temp\PC Camera\");
                }
                read.ReplaceValueOf("Time_Interval", time_interval.ToString());
            }
            catch (Exception ex)
            {
                err(ex);
            }
        }

        private void img_loc_btn_Click(object sender, RoutedEventArgs e)
        {
            if (img_loc.Text != "")
            {
                try
                {
                    img_path = img_loc.Text + slash;
                    img_loc.Text = "";
                    read.ReplaceValueOf("Images_Save_Location", img_path);
                    temp = "image location changed to: " + img_path;
                    time_out.Text += temp + "\n";
                    logger.Information(temp, current_time);
                }
                catch (Exception ex)
                {
                    err(ex);
                }
            }
        }
        void err(Exception ex)
        {
            time_out.Text += ex.ToString() + "\n";
            current_time = DateTime.Now.ToString("dddd dd MMMM yyyy hh.mm tt");
            logger.Error(ex,current_time);
        }
    }
}