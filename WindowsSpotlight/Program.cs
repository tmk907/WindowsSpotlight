using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
//using System.Runtime.InteropServices;

namespace WindowsSpotlight
{
    class Program
    {
        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //private static extern Int32 SystemParametersInfo(UInt32 action, UInt32 uParam, String vParam, UInt32 winIni);

        //private static readonly UInt32 SPI_SETDESKWALLPAPER = 0x14;
        //private static readonly UInt32 SPIF_UPDATEINIFILE = 0x01;
        //private static readonly UInt32 SPIF_SENDWININICHANGE = 0x02;


        static void Main(string[] args)
        {
            var imageExt = ".jpg";

            var spotlightFolder = @"C:\Users\tomek\AppData\Local\Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets";
            var wallpapersFolder = @"C:\Users\tomek\Pictures\Spotlight Wallpapers";
            var newWallpapersFolder = @"C:\Users\tomek\Pictures\Spotlight Wallpapers\New wallpapers";

            Directory.CreateDirectory(wallpapersFolder);
            var newWallpapersFolderInfo = Directory.CreateDirectory(newWallpapersFolder);
            var createdAt = DateTime.MinValue;

            foreach(var file in newWallpapersFolderInfo.GetFiles())
            {
                if(file.CreationTimeUtc > createdAt)
                {
                    createdAt = file.CreationTimeUtc;
                }
            }

            var spotlightFilePaths = Directory.EnumerateFiles(spotlightFolder);

            var newSpotlightFiles = new List<string>();

            foreach (var filePath in spotlightFilePaths)
            {
                if (File.GetCreationTimeUtc(filePath) > createdAt)
                {
                    using (var image = Image.FromFile(filePath))
                    {
                        if (image.Width > image.Height)
                        {
                            newSpotlightFiles.Add(filePath);
                        }
                    }
                }
            }

            if (newSpotlightFiles.Count > 0)
            {
                Directory.Delete(newWallpapersFolder, true);
                Directory.CreateDirectory(newWallpapersFolder);

                string newWallpaperPath = "";

                foreach (var file in newSpotlightFiles)
                {
                    try
                    {
                        File.Copy(file, Path.Combine(wallpapersFolder, Path.GetFileName(file) + imageExt));
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    newWallpaperPath = Path.Combine(newWallpapersFolder, Path.GetFileName(file) + imageExt);
                    File.Copy(file, newWallpaperPath);
                }
            
                //SetWallpaper(newWallpaperPath);
            }

            //try
            //{
            //    var startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            //}
            //catch (Exception)
            //{

            //}
        }

        //private static void SetWallpaper(String path)
        //{
        //    SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        //}
    }
}
