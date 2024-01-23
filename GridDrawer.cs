using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImageTiles
{
    internal class GridDrawer
    {
        private readonly ImagesStore imagesStore;

        int startX, startY;
        bool verticalCompletion = false;

        Padding padding; 

        public GridDrawer(ImagesStore imagesStore,
            int startX,
            int startY, 
            bool verticalCompletion
            )
        {
            this.imagesStore = imagesStore;
            this.startX = startX;
            this.startY = startY;
            this.verticalCompletion = verticalCompletion;
        }

        public void DrawImageGrid(SKCanvas canvas, GridNode rootNode, Padding padding)
        {
            this.padding = padding;
            int currentX = startX,
                currentY = startY;

            DrawImageNode(canvas, rootNode, ref currentX, ref currentY, !verticalCompletion);    
        }

        void DrawImageNode(SKCanvas canvas, GridNode rootNode, ref int currentX, ref int currentY, bool verticalCompletion)
        {
            if (rootNode.isLeaf) 
            {
                DrawImage(canvas, rootNode, ref currentX, ref currentY, verticalCompletion);
            }
            else
            {
                int newX = currentX,
                    newY = currentY;

                if (verticalCompletion)
                    currentY += ((int)rootNode.height + padding.Up + padding.Bottom);
                else
                    currentX += ((int)rootNode.width + padding.Left + padding.Right);

                verticalCompletion = !verticalCompletion;
                foreach (var child in rootNode.childs)
                {
                    DrawImageNode(canvas, child, ref newX, ref newY, verticalCompletion);
                }
            }
        }

        void DrawImage(SKCanvas canvas, GridNode node, ref int currentX, ref int currentY, bool verticalCompletion)
        {
            int imgUidToDraw = node.imageUid;
            if (node.imageUid <= 0)
                imgUidToDraw = 1;
            //add img filler

            var image = imagesStore.GetImage(imgUidToDraw);
            if (image != null)
            {
                canvas.DrawImage(image, new SKRect(
                    currentX + padding.Left, 
                    currentY + padding.Up, 
                    currentX + padding.Left + node.width, 
                    currentY + padding.Up + node.height));

                image.Dispose();
            }

            

            if (verticalCompletion)
            {
                currentY += ((int)node.height + padding.Up + padding.Bottom);
            }
            else
                currentX += ((int)node.width + padding.Left + padding.Right);
        }

    }
}
