using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PhotoSauce.MagicScaler;

namespace WindowsSpotlight
{
    class Program
    {
        static void Main(string[] args)
        {
            var imageExt = ".jpg";
            
            var picturesFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            
            var spotlightFolder = Path.Combine(appDataFolder,@"Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets");
            var wallpapersFolder = Path.Combine(picturesFolderPath, @"Spotlight Wallpapers");
            var newWallpapersFolder = Path.Combine(picturesFolderPath, @"Spotlight Wallpapers\New wallpapers");
            
            var logFilePath = Path.Combine(wallpapersFolder, "log.txt");

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
                    var image = ImageFileInfo.Load(filePath);
                    var frame = image.Frames.FirstOrDefault();
                    if (frame.Width > frame.Height)
                    {
                        newSpotlightFiles.Add(filePath);
                    }
                }
            }

            if (newSpotlightFiles.Count > 0)
            {
                try
                {
                    foreach (var file in newWallpapersFolderInfo.GetFiles())
                    {
                        file.Delete();
                    }

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

                }
                catch (Exception ex)
                {
                    File.AppendAllText(logFilePath, ex.ToString());
                }
            }
        }
    }
}
