using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTiles
{
    internal class GridDrawer
    {
        private readonly ImagesStore imagesStore;

        int startX, startY;
        int currentX, currentY;
        bool verticalCompletion = false;

        int
            paddingTop = 5,
            paddingBottom = 10,
            paddingLeft = 5,
            paddingRight = 15;

        public GridDrawer(ImagesStore imagesStore,
            int startX,
            int startY, 
            bool horizontalCompletion, 
            int paddingTop = 0, 
            int paddingBottom = 0, 
            int paddingLeft = 0,
            int paddingRight = 0)
        {
            this.imagesStore = imagesStore;
            this.startX = startX;
            this.startY = startY;
            this.currentX = currentX;
            this.currentY = currentY;
            this.verticalCompletion = horizontalCompletion;
            this.paddingTop = paddingTop;
            this.paddingBottom = paddingBottom;
            this.paddingLeft = paddingLeft;
            this.paddingRight = paddingRight;
        }

        public void DrawImageGrid(SKCanvas canvas, GridNode rootNode)
        {
            currentX = startX;
            currentY = startY;
            DrawImageNode(canvas, rootNode, ref currentX, ref currentY, verticalCompletion);    
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

            using var image = imagesStore.GetImage(imgUidToDraw);
            if (image != null)
            {
                canvas.DrawImage(image, new SKRect(
                    currentX + paddingLeft, 
                    currentY + paddingTop, 
                    currentX + paddingLeft + node.width, 
                    currentY + paddingTop + node.height));
            }

            if (verticalCompletion)
            {
                currentY += (node.height + paddingTop + paddingBottom);
            }
            else
                currentX += (node.width + paddingLeft + paddingRight);
        }

    }
}
