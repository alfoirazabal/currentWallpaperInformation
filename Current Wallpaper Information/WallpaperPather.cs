using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Current_Wallpaper_Information
{

    // Source: https://www.whitebyte.info/programming/why-being-a-programmer-ist-great-or-how-to-get-the-current-windows-wallpaper-in-c#:~:text=The%20current%20wallpaper%20path%20can,at%20least%20in%20Windows%208.1).
    class WallpaperPather
    {

        private static readonly string REGISTRY_FOLDER_PATH = "Control Panel\\Desktop";
        private static readonly string REGISTRY_FILE_NAME = "TranscodedImageCache";

        // Source: http://stackoverflow.com/a/406576/441907
        private static byte[] SliceMe(byte[] source, int pos)
        {
            byte[] destfoo = new byte[source.Length - pos];
            Array.Copy(source, pos, destfoo, 0, destfoo.Length);
            return destfoo;
        }

        public static string GetCurrentWallpaper()
        {
            byte[] path = (byte[])Registry.CurrentUser.OpenSubKey(REGISTRY_FOLDER_PATH).GetValue(REGISTRY_FILE_NAME);
            String wallpaper_file = Encoding.Unicode.GetString(SliceMe(path, 24)).TrimEnd("\0".ToCharArray());
            return wallpaper_file;
        }

    }
}
