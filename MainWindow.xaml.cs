using System.Text;
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
        GridDrawer drawer;
        GridNodesScaler scaler;
        GridNode rootNode;
        Padding padding;
        public MainWindow()
        {
            imagesStore = new();

            rootNode = new();
            rootNode.AddLeaf(1);
            var br1 = rootNode.AddBranch();
            br1.AddLeaf(2);
            br1.AddLeaf(3);

            rootNode.AddLeaf(5);
            rootNode.AddLeaf(4);

            padding = new(
                up: 0,
                down: 0,
                left: 0,
                right: 0
                );

            bool verticalCompletion = false;

            scaler = new(
                imagesStore: imagesStore,
                rootNode: rootNode,
                padding: padding,
                mainLength: 1000,
                verticalCompletion: verticalCompletion);

            drawer = new(
                imagesStore,
                startX: 0,
                startY: 0,
                verticalCompletion: verticalCompletion
                );

            InitializeComponent();
        }


        void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            // Clear the canvas
            canvas.Clear(SKColors.White);
            drawer.DrawImageGrid(canvas, rootNode, padding); // Example tile size 100x100
        }
    }
}