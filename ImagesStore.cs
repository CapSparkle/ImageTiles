using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImageTiles
{
    internal class ImagesStore
    {
        public string directoryName { get; set; } = "images";

        string directoryPath 
        { 
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryName).ToString();
            } 
        }
        string GetPathByUID(int imageUid)
        {
            if((uidToNameMap !=  null) && (uidToNameMap.ContainsKey(imageUid))) 
                return Path.Combine(directoryPath, uidToNameMap[imageUid]).ToString();

            return "";
        }
        public Dictionary<int, string>? uidToNameMap { get; private set; }

        public ImagesStore(string directoryName = "")
        {
            if(directoryName != "")
                this.directoryName = directoryName;

            LoadImages();
        }

        private void LoadImages()
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Directory {directoryPath} does not exist.");
                return;
            }

            uidToNameMap = new();

            var imageFiles = Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly)
                                      .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                                                  || file.EndsWith(".png", StringComparison.OrdinalIgnoreCase));

            int uid = 1;
            foreach (var file in imageFiles)
            {
                uidToNameMap[uid++] = file;
            }
        }

        public SKImage? GetImage(int imageUid)
        {
            var filePath = GetPathByUID(imageUid);
            if (File.Exists(filePath))
            {
                SKImage image;
                using (var stream = new SKFileStream(filePath))
                    image = SKImage.FromBitmap(SKBitmap.Decode(stream));

                return image;
            }
            else
                return null;
        }
    }
}
