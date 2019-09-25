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

namespace PC_Camera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int min = 5;
        string current_time = "";
        public MainWindow()
        {
            string location = @"D:\New folder12\PC Camera\";

            InitializeComponent();
            CameraChoice _CameraChoice = new CameraChoice();
            CameraControl cam = new CameraControl();
            _CameraChoice.UpdateDeviceList();
            var moniker = _CameraChoice.Devices[0].Mon;
            ResolutionList resolutions = Camera.GetResolutionList(moniker);

            Res.Text += "Resolutions " + "\n";
            foreach (Resolution r in resolutions)
            {
                Res.Text += Convert.ToString(r + "\n");
            }
            Res.Text += "Starting Camera" + "\n";

            var timer = new System.Timers.Timer();
            timer.Interval = 1000 * min;
            timer.Elapsed += ontimed;
            timer.Start();
            void ontimed(object sender, System.Timers.ElapsedEventArgs e)
            {
                current_time = DateTime.Now.ToString("dddd dd MMMM yyyy hh.mm.ss tt");

                test(current_time);
                pc_img(current_time);
                time_out.Text = "image captured at time " + current_time+"\n";
                timer.Interval = (1000 * 60) * min;
            }
            void pc_img(string time)
            {

                var image = ScreenCapture.CaptureDesktop();
                image.Save(location + @"PC Images\" + time + " PC.jpg", ImageFormat.Jpeg);

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
            void test(string time)
            {
                this.Dispatcher.Invoke(() =>
                {
                    //   try
                    {

                        image_taker(time);
                    }
                    //  catch (Exception ex)
                    {
                        //     time_out.Text = ex.ToString();
                    }
                });

            }
            void image_taker(string time)
            {

                //image taker

                cam.SetCamera(moniker, resolutions[0]);
                cam.SnapshotSourceImage().Save(location + @"User Images\" + time + " User.png");
                // cam.Dispose();
            }

        }

        private void Set_mins_Click(object sender, RoutedEventArgs e)
        {
            min = Convert.ToInt32(mins_num.Text);
        }
    }
}