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


namespace PC_Camera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int min = 1;
        string current_time = "";
        string current_dir = "";
        bool check = false;
        string path = @"C:\Users\ghali\AppData\Local\Temp\PC Camera\settings.txt";
        public MainWindow()
        {
            string location = @"D:\New folder12\PC Camera\";

            InitializeComponent();
            CameraChoice _CameraChoice = new CameraChoice();
            CameraControl cam = new CameraControl();
            _CameraChoice.UpdateDeviceList();
            var moniker = _CameraChoice.Devices[0].Mon;
            ResolutionList resolutions = Camera.GetResolutionList(moniker);
            if (!Directory.Exists(@"C:\Users\ghali\AppData\Local\Temp\PC Camera\"))
            {
                Directory.CreateDirectory(@"C:\Users\ghali\AppData\Local\Temp\PC Camera\");
            }

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
            foreach (Resolution r in resolutions)
            {
                Res.Text += Convert.ToString(r + "\n");
            }
            Res.Text += "Starting Camera" + "\n";

            var timer = new System.Timers.Timer();
            timer.Interval = (1000 * 60) * min;
            //this will run the ontimed function when time is elapsed
            timer.Elapsed += ontimed;
            //this starts the timer
            timer.Start();

            void ontimed(object sender, System.Timers.ElapsedEventArgs e)
            {   //the time at which the image got taken
                current_time = DateTime.Now.ToString("dddd dd MMMM yyyy hh.mm tt");
                //the location at which the mage will be stored along with the generic location
                //it is used to store images according to each day
                current_dir = DateTime.Now.ToString("dddd dd MMMM yyyy")+"\\";

                //checks for directories
                if (!Directory.Exists(location + @"PC Images\"+current_dir))
                {
                    Directory.CreateDirectory(location + @"PC Images\" + current_dir);
                }
                if (!Directory.Exists(location + @"User Images\"+current_dir))
                {
                    Directory.CreateDirectory(location + @"User Images\" + current_dir);
                }
                this.Dispatcher.Invoke(() =>
                {
                    try
                {

                        user_image(current_time, current_dir);
                         pc_img(current_time, current_dir);
                        time_out.Text += "image captured at time " + current_time + "\n";
                        if(check==true)
                        {
                        time_out.Text += "minutes updated to " + min + "\n";
                            check = false;
                        }
                        timer.Interval = (1000 * 60) * min;
                }
                catch(Exception ex)
                {
                        time_out.Text += ex.ToString() + "\n";
                    }
                });
            }
            void pc_img(string time,string dir)
            {

                var image = ScreenCapture.CaptureDesktop();
                image.Save(location + @"PC Images\"+dir + time + " PC.jpg", ImageFormat.Jpeg);

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
                cam.SnapshotSourceImage().Save(location + @"User Images\"+dir + time + " User.png");
                // cam.Dispose();
                }
                catch (Exception ex)
                {
                    time_out.Text += ex.ToString() + "\n";
                }

            }

        }

        private void Set_mins_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                min = Convert.ToInt32(mins_num.Text);
                time_out.Text += "waiting for minutes to update... "+ "\n";
                mins_num.Text = "";
                check = true;
            }
            catch(Exception ex)
            {
                time_out.Text += ex.ToString() + "\n";
            }
        }

        private void Set_Def_Click(object sender, RoutedEventArgs e)
        {

            try
            {
              
                min = Convert.ToInt32(Def_Mins.Text);
                time_out.Text += "waiting for minutes to update... " + "\n";
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
            }
        }
    }
}