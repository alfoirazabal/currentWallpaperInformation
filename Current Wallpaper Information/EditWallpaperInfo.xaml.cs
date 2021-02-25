using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Current_Wallpaper_Information.SQLiteInfoExtracter;

namespace Current_Wallpaper_Information
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class EditWallpaperInfo : Window
    {

        private WallpaperInfo CurrentWallpaperInfo;

        public EditWallpaperInfo(WallpaperInfo wallpaperInfo)
        {
            InitializeComponent();
            this.CurrentWallpaperInfo = wallpaperInfo;

            this.lblFileLocation.Content = wallpaperInfo.FileLocation;
            this.txtLocation.Text = wallpaperInfo.Location;
            this.txtSourceLink.Text = wallpaperInfo.SourceLink;
            this.txtDescription.Text = wallpaperInfo.Description;
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentWallpaperInfo.Location = txtLocation.Text;
            this.CurrentWallpaperInfo.SourceLink = txtSourceLink.Text;
            this.CurrentWallpaperInfo.Description = txtDescription.Text;
            this.CurrentWallpaperInfo.Save();
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
