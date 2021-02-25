using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using static Current_Wallpaper_Information.SQLiteInfoExtracter;

namespace Current_Wallpaper_Information
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private NotifyIcon NotificationIcon = new NotifyIcon();
        private WallpaperInfo CurrentWallpaperInfo;
        private bool WindowIsOpen = false;

        public MainWindow()
        {

            InitializeComponent();

            SetWindowLocation();
            SetNotificationIcon();

            this.ShowInTaskbar = false;

            btnClose.Click += btnClose_Click;
            btnCopyInfo.Click += btnCopyInfo_Click;
            btnChangeInfo.Click += btnChangeInfo_Click;

        }

        private void btnChangeInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.WindowIsOpen = false;
            new EditWallpaperInfo(CurrentWallpaperInfo).Show();
        }

        private void btnCopyInfo_Click(object sender, RoutedEventArgs e)
        {
            string stringedWallpaperInfo = this.CurrentWallpaperInfo.Stringify();
            System.Windows.Clipboard.SetText(stringedWallpaperInfo);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.WindowIsOpen = false;
        }

        public void SetNotificationIcon()
        {
            this.ShowInTaskbar = true;

            NotificationIcon.Icon = new System.Drawing.Icon("./res/icon.ico");
            NotificationIcon.Visible = true;
            NotificationIcon.Text = "Click to get wallpaper information. Right Click to view options";
            NotificationIcon.ContextMenuStrip = new ContextMenuStrip();
            NotificationIcon.ContextMenuStrip.Items.Add("View Current Wallpaper", null, OpenCurrentWallpaperWindow);
            NotificationIcon.ContextMenuStrip.Items.Add("Close", null, CloseProgram);
            NotificationIcon.MouseClick += NotificationIconClick;

            this.Hide();
        }

        private void NotificationIconClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.WindowIsOpen)
                {
                    this.Hide();
                    this.WindowIsOpen = false;
                }
                else
                {
                    OpenCurrentWallpaperWindow(sender, e);
                }
            }
        }

        private void OpenCurrentWallpaperWindow(object sender, EventArgs e)
        {
            string curr_wall = WallpaperPather.GetCurrentWallpaper();
            lblFolderSource.Content = Path.GetDirectoryName(curr_wall);
            lblFileSource.Content = Path.GetFileName(curr_wall);

            this.CurrentWallpaperInfo = SQLiteInfoExtracter.GetFileInformation(curr_wall);
            lblLocation.Content = this.CurrentWallpaperInfo.Location;
            txtbDescription.Text = this.CurrentWallpaperInfo.Description;

            this.CurrentWallpaperInfo.Save();

            this.WindowIsOpen = true;
            this.Show();
        }

        private void CloseProgram(object sender, EventArgs e)
        {
            this.NotificationIcon.Visible = false;
            this.Close();
        }

        public void SetWindowLocation()
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width - 10;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }
    }
}
