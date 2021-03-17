using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Drawing;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace video_player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
      
        System.Windows.Threading.DispatcherTimer dis;
        string[] files;
        string[] files1;
        string[] path1;
        string[] path;
        double store;
        bool isdrag = false;
        Thumb thumb = null;

        public MainWindow()
        {
            InitializeComponent();
            dis = new System.Windows.Threading.DispatcherTimer();
            dis.Interval = TimeSpan.FromMilliseconds(200);
            dis.Tick += new EventHandler(timer_tick);

            position_slider.ApplyTemplate();
            thumb = (position_slider.Template.FindName("PART_Track", position_slider) as Track).Thumb;
            thumb.MouseEnter += new System.Windows.Input.MouseEventHandler(thumb_MouseEnter);
         
        }

        private void thumb_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.MouseDevice.Captured == null)
            {
                MouseButtonEventArgs args = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left);
                args.RoutedEvent = MouseLeftButtonDownEvent;
                (sender as Thumb)?.RaiseEvent(args);
            }
        }

        void InitializePropertyValue()
        {
            volume_slider.Value = MediaElement1.Volume;
            store = volume_slider.Value;
            speed_slider.Value = MediaElement1.SpeedRatio;
        }

        private void Button_Click_Browse(object sender, RoutedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.AddExtension = true;
            of.Filter = "Media Files(*mp4*)|*mp4*|(*mp3*)|*mp3*|(*wmv*)|*wmv*|(*webm*)|*webm*|(*mpg*)|*mpg*|(*3g2*)|*3g2*|(*avi*)|*avi8|(*mkv*)|*mkv*|(*mov*)|*mov*|(*flv*)|*flv*|(*amr*)|*amr*|*wav*|(*wav*)|*ogg*|(*ogg*)|*vob*|(*vob*)";
            if (of.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MediaElement1.Source = new Uri(of.FileName);
                this.Title = System.IO.Path.GetFileName(of.FileName);

                string path = of.FileName;
                string foldername = System.IO.Path.GetDirectoryName(path);
                string filename = System.IO.Path.GetFileName(path);

                MediaElement1.Visibility = Visibility.Visible;
                stop.Visibility = Visibility.Visible;
                Backward.Visibility = Visibility.Visible;
                Forward.Visibility = Visibility.Visible;
                position_slider.Visibility = Visibility.Visible;
                position_lbl.Visibility = Visibility.Visible;
                menu.Visibility = Visibility.Visible;
                volume_slider.Visibility = Visibility.Visible;
                speed_slider.Visibility = Visibility.Visible;
                volume.Visibility = Visibility.Visible;
                speed.Visibility = Visibility.Visible;
                PlayPause_Button.Visibility = Visibility.Visible;
                unmute_mute.Visibility = Visibility.Visible;
                sd.Visibility = Visibility.Visible;
                vol.Visibility = Visibility.Visible;
                textbox1.Visibility = Visibility.Visible;

                System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(foldername);
                System.IO.FileInfo[] fileNames = dirInfo.GetFiles("*.*");
                System.IO.FileInfo fileName = new FileInfo(filename);

                foreach (System.IO.FileInfo fi in fileNames)
                {
                    DateTime dtCreationTime = fi.CreationTime;
                    if (fi.Name == filename)
                    {
                        textbox1.Text = "Properties Of File:" + "\n\n" + "Extension Type:" + "\n" + fi.Extension + "\n\n" + "Directory of File:" + "\n" + fi.DirectoryName + "\n\n" + "File Name:" + "\n" + fi.Name + "\n\n" + "File Length:" + "\n" + fi.Length + "bytes" + "\n\n" + "Date And Time File Created:" + "\n" + dtCreationTime.ToString() + "\n\n" + "Attributes of file:" + "\n" + fi.Attributes;
                    }
                }
                MediaElement1.Play();
                InitializePropertyValue();
                PlayPause_Button.IsChecked = true;
            }
        }

        private void Button_Click_Radio(object sender, RoutedEventArgs e)
        {
            PlayPause_Button.Visibility = Visibility.Visible;
            volume_slider.Visibility = Visibility.Visible;
            speed_slider.Visibility = Visibility.Visible;
            volume.Visibility = Visibility.Visible;
            speed.Visibility = Visibility.Visible;
            stop.Visibility = Visibility.Hidden;
            Backward.Visibility = Visibility.Hidden;
            Forward.Visibility = Visibility.Hidden;
            position_slider.Visibility = Visibility.Hidden;
            position_lbl.Visibility = Visibility.Hidden;
            menu.Visibility = Visibility.Hidden;
            unmute_mute.Visibility = Visibility.Visible;

            MediaElement1.Source = new Uri("http://bbcwssc.ic.llnwd.net/stream/bbcwssc_mp1_ws-einws");
            this.Title = "BBC WORLD SERVICE";
            textbox1.Clear();
            InitializePropertyValue();
            MediaElement1.Play();
            PlayPause_Button.IsChecked = true;
        }

        private void Button_Click_PlayPause(object sender, RoutedEventArgs e)
        {
            if (MediaElement1.Source != null)
            {
                 if (PlayPause_Button.IsChecked == true)
                 {
                    MediaElement1.Play();
                  
                 }
                 else
                 {
                    MediaElement1.Pause();
                 }
            }
            else
            {
                PlayPause_Button.IsChecked = false;
            }
        }

        private void Button_Click_Mute_Unmute(object sender, RoutedEventArgs e)
        {
            

            if (MediaElement1.Source != null)
            {
                if (unmute_mute.IsChecked == true)
                {
                    volume_slider.Value -= 1;
                    MediaElement1.Volume = volume_slider.Value;
                    Mute_Unmute.IsChecked = true;


                }
                else
                {
                    volume_slider.Value = store;
                    Mute_Unmute.IsChecked = false;
                }
            }
            else
            {
                unmute_mute.IsChecked = false;
            }
        }

         private void Volume_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
         {
            var slider = sender as Slider;
            double value = slider.Value;
            vol.Text = value.ToString("0.0");
            MediaElement1.Volume = (double)volume_slider.Value;
            if(value>0)
            {
                InitializePropertyValue();
                unmute_mute.IsChecked = false;
                Mute_Unmute.IsChecked = false;
            }
            else
            {
                Mute_Unmute.IsChecked = true;
                unmute_mute.IsChecked = true;
            }
         }

        private void Button_Click_Mute(object sender, RoutedEventArgs e)
        {
            if (MediaElement1.Source != null)
            {

                if (Mute_Unmute.IsChecked == true)
                {
                    volume_slider.Value -= 1;
                    MediaElement1.Volume = volume_slider.Value;
                    unmute_mute.IsChecked =true;

                }
                else
                {
                    volume_slider.Value = store;
                    unmute_mute.IsChecked = false;

                }
            }
            else
            {
                Mute_Unmute.IsChecked = false;
            }
        }

        private void Button_Click_Stop(object sender, RoutedEventArgs e)
        {
            MediaElement1.Stop();
            PlayPause_Button.IsChecked = false;
        }

       
        private void media_open(object sender, RoutedEventArgs e)
        {
            if (MediaElement1.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = MediaElement1.NaturalDuration.TimeSpan;
                position_slider.Maximum = ts.TotalSeconds;
                position_slider.SmallChange = 1;
                position_slider.LargeChange = Math.Min(10, ts.Seconds / 10);
            }
            dis.Start();
        }

        private void media_end(object sender, RoutedEventArgs e)
        {

            MediaElement1.Stop();
            PlayPause_Button.IsChecked = false;
        }

        void timer_tick(object sender, EventArgs e)
        {
            if (!isdrag)
            {
                position_slider.Value = MediaElement1.Position.TotalSeconds;
                int sec = (int)position_slider.Value;
                TimeSpan time = TimeSpan.FromSeconds(sec);
                string t = time.ToString(@"hh\:mm\:ss");
                position_lbl.Content = t;
            }

        }

        private void Seek_Dragstart(object sender, DragStartedEventArgs e)
        {
            isdrag = true;

        }

        private void Seek_Dragend(object sender, DragCompletedEventArgs e)
        {
            isdrag = false;
            MediaElement1.Position = TimeSpan.FromSeconds(position_slider.Value);
        }

        private void Speed_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            double value = slider.Value;
            sd.Text = value.ToString("0.0");
            MediaElement1.SpeedRatio = (double)speed_slider.Value;
        }

        private void Button_Click_Forward(object sender, RoutedEventArgs e)
        {
            MediaElement1.Position = MediaElement1.Position + TimeSpan.FromSeconds(20);
        }

        private void Button_Click_Backward(object sender, RoutedEventArgs e)
        {
            MediaElement1.Position = MediaElement1.Position - TimeSpan.FromSeconds(20);
        }

        private void MenuItem_Click_JumpForward(object sender, RoutedEventArgs e)
        {
            MediaElement1.Position = MediaElement1.Position + TimeSpan.FromSeconds(20);
        }

        private void MenuItem_Click_JumpBackward(object sender, RoutedEventArgs e)
        {
            MediaElement1.Position = MediaElement1.Position - TimeSpan.FromSeconds(20);
        }

        private void MenuItem_Click_Stop(object sender, RoutedEventArgs e)
        {
            MediaElement1.Stop();
            PlayPause_Button.IsChecked = false;
        }
       
        private void MenuItem_Click_IncreaseSpeed(object sender, RoutedEventArgs e)
        {
            speed_slider.Value += 0.2;
            MediaElement1.SpeedRatio = speed_slider.Value;
        }

        private void MenuItem_Click_DecreaseSpeed(object sender, RoutedEventArgs e)
        {
            speed_slider.Value -= 0.2;
            MediaElement1.SpeedRatio = speed_slider.Value;
        }

        private void MenuItem_Click_IncreaseVolume(object sender, RoutedEventArgs e)
        {
            volume_slider.Value += 0.2;
            MediaElement1.Volume = volume_slider.Value;
        }

        private void MenuItem_Click_DecreaseVolume(object sender, RoutedEventArgs e)
        {
            volume_slider.Value -= 0.2;
            MediaElement1.Volume = volume_slider.Value;
        }

        private void MenuItem_Click_Takess(object sender, RoutedEventArgs e)
        {
            Thread pct = new Thread(ss);
            pct.Start();
        }

        void ss()
        {
            Bitmap memoryImage;
            memoryImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            System.Drawing.Size s = new System.Drawing.Size(memoryImage.Width, memoryImage.Height);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);
            string fileName = string.Format(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Screenshot" + "_" + DateTime.Now.ToString("(dd_MMMM_hh_mm_ss_tt)") + ".png");
            memoryImage.Save(fileName);
            System.Windows.Forms.MessageBox.Show("Screenshot Captured");
        }

        private void Button_Click_Playlist(object sender, RoutedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = true;
            if (of.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (listbox1.Items.Count == 0)
                {
                    files = of.SafeFileNames;
                    path = of.FileNames;

                    for (int i = 0; i < files.Length; i++)
                    {
                        listbox1.Items.Add(files[i]);
                    }
                }
                else
                {
                    if (files.Length > 0 && path.Length > 0)
                    {
                        files1 = of.SafeFileNames;
                        path1 = of.FileNames;
                        Array.Resize(ref files, files.Length + files1.Length);
                        Array.Resize(ref path, path.Length + +path1.Length);
                        files1.CopyTo(files, listbox1.Items.Count);
                        path1.CopyTo(path, listbox1.Items.Count);
                        files = files.Distinct().ToArray();
                        path = path.Distinct().ToArray();

                        for (int i = 0; i < files.Length; i++)
                        {
                            Console.WriteLine(files[i]);
                        }

                        for (int i = 0; i < path.Length; i++)
                        {
                            Console.WriteLine(path[i]);
                        }

                    }

                    for (int i = listbox1.Items.Count; i < files.Length; i++)
                    {
                        listbox1.Items.Add(files[i]);
                    }
                }
            }
        }

        private void Mouse_Doubleclick(object sender, RoutedEventArgs e)
        {

            InitializePropertyValue();
            stop.Visibility = Visibility.Visible;
            Backward.Visibility = Visibility.Visible;
            Forward.Visibility = Visibility.Visible;
            position_slider.Visibility = Visibility.Visible;
            position_lbl.Visibility = Visibility.Visible;
            menu.Visibility = Visibility.Visible;
            MediaElement1.Visibility = Visibility.Visible;
            sd.Visibility = Visibility.Visible;
            vol.Visibility = Visibility.Visible;
            volume.Visibility = Visibility.Visible;
            speed.Visibility = Visibility.Visible;
            volume_slider.Visibility = Visibility.Visible;
            speed_slider.Visibility = Visibility.Visible;
            PlayPause_Button.Visibility = Visibility.Visible;
            unmute_mute.Visibility = Visibility.Visible;
            textbox1.Visibility = Visibility.Visible;

            Uri uri = null;

            uri = new Uri(path[listbox1.SelectedIndex]);
            Console.WriteLine(path[listbox1.SelectedIndex]);

            string s = System.IO.Path.GetFileName(path[listbox1.SelectedIndex]);
            Console.WriteLine(s);
            this.Title = s;

            string s1 = System.IO.Path.GetDirectoryName(path[listbox1.SelectedIndex]);
            Console.WriteLine(s1);

            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(s1);
            System.IO.FileInfo[] fileNames = dirInfo.GetFiles("*.*");

            foreach (System.IO.FileInfo fi in fileNames)
            {
                DateTime dtCreationTime = fi.CreationTime;
                if (fi.Name == s)
                {
                    textbox1.Text = "Properties Of File:" + "\n\n" + "Extension Type:" + "\n" + fi.Extension + "\n\n" + "Directory Of File:" + "\n" + fi.DirectoryName + "\n\n" + "File Name:" + "\n" + fi.Name + "\n\n" + "File Length:" + "\n" + fi.Length + "\n\n" + "Date And Time File ac:" + "\n" + dtCreationTime.ToString() + "\n\n" + "Attributes of file:" + "\n" + fi.Attributes;
                }
            }
            PlayPause_Button.IsChecked = true;
            MediaElement1.Source = uri;
            MediaElement1.Play();
        }

        private void MenuItem_Click_About(object sender, RoutedEventArgs e)
        {
            about ab = new about();
            ab.ShowDialog();
        }

        private void MenuItem_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click_Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MenuItem_Click_Maximize(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
        }
    }
}