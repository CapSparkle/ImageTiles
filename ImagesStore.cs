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
        public string directory { get; set; } = "images";
        public Dictionary<int, string>? uidToNameMap { get; private set; }

        public ImagesStore(string directory = "")
        {
            if(directory != "")
                this.directory = directory;

            LoadImages();
        }

        private void LoadImages()
        {
            if (!Directory.Exists(directory))
            {
                Console.WriteLine($"Directory {directory} does not exist.");
                return;
            }

            uidToNameMap = new();

            var imageFiles = Directory.EnumerateFiles(directory, "*.*", SearchOption.TopDirectoryOnly)
                                      .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                                                  || file.EndsWith(".png", StringComparison.OrdinalIgnoreCase));

            int uid = 1;
            foreach (var file in imageFiles)
            {
                uidToNameMap[uid++] = file;
            }
        }

        public (SKBitmap?,SKPaint?) GetImageBitMap(int imageUid)
        {
            if (File.Exists(directory))
            {
                SKBitmap bitmap;
                using (var stream = new SKFileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directory, uidToNameMap[imageUid])))
                    bitmap = SKBitmap.Decode(stream);

                SKPaint paint = new()
                {
                    FilterQuality = SKFilterQuality.High
                };

                return (bitmap, paint);
            }
            else
                return (null, null);
        }
    }
}
