using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MouseKeyboardActivityMonitor.WinApi;
using System.Timers;
using System.Windows.Threading;
using System.Diagnostics;

namespace MouseMenu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Normal);
        MouseKeyboardActivityMonitor.MouseHookListener mouse = new MouseKeyboardActivityMonitor.MouseHookListener(new GlobalHooker());

        int movingShowCount = 0;
        int movingHideCount = 0;
        int mouseLeft = 0;
        int mouseTop = 0;

        //how many seconds to wait before show menu (without mouse movement)
        int menuShowDelay = 3;
        //how many seconds to wait before hide menu (without mouse movement)
        int menuHideDelay = 3;
        
        public bool IsVisible
        {
            get 
            { 
                return this.Visibility == System.Windows.Visibility.Visible; 
            }
        }


        public MainWindow()
        {
            InitializeComponent();
        }


        private void HideWindow()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
            movingShowCount = 0;
            movingHideCount = 0;
            timer.Start();
            mouse.Enabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
            this.Topmost = true;

            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += (sd, args) =>
            {

                if (!IsVisible)
                {
                    if (movingShowCount == menuShowDelay)
                    {
                        movingShowCount = 0;
                        this.Left = mouseLeft;
                        this.Top = mouseTop;
                        this.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                        movingShowCount++;
                }
                else
                {
                    if (movingHideCount == menuHideDelay)
                    {
                        movingHideCount = 0;
                        this.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                        movingHideCount++;
                }
            };

            mouse.MouseMove += (sd, args) =>
            {
                movingShowCount = 0;
                movingHideCount = 0;
                mouseLeft = args.X;
                mouseTop = args.Y;
            };

            mouse.Enabled = true;
            timer.Start();
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked " + (sender as Button).Content);
            HideWindow();
        }
    }
}
