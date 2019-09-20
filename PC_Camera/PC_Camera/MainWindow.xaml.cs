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

namespace PC_Camera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            int min = 1;
            DateTime dt = new DateTime();
            InitializeComponent();
            CameraControl cam = new CameraControl();
            CameraChoice _CameraChoice = new CameraChoice();
            //CameraControl cam = new CameraControl();
            Camera c = new Camera();
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
            timer.Interval = 5000;
            timer.Elapsed += ontimed;
            timer.Start();
             void ontimed(object sender, System.Timers.ElapsedEventArgs e)
                {
                test();
            }
            void test()
            {
                this.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        time_out.Text += Convert.ToString(DateTime.Now) + "\n";
                    }
                    catch (Exception ex)
                    {
                        time_out.Text = ex.ToString();
                    }
                });

            }
            void image_taker()
            {
                //image taker
                cam.SetCamera(moniker, resolutions[0]);
                cam.SnapshotSourceImage().Save("D:\\out.png");
                cam.Dispose();
            }

        }
    }
}
