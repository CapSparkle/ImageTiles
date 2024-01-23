﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;

namespace ImageTiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ImagesStore imagesStore;
        ImageGridNode rootNode;
        public MainWindow()
        {

            imagesStore = new();
            rootNode = new();
            rootNode.AddLeaf(1);
            InitializeComponent();
        }

        void DrawImageGrid(SKCanvas canvas, ImageGridNode rootNode, int startX, int startY, int tileWidth, int tileHeight)
        {
            DrawNode(canvas, rootNode, startX, startY, tileWidth, tileHeight);

            foreach (var child in rootNode.childs)
            {
                DrawImageGrid(canvas, child, startX, startY + tileHeight, tileWidth, tileHeight);
                startX += tileWidth;
            }
        }

        void DrawNode(SKCanvas canvas, ImageGridNode node, int x, int y, int width, int height)
        {
            if (node.imageUid < 0) 
                return;

            using var image = imagesStore.GetImage(node.imageUid);
            if (image != null)
            {
                canvas.DrawImage(image, new SKRect(x, y, x + width, y + height));
            }
        }

        //// You need to call this method in your SkiaSharp paint event
        void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            // Clear the canvas
            canvas.Clear(SKColors.White);
            DrawImageGrid(canvas, rootNode, 0, 0, 100, 100); // Example tile size 100x100
        }
    }
}