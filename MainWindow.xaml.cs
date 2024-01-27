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
            var br11 = br1.AddBranch();
            br11.AddLeaf(2);
            br11.AddLeaf(3);
            br1.AddLeaf(4);

            var b2 = rootNode.AddBranch();
            b2.AddLeaf(1);
            var b21 = b2.AddBranch();
            b21.AddLeaf(7);
            b21.AddLeaf(6);
            var b211 = b21.AddBranch();
            b211.AddLeaf(2);
            b211.AddLeaf(5);
            var b212 = b21.AddBranch();
            b212.AddLeaf(7);
            b212.AddLeaf(4);
            var b3 = rootNode.AddBranch();
            b3.AddLeaf(1);
            b3.AddLeaf(3);
            b3.AddLeaf(5);
            b21.AddLeaf(6);

            DrawStoryboard(
                width: 1000, 
                paddingTop: 5, 
                paddingRight: 5, 
                paddingBottom: 5, 
                paddingLeft: 5
            );

            
            InitializeComponent();
        }

        void DrawStoryboard(int width, int paddingTop, int paddingRight, int paddingBottom, int paddingLeft)
        {
            padding = new(
                up: paddingTop,
                down: paddingBottom,
                left: paddingLeft,
                right: paddingRight
                );

            bool verticalFilling = false;

            scaler = new(
                imagesStore: imagesStore,
                rootNode: rootNode,
                padding: padding,
                mainWidth: width,
                verticalFilling: verticalFilling);

            drawer = new(
                imagesStore,
                startX: 0,
                startY: 0,
                verticalFilling: verticalFilling
                );
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