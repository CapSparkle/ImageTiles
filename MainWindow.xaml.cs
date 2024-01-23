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
        public MainWindow()
        {
            int width = 1000;
            

            imagesStore = new();

            rootNode = new();
            rootNode.AddLeaf(1);
            var br1 = rootNode.AddBranch();
            br1.AddLeaf(2);
            br1.AddLeaf(3);


            scaler = new(
                imagesStore: imagesStore,
                rootNode: rootNode);

            drawer = new(
                imagesStore,
                startX: 0,
                startY: 0,
                horizontalCompletion: true,
                paddingTop: 5,
                paddingBottom: 10,
                paddingLeft: 5,
                paddingRight: 10);

            InitializeComponent();
        }


        void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            // Clear the canvas
            canvas.Clear(SKColors.White);
            drawer.DrawImageGrid(canvas, rootNode); // Example tile size 100x100
        }
    }
}